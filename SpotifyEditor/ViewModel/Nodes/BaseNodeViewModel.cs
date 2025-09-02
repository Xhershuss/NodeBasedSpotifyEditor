using Microsoft.Win32;
using SpotifyEditor.Helpers;
using SpotifyEditor.Helpers.FileController;
using SpotifyEditor.Model;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View;
using SpotifyEditor.View.Nodes;
using SpotifyEditor.View.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace SpotifyEditor.ViewModel.Nodes
{
    public class BaseNodeViewModel
    {

        public NodeService _nodeService { get; } = NodeService.Instance;

       
        public INodeView _viewInterface { get; set; }


        public RelayCommand<NodeCommandParameter> AddNode { get; set; }
        public RelayCommand<BaseNodeModel> RemoveNode { get; set; }

        public RelayCommand<string> SaveNodes { get; set; }
        public RelayCommand<ProjectDto> LoadExistingNodes { get; set; }
        public RelayCommand<object> ClearNodes{ get; set; }
        public RelayCommand<object> ResetNodes { get; set; }
        public AsyncRelayCommand TriggerExecution { get; set; }


        public BaseNodeViewModel(Canvas boardCanvas) {


            AddNode = new RelayCommand<NodeCommandParameter>(param => {

               
                var position = param.Position;

               
                var viewModel = NodeFactory.CreateNodeViewModel(param.NodeType,boardCanvas);

                var node = viewModel.CreateNode(position);

                node.NodeType = param.NodeType;

                //For loading existing nodes from file
                if (param.HasNodeId)
                {
                    node.Id = param.NodeId;
                    for (int i = 0; i < node.InputCount; i++)
                    {
                        node.InputPorts[i].Id = param.InputPortIds[i];
                        node.InputPorts[i].Owner = node;
                    }
                    for (int i = 0; i < node.OutputCount; i++)
                    {
                        node.OutputPorts[i].Id = param.OutputPortIds[i];
                        node.OutputPorts[i].Owner = node;
                    }
                }


                node.ViewModelReference = viewModel;

                _viewInterface = NodeFactory.CreateNodeView(param.NodeType,node, boardCanvas) ;

                var _view = _viewInterface as UIElement
                    ?? throw new InvalidCastException("INodeView must be a UIElement");

                _viewInterface.AddPorts(node.InputPorts, node.OutputPorts);

                _viewInterface.LoadPortPositions(node.InputPorts, node.OutputPorts);

                Canvas.SetLeft(_view, node.Position.X);
                Canvas.SetTop(_view, node.Position.Y);
                boardCanvas.Children.Add(_view);

               

                _nodeService.AllNodes.Add(node);
            });

            TriggerExecution = new AsyncRelayCommand(async data => {

                if (Interlocked.Exchange(ref _nodeService.IsAllSystemsExecuted, 1) == 1) return;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    var triggerButton = (Button)mainWindow.FindName("TriggerButton");
                    if (triggerButton != null)
                        triggerButton.IsEnabled = false;
                });

                _nodeService.DetermineStartAndEndNodes(); 
                var executionContext= new DataExecutionContext
                {
                    Data = data as List<object>,
                    Metadata = new Dictionary<string, object>(),
                };

                await _nodeService.Execute(_nodeService._startNodes);

            });

            RemoveNode = new RelayCommand<BaseNodeModel>(node => {
                
    
                foreach (var inputPort in node.InputPorts)
                {
                    var successedConnections = InputPortView.SuccessedConnections.Where(s => s.Start == inputPort.ViewReference);
                    foreach (var connection in successedConnections.ToList())
                    {
      
                        boardCanvas.Children.Remove(connection.Line);
                        InputPortView.SuccessedConnections.Remove(connection);
                    }

                    foreach (var connectedNode in inputPort.ConnectedNodes.ToList())
                    {
                        foreach (var outputPort in connectedNode.OutputPorts)
                        {
                            outputPort.ConnectedInputPorts.Remove(inputPort);
                            outputPort.ConnectedNodes.Remove(node);
                        }
                    }
                    inputPort.ConnectedNodes.Clear();
                    inputPort.ConnectedOutputPorts.Clear();
                }


                foreach (var outputPort in node.OutputPorts)
                {
                    var successedConnections = OutputPortView.SuccessedConnections.Where(s => s.Start == outputPort.ViewReference);
                    foreach (var connection in successedConnections.ToList())
                    {

                        boardCanvas.Children.Remove(connection.Line);
                        OutputPortView.SuccessedConnections.Remove(connection);
                    }

                    foreach (var connectedNode in outputPort.ConnectedNodes.ToList())
                    {
                        foreach (var inputPort in connectedNode.InputPorts)
                        {
                            inputPort.ConnectedOutputPorts.Remove(outputPort);
                            inputPort.ConnectedNodes.Remove(node);
                        }
                    }
                    outputPort.ConnectedNodes.Clear();
                    outputPort.ConnectedInputPorts.Clear();
                }


                _nodeService.AllNodes.Remove(node);

                var _view = node.ViewReference as UIElement;

                if (_view is UIElement view)
                {
                    boardCanvas.Children.Remove(view);
                }
            });

            LoadExistingNodes = new RelayCommand<ProjectDto>(k => {

                NodeService.Instance.AllNodes.ToList().ForEach(node => RemoveNode.Execute(node));

                var filePath = FileService.SelectFile();
                var o = FileService.Load(filePath);

                if (o == null)
                {
                    MessageBox.Show("Failed to load the project file.");
                    return;
                }

                var nodeDatas = o.Nodes;
                var connectionDatas = o.Connections;

                
                foreach (var node in nodeDatas) {
                    AddNode.Execute(new NodeCommandParameter
                    {
                        VisibleName = node.Name,
                        NodeType = node.NodeType,
                        NodeId = node.Id,
                        Position = new System.Drawing.Point(node.PosX, node.PosY),
                        InputPortIds = node.InputPorts.Select(p => p.PortId).ToList(),
                        OutputPortIds = node.OutputPorts.Select(p => p.PortId).ToList()
                    });
                };

           
                foreach (var connection in connectionDatas)
                {

                    var inPort = InputPortModel.GetPortModelById(connection.StartPort.PortId);
                    var outPort = OutputPortModel.GetPortModelById(connection.EndPort.PortId);

                    var startNode = outPort?.Owner;
                    var  endNode= inPort?.Owner;

                    inPort.ConnectedNodes.Add(startNode);
                    outPort.ConnectedNodes.Add(endNode);

                    inPort.ConnectedOutputPorts.Add(outPort);
                    outPort.ConnectedInputPorts.Add(inPort);


                    var line = new Line();
                    outPort.ViewReference.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var startCenter = new System.Windows.Point(outPort.ViewReference.ActualWidth / 2, outPort.ViewReference.ActualHeight / 2);
                        var endCenter = new System.Windows.Point(inPort.ViewReference.ActualWidth / 2, inPort.ViewReference.ActualHeight / 2);

                        var startPoint = outPort.ViewReference.TranslatePoint(startCenter, boardCanvas);
                        var endPoint = inPort.ViewReference.TranslatePoint(endCenter, boardCanvas);

                        var line = new Line
                        {
                            Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1ED760")),
                            StrokeThickness = 2,
                            IsHitTestVisible = false,
                            X1 = startPoint.X,
                            Y1 = startPoint.Y,
                            X2 = endPoint.X,
                            Y2 = endPoint.Y,
                            Effect = new DropShadowEffect
                            {
                                Color = System.Windows.Media.Color.FromRgb(50, 255, 100),
                                BlurRadius = 20,
                                ShadowDepth = 0,
                                Opacity = 1.0
                            }
                        };

                        OutputPortView.SuccessedConnections.Add(
                            (outPort.ViewReference,
                            inPort.ViewReference,
                            line));
                        InputPortView.SuccessedConnections.Add(
                            (inPort.ViewReference,
                            outPort.ViewReference,
                            line));
                        boardCanvas.Children.Add(line);

                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                   


                }
                

            });

            ClearNodes = new RelayCommand<object>(o => {
                _nodeService.AllNodes.ToList().ForEach(node => RemoveNode.Execute(node));
            });

            SaveNodes  = new RelayCommand<string>(o => {

                foreach (var node in _nodeService.AllNodes)
                {
                    node.IsExecuted = false;
                    node.ReverseIndicator();
                    foreach (var inputPort in node.InputPorts)
                    {
                        inputPort.InputPortData = null;
                    }
                    foreach (var outputPort in node.OutputPorts)
                    {
                        outputPort.OutputPortData = null;
                    }
                }

                var filepath = FileService.SelectFolder();
                if (string.IsNullOrEmpty(filepath))
                    return;
                FileService.Save( filepath);
            });

            ResetNodes = new RelayCommand<object>(o => {

                foreach(var node in _nodeService.AllNodes)
                {
                    node.IsExecuted = false;
                    node.ReverseIndicator();
                    foreach(var inputPort in node.InputPorts)
                    {
                        inputPort.InputPortData = null;
                    }
                    foreach(var outputPort in node.OutputPorts)
                    {
                        outputPort.OutputPortData = null;
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    var triggerButton = (Button)mainWindow.FindName("TriggerButton");
                    if (triggerButton != null)
                        triggerButton.IsEnabled = true;
                });
                Interlocked.Exchange(ref _nodeService.IsAllSystemsExecuted, 0);
            } );
        }

        public BaseNodeViewModel() { }




        // currently used for overloading CreateNode method in derived classes
        // will be added Generic type T later 
        public virtual BaseNodeModel CreateNode(System.Drawing.Point pos) 
        {
            
            var node = new BaseNodeModel();
            node.Position = pos;
            node.Id = Guid.NewGuid().ToString();
    

            node.InputPorts = new ObservableCollection<InputPortModel>() { };
            node.OutputPorts= new ObservableCollection<OutputPortModel>() { };


            for(int i =0;i<node.InputCount;i++){
                node.InputPorts.Add( new InputPortModel());
            }
            for (int i = 0; i < node.OutputCount; i++) {
                node.OutputPorts.Add(new OutputPortModel());
            }


            return node;

        }

	}
}

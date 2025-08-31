using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View.Nodes.Ports;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SpotifyEditor.View.Nodes
{
    /// <summary>
    /// Interaction logic for BaseNode.xaml
    /// </summary>
    public partial class BaseNodeView : UserControl, INodeView
    {
        private bool _isDragging;
        private Point _mouseStartPos;
        private double _origLeft, _origTop;

        BaseNodeView? curNode;

        public BaseNodeModel nodeModel { get; set; }

        public Canvas _canvas { get; set; }
        public BaseNodeView()
        {
            InitializeComponent();

        }


    

        public void AddPorts(ObservableCollection<InputPortModel> inputPorts,
                             ObservableCollection<OutputPortModel> outputPorts)
        {
            

           foreach (var inputPort in inputPorts)
           {
               var portView = new InputPortView(inputPort );
               inputPortContainer.Children.Add(portView);
           }

           foreach (var outputPort in outputPorts) {
                var portView = new OutputPortView(outputPort);
                outputPortContainer.Children.Add(portView);
           }
       }

      
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _mouseStartPos = e.GetPosition(this.Parent as UIElement);

            _origLeft = Canvas.GetLeft(this);
            _origTop = Canvas.GetTop(this);

            this.CaptureMouse();
            e.Handled = true;

            curNode = this;


        }

        private void OnMouseMove(object sender, MouseEventArgs e) {

            if (!_isDragging)
                return;

            Mouse.OverrideCursor = Cursors.Hand;

            var currentPos = e.GetPosition(this.Parent as UIElement);
            var offset = currentPos - _mouseStartPos;

            Canvas.SetLeft(this, _origLeft + offset.X);
            Canvas.SetTop(this, _origTop + offset.Y);

            if (curNode == null)
                return;

            foreach (var port in curNode.outputPortContainer.Children.OfType<OutputPortView>())
            {
                if(port.SuccesedConnections.Count != 0 )
                {
  
                    for (int i = 0; i < port.SuccesedConnections.Count; i++)
                    {
                        var (startPort, endPort, line) = port.SuccesedConnections[i];

                        var sP = startPort.TranslatePoint(new Point(port.Width / 2, port.Height / 2), _canvas);
                        var eP = endPort.TranslatePoint(new Point(port.Width / 2, port.Height / 2), _canvas);
                        line.X1 = sP.X;
                        line.Y1 = sP.Y;
                        line.X2 = eP.X;
                        line.Y2 = eP.Y;
                        port.SuccesedConnections[i] = (startPort, endPort, line);
                    }

                }
            }

            foreach (var port in curNode.inputPortContainer.Children.OfType<InputPortView>())
            {
                if (port.SuccesedConnections.Count != 0)
                {
                    

                    for (int i = 0; i < port.SuccesedConnections.Count; i++)
                    {
                        var (startPort, endPort, line) = port.SuccesedConnections[i];
            
                        var sP = startPort.TranslatePoint(new Point(port.Width / 2, port.Height / 2), _canvas);
                        var eP = endPort.TranslatePoint(new Point(port.Width / 2, port.Height / 2), _canvas);
                        line.X1 = sP.X;
                        line.Y1 = sP.Y;
                        line.X2 = eP.X;
                        line.Y2 = eP.Y;
                        port.SuccesedConnections[i] = (startPort, endPort, line);
                    }
                }
            }

        }

   

        private void OnMouseLeftButtonUp(object sender, MouseEventArgs e) { 
        
            if (!_isDragging)
                return;

            _isDragging = false;   
            Mouse.OverrideCursor = null; 
            this.ReleaseMouseCapture();   
            e.Handled = true;
        }

      
    }
}

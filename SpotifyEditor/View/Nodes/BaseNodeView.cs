using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View.Nodes.Common;
using SpotifyEditor.View.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpotifyEditor.View.Nodes
{
    public class BaseNodeView : UserControl, INodeView
    {
        public BaseNodeModel nodeModel { get; set; }

        private NodeSelection selectionInstance = NodeSelection.Instance;

        public Canvas _canvas { get; set; }

        public virtual StackPanel OutputPortContainer{ get; set; }
        public virtual StackPanel InputPortContainer { get; set; }


        private bool _isDragging;
        private Point _mouseStartPos;
        private double _origLeft, _origTop;


        INodeView? curNode;


        public BaseNodeView()
        {
            this.MouseLeftButtonDown += Base_OnMouseLeftButtonDown;
            this.MouseLeftButtonUp += Base_OnMouseLeftButtonUp;
            this.MouseMove += Base_OnMouseMove;
        }

       

 
  
        public void LoadPortPositions(ObservableCollection<InputPortModel> inputPorts,
                            ObservableCollection<OutputPortModel> outputPorts)
        {
       
            foreach (var inputPort in inputPorts)
            {
                var portView = inputPort.ViewReference;
                var center = new System.Windows.Point(portView.Width / 2.0, portView.Height / 2.0);
                portView.Loaded += (s, e) =>
                {
                    var p = portView.TranslatePoint(center, _canvas);
                    inputPort.Position = p;
                };
            }
            foreach (var outputPort in outputPorts)
            {
                var portView = outputPort.ViewReference;
                var center = new System.Windows.Point(portView.Width / 2.0, portView.Height / 2.0);
                portView.Loaded += (s, e) =>
                {
                    var p = portView.TranslatePoint(center, _canvas);
                    outputPort.Position = p;
                };
            }
        }

        public void AddPorts(ObservableCollection<InputPortModel> inputPorts,
                             ObservableCollection<OutputPortModel> outputPorts)
        {


            foreach (var inputPort in inputPorts)
            {
                var portView = new InputPortView(inputPort);
                InputPortContainer.Children.Add(portView);
                
            }

            foreach (var outputPort in outputPorts)
            {
                var portView = new OutputPortView(outputPort);
                OutputPortContainer.Children.Add(portView);
            }
        }

        public void Base_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _mouseStartPos = e.GetPosition(this.Parent as UIElement);

            _origLeft = Canvas.GetLeft(this);
            _origTop = Canvas.GetTop(this);

            this.CaptureMouse();
            e.Handled = true;

            curNode = this;

            selectionInstance.SelectedNode = curNode;
            nodeModel.SelectedNodeBorder = Visibility.Visible;

        }

        public void Base_OnMouseMove(object sender, MouseEventArgs e)
        {

            if (!_isDragging)
                return;

            Mouse.OverrideCursor = Cursors.Hand;

            var currentPos = e.GetPosition(this.Parent as UIElement);
            var offset = currentPos - _mouseStartPos;

            nodeModel.Position = new System.Drawing.Point((int)(_origLeft + offset.X), (int)(_origTop + offset.Y));

            Canvas.SetLeft(this, _origLeft + offset.X);
            Canvas.SetTop(this, _origTop + offset.Y);

            if (curNode == null)
                return;

            foreach (var (startPort, endPort, line) in OutputPortView.SuccessedConnections)
            {
                var sP = startPort.TranslatePoint(
                    new Point(startPort.Width / 2, startPort.Height / 2), _canvas);
                var eP = endPort.TranslatePoint(
                    new Point(endPort.Width / 2, endPort.Height / 2), _canvas);

                line.X1 = sP.X;
                line.Y1 = sP.Y;
                line.X2 = eP.X;
                line.Y2 = eP.Y;

                if (startPort.portModel != null)
                    startPort.portModel.Position = new System.Windows.Point(sP.X, sP.Y);
                if (endPort.portModel != null)
                    endPort.portModel.Position = new System.Windows.Point(eP.X, eP.Y);
            }

            foreach (var (startPort, endPort, line) in InputPortView.SuccessedConnections)
            {
                var sP = startPort.TranslatePoint(
                    new Point(startPort.Width / 2, startPort.Height / 2), _canvas);
                var eP = endPort.TranslatePoint(
                    new Point(endPort.Width / 2, endPort.Height / 2), _canvas);

                line.X1 = sP.X;
                line.Y1 = sP.Y;
                line.X2 = eP.X;
                line.Y2 = eP.Y;

                if (startPort.portModel != null)
                    startPort.portModel.Position = new System.Windows.Point(sP.X, sP.Y);
                if (endPort.portModel != null)
                    endPort.portModel.Position = new System.Windows.Point(eP.X, eP.Y);
            }

        }


        public void Base_OnMouseLeftButtonUp(object sender, MouseEventArgs e)
        {

            if (!_isDragging)
                return;

            _isDragging = false;
            Mouse.OverrideCursor = null;
            this.ReleaseMouseCapture();
            e.Handled = true;

            if (selectionInstance.selectedNode == this)
            {
                nodeModel.SelectedNodeBorder = Visibility.Hidden;

            }
        }

    }
}

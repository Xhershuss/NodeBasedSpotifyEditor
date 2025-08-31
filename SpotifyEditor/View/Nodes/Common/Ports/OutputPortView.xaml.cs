using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotifyEditor.View.Nodes.Ports
{

    public partial class OutputPortView : UserControl
    {
        private bool _isPressed;
        private Point _mousePos;
        private Point _startPort;
        private Line _line;
        
        private Board _board;
        public string Id => portModel.Id;
        public string OwnerId => portModel.Owner.Id;


        public static List<(OutputPortView Start, InputPortView End, Line Line)> SuccessedConnections { get; set; }
                            = new List<(OutputPortView Start, InputPortView End, Line Line)>();
        public OutputPortModel portModel{ get; set; }
        public OutputPortView(OutputPortModel model)
        {
            InitializeComponent();
            portModel = model;
            model.ViewReference = this;

            var mainWin = Application.Current.MainWindow as MainWindow;
            _board = mainWin.GetMainBoard();

            outputPortEllipse.Tag = this;
        }
        public OutputPortView() {  }
        public Ellipse OutputPortEllipse => outputPortEllipse;


        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;

            double centerX = outputPortEllipse.Width / 2;
            double centerY = outputPortEllipse.Height / 2;

            _startPort = outputPortEllipse.TranslatePoint(
                   new Point(centerX, centerY),
                   _board.ForegroundCanvas);


            _line = new Line()
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                IsHitTestVisible = false,
                Effect = new DropShadowEffect
                {
                    Color = Colors.White,
                    BlurRadius = 20,
                    ShadowDepth = 0,
                    Opacity = 1.0
                }
            };

            _line.X1 = _startPort.X;
            _line.Y1 = _startPort.Y;


            _line.X2 = _line.X1;
            _line.Y2 = _line.Y1;

            _board.ForegroundCanvas.Children.Add(_line);


            outputPortEllipse.CaptureMouse();


            e.Handled = true;

        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {

            if (!_isPressed)
                return;

            _mousePos = e.GetPosition(_board);

            _line.X2 = _mousePos.X;
            _line.Y2 = _mousePos.Y;

        }

        private void OnMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (!_isPressed) return;

            _isPressed = false;
            outputPortEllipse.ReleaseMouseCapture();
            e.Handled = true;




            Point _mousePos = Mouse.GetPosition(_board.ForegroundCanvas);
            var hit = _board.ForegroundCanvas.InputHitTest(_mousePos) as DependencyObject;



            var inputPort = VisualTreeHelpers.FindParentOfType<InputPortView>(hit);

            if (inputPort != null)
            {

                Point center = inputPort.TranslatePoint(
                    new Point(inputPort.Width / 2, inputPort.Height / 2),
                    _board.ForegroundCanvas);



                var sHit = _board.ForegroundCanvas.InputHitTest(_startPort) as DependencyObject;
                var startPort = VisualTreeHelpers.FindParentOfType<OutputPortView>(sHit);

                var eHit = _board.ForegroundCanvas.InputHitTest(center) as DependencyObject;
                var endPort = VisualTreeHelpers.FindParentOfType<InputPortView>(eHit);


                var startHit = _board.ForegroundCanvas.InputHitTest(_startPort) as DependencyObject;
                var startNode = VisualTreeHelpers.FindParentOfType<INodeView>(startHit);

                var endHit = _board.ForegroundCanvas.InputHitTest(center) as DependencyObject;
                var endNode = VisualTreeHelpers.FindParentOfType<INodeView>(endHit);

                var exists = SuccessedConnections.Any(conn =>
                        conn.Start == startPort &&
                        conn.End == endPort
                    );

                portModel.Owner = startNode.nodeModel;
                endPort.portModel.Owner = endNode.nodeModel;

                if (startNode != endNode && !exists )
                {

                    portModel.ConnectedNodes.Add(endNode.nodeModel);
                    endPort.portModel.ConnectedNodes.Add(startNode.nodeModel);

                    endPort.portModel.ConnectedOutputPorts.Add(startPort.portModel);


                    Line line = new Line
                    {
                        Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1ED760")),
                        StrokeThickness = 2,
                        IsHitTestVisible = false,
                        X1 = _startPort.X,
                        Y1 = _startPort.Y,
                        X2 = center.X,
                        Y2 = center.Y,
                        Effect = new DropShadowEffect
                        {
                            Color = Color.FromRgb(50, 255, 100),
                            BlurRadius = 20,             
                            ShadowDepth = 0,            
                            Opacity = 1.0
                        }
                    };

                    _board.ForegroundCanvas.Children.Add(line);
                    OutputPortView.SuccessedConnections.Add((startPort,endPort,line));
                    InputPortView.SuccessedConnections.Add((endPort, startPort, line));

                }

            }
            
            _board.ForegroundCanvas.Children.Remove(_line);

        }


    }
}

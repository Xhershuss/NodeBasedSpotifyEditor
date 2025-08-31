using SpotifyEditor;
using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SpotifyEditor.View.Nodes.Ports
{
    
    public partial class InputPortView : UserControl
    {

        private bool _isPressed;
        private Point _mousePos;
        private Point _startPort;
        private Line _line;

        public string Id => portModel.Id;
        public string OwnerId => portModel.Owner.Id;

        public static List<(InputPortView Start, OutputPortView End, Line Line)> SuccessedConnections { get; set; }
                            = new List<(InputPortView Start, OutputPortView End, Line Line)>();

        private Board _board;

        public InputPortModel portModel { get; set; }
        public InputPortView(InputPortModel model)
        {
            InitializeComponent();
            portModel = model;
            model.ViewReference = this;

            var mainWin = Application.Current.MainWindow as MainWindow;
            _board = mainWin.GetMainBoard();

            inputPortEllipse.Tag = this;

        }

        public InputPortView() {  }

        public Ellipse InputPortEllipse => inputPortEllipse;


        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;
            
            double centerX = inputPortEllipse.Width / 2;
            double centerY = inputPortEllipse.Height / 2;

             _startPort= inputPortEllipse.TranslatePoint(
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


            inputPortEllipse.CaptureMouse();


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
            inputPortEllipse.ReleaseMouseCapture();
            e.Handled = true;

          

   
            Point _mousePos = Mouse.GetPosition(_board.ForegroundCanvas);
            var hit = _board.ForegroundCanvas.InputHitTest(_mousePos) as DependencyObject;
         
            

            var outputPort = VisualTreeHelpers.FindParentOfType<OutputPortView>(hit);

            if(outputPort != null )
            {
                
                Point center = outputPort.TranslatePoint(
                    new Point(outputPort.Width / 2, outputPort.Height / 2),
                    _board.ForegroundCanvas);


                var sHit = _board.ForegroundCanvas.InputHitTest(_startPort) as DependencyObject;
                var startPort = VisualTreeHelpers.FindParentOfType<InputPortView>(sHit);

                var eHit = _board.ForegroundCanvas.InputHitTest(center) as DependencyObject;
                var endPort = VisualTreeHelpers.FindParentOfType<OutputPortView>(eHit);



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

                if (startNode != endNode && !exists)
                {

                    portModel.ConnectedNodes.Add(endNode.nodeModel);
                    endPort.portModel.ConnectedNodes.Add(startNode.nodeModel);

                    portModel.ConnectedOutputPorts.Add(endPort.portModel);

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
                    InputPortView.SuccessedConnections.Add((startPort, endPort, line));
                    OutputPortView.SuccessedConnections.Add((endPort, startPort, line));

                }

            }

            _board.ForegroundCanvas.Children.Remove(_line);

                
           
        }


    }
}

using SpotifyEditor.Helpers;
using SpotifyEditor.Helpers;
using SpotifyEditor.View.Nodes.Common;
using SpotifyEditor.ViewModel;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
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


namespace SpotifyEditor.View
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    /// 
    public class NodeCommandParameter
    {
        public string VisibleName { get; set; }
        public string? NodeId { get; set; }
        public string NodeType { get; set; }
        public System.Drawing.Point Position { get; set; }

        public List<string> InputPortIds { get; set; } 
        public List<string> OutputPortIds { get; set; }
        public bool HasNodeId => !string.IsNullOrEmpty(NodeId);

        public NodeCommandParameter(string visibleName,string nodeType, System.Drawing.Point position)
        {
            VisibleName = visibleName;
            NodeType = nodeType;
            Position = position;
        }

        //For Loading existing nodes from file
        public NodeCommandParameter(
            string visibleName, string nodeType, string nodeId,System.Drawing.Point position, List<string> inputPortIds, List<string> outputPortIds)
        {
            VisibleName = visibleName;
            NodeType = nodeType;
            Position = position;
            NodeId = nodeId;
            InputPortIds = inputPortIds;
            OutputPortIds = outputPortIds;
        }
        public NodeCommandParameter() { }
    }   

    public partial class Board : UserControl
    {

        
        public NodeService nodeService { get; set; } 
        public Board() {
            InitializeComponent();
            nodeService = NodeService.Instance;

            DataContext = new BaseNodeViewModel(BoardForeground);

       
        }


        public Canvas ForegroundCanvas => BoardForeground;

        private void PopulateContextMenu(object dict, Point mousePos)
        {
           
            if (dict is not Dictionary<string, object> nodeData)
            {

                return;
            }

            NodeContextMenu.Items.Clear();


            var vm = this.DataContext as BaseNodeViewModel;
            if (vm == null)
            {

                return;
            }

            System.Drawing.Point drawingPoint = new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y);

            AddMenuItems(nodeData, NodeContextMenu.Items);

            
            void AddMenuItems(IDictionary<string, object> currentDictionary, ItemCollection parentItems)
            {
                foreach (var kvp in currentDictionary)
                {
                    string key = kvp.Key;
                    object value = kvp.Value;

              
                    if (value is Dictionary<string, object> branchDict)
                    {
                        var menuItem = new MenuItem { Header = key };
                        parentItems.Add(menuItem);
                     
                        AddMenuItems(branchDict, menuItem.Items);
                    }
           
                    else if (value is Dictionary<string, string> leafDict)
                    {
   
                        var parentMenuItem = new MenuItem { Header = key };
                        parentItems.Add(parentMenuItem);

                        foreach (var leafKvp in leafDict)
                        {
                            string nodeType = leafKvp.Key;
                            string visibleName = leafKvp.Value;

                            var nodeItem = new MenuItem
                            {
                                Header = visibleName,
                                Command = vm.AddNode, 
                                CommandParameter = new NodeCommandParameter(visibleName, nodeType, drawingPoint)
                            };
                            parentMenuItem.Items.Add(nodeItem);
                        }
                    }
                 
                }
            }
        }

        private void NodeContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var dict = NodeFactory.ContextMenuContent;

            Point mousePos = Mouse.GetPosition(ForegroundCanvas);

            PopulateContextMenu(dict, mousePos);
        }
    }
}
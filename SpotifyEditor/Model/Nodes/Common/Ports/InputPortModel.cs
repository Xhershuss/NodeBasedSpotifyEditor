using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyEditor.Helpers;
using SpotifyEditor.View.Nodes.Ports;
using System.Drawing;
using SpotifyEditor.ViewModel.Nodes;
using System.Diagnostics;

namespace SpotifyEditor.Model.Nodes.Ports
{
    public class InputPortModel : BindableBase
    {

        private BaseNodeModel owner;
        public DataExecutionContext? InputPortData { get; set; }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public InputPortView ViewReference { get; set; }

        public System.Windows.Point Position { get; set; }
        public BaseNodeModel Owner
        {
            get { return owner; }
            set { owner = value; OnPropertyChange(); }
        }

        private ObservableCollection<BaseNodeModel> connectedNodes
                            = new ObservableCollection<BaseNodeModel>();

        public ObservableCollection<BaseNodeModel> ConnectedNodes
        {
            get { return connectedNodes; }
            set { connectedNodes = value; }
        }

        public List<OutputPortModel> ConnectedOutputPorts { get; set; }
                                            = new List<OutputPortModel>();

        public InputPortModel() { }

        public static InputPortModel? GetPortModelById(string id)
        {
            return NodeService.Instance.AllNodes
                .SelectMany(n => n.InputPorts)      
                .FirstOrDefault(p => p.Id == id);   
        }
    }
}

using SpotifyEditor.Helpers;
using SpotifyEditor.View.Nodes.Ports;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Model.Nodes.Ports
{
    public class OutputPortModel:BindableBase
    {
        private BaseNodeModel owner;

        public BaseNodeModel Owner
        {
            get { return owner; }
            set { owner = value; OnPropertyChange(); }
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public System.Windows.Point Position { get; set; } 
        public OutputPortView  ViewReference{ get; set; }
        public DataExecutionContext? OutputPortData { get; set; }

        private ObservableCollection<BaseNodeModel> connectedNodes
                            = new ObservableCollection<BaseNodeModel>();
        public ObservableCollection<BaseNodeModel> ConnectedNodes
        {
            get { return connectedNodes; }
            set { connectedNodes = value;  }
        }


        public List<InputPortModel> ConnectedInputPorts { get; set; }
                                = new List<InputPortModel>();

        public OutputPortModel() { }

        public static OutputPortModel? GetPortModelById(string id)
        {
            return NodeService.Instance.AllNodes
                .SelectMany(n => n.OutputPorts)     
                .FirstOrDefault(p => p.Id == id);   
        }

    }
}

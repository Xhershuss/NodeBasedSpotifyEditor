using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpotifyEditor.View.Nodes
{
    public interface INodeView
    {

        public BaseNodeModel nodeModel { get; set; }

        public Canvas _canvas { get; set; }

        public void AddPorts(ObservableCollection<InputPortModel> inputPorts,
                             ObservableCollection<OutputPortModel> outputPorts);
        public void LoadPortPositions(ObservableCollection<InputPortModel> inputPorts,
                             ObservableCollection<OutputPortModel> outputPorts);

    }
}

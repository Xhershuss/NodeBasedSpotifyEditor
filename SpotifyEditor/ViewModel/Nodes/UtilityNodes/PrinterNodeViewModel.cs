using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.GeneratorNodes;
using SpotifyEditor.Model.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SpotifyEditor.Model.Nodes.UtilityNodes;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpotifyEditor.ViewModel.Nodes.UtilityNodes
{
    public class PrinterNodeViewModel:BaseNodeViewModel
    {
        public PrinterNodeViewModel(Canvas canvas) : base(canvas)
        {

        }


        public override PrinterNodeModel CreateNode(System.Drawing.Point pos)
        {
            int index = 0;
            var node = new PrinterNodeModel();
            node.Position = pos;
    


            node.InputPorts = new ObservableCollection<InputPortModel>() { };
            node.OutputPorts = new ObservableCollection<OutputPortModel>() { };


            for (int i = 0; i < node.InputCount; i++)
            {
                node.InputPorts.Add(new InputPortModel());
            }
            for (int i = 0; i < node.OutputCount; i++)
            {
                node.OutputPorts.Add(new OutputPortModel());
            }


            return node;

        }

    }
}

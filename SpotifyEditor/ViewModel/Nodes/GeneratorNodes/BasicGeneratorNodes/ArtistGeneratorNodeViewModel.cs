using SpotifyEditor.Model;
using SpotifyEditor.Model.Nodes.GeneratorNodes;
using SpotifyEditor.Model.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpotifyEditor.ViewModel.Nodes.GeneratorNodes
{
    public class ArtistGeneratorNodeViewModel : BaseNodeViewModel
    {

        public ArtistGeneratorNodeViewModel(Canvas canvas) : base(canvas)
        {
         
        }

      
        public override ArtistGeneratorNodeModel CreateNode(System.Drawing.Point pos)
        {
            var node = new ArtistGeneratorNodeModel();
            node.Position = pos;

 

            node.InputPorts = new ObservableCollection<InputPortModel>() { };
            node.OutputPorts = new ObservableCollection<OutputPortModel>() { };


            for (int i = 0; i <node.InputCount; i++)
            {
                node.InputPorts.Add(new InputPortModel());
            }
            for (int i = 0; i <  node.OutputCount; i++)
            {
                node.OutputPorts.Add(new OutputPortModel());
            }

            
            return node;

        }

    }
}

using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View.Nodes.Common;
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

namespace SpotifyEditor.View.Nodes.GeneratorNodes
{

    public partial class ArtistsAlbumsGeneratorNodeView : BaseNodeView
    {
            

        public ArtistsAlbumsGeneratorNodeView()
        {
            InitializeComponent();

        }

        public override StackPanel OutputPortContainer
        {
            get { return outputPortContainer; }
            set { outputPortContainer = value; }
        }

        public override StackPanel InputPortContainer
        {
            get { return inputPortContainer; }
            set { inputPortContainer = value; }
        }

    }
}

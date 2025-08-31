using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Ports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpotifyEditor.View.Nodes.Common
{
    public class NodeSelection :BindableBase
    {
        private static NodeSelection? _instance;
        public static NodeSelection Instance => _instance ??= new NodeSelection();

        public INodeView? selectedNode;

		public INodeView? SelectedNode
		{
			get { return selectedNode; }
			set { selectedNode = value;  }
		}

		private bool isVisible;

		public bool IsVisible
		{
			get { return isVisible; }
			set { isVisible = value; OnPropertyChange(); }
		}


	}
}

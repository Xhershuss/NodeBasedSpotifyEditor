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
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.ViewModel.Nodes.Common;

namespace SpotifyEditor.View.Nodes.Common
{
  
    public partial class GenericListView : Window
    {
        public GenericListView(BaseNodeModel nodeModel)
        {
            InitializeComponent();
            this.DataContext = new GenericListViewModel(nodeModel);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }   
    }
}

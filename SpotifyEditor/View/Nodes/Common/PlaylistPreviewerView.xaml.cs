using Microsoft.Win32;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.CreatorNodes;
using SpotifyEditor.ViewModel.Nodes.Common;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SpotifyEditor.View.Nodes.Common
{
    /// <summary>
    /// Interaction logic for PlaylistPreviewer.xaml
    /// </summary>
    public partial class PlaylistPreviewerView : Window
    {
        public PlaylistPreviewerView(BaseNodeModel node)
        {
            InitializeComponent();


            if (node is PlaylistCreatorNodeModel plCreatorNode)
            {
                this.DataContext = new PlaylistPreviewerViewModel(plCreatorNode);
            }
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (this.DataContext is PlaylistPreviewerViewModel vm)
                {
                    vm.ImagePath = openFileDialog.FileName;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.DataContext is PlaylistPreviewerViewModel vm)
            {
                vm.SaveChanges();
            }

            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}

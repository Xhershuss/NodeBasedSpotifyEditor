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
using System.Text.RegularExpressions;
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
    public partial class UserTopArtistsGeneratorNodeView : BaseNodeView
    {
        public UserTopArtistsGeneratorNodeView()
        {
            InitializeComponent();
 

      
            this.Focusable = true;

            
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


        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!(e.OriginalSource is TextBox))
            {

                this.Focus();
            }
        }


        private void NumberTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }


        private void NumberTb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V))
            {
                e.Handled = true;
            }
        }
    }
}

using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes.Ports;
using SpotifyEditor.View.Nodes;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SpotifyEditor.Model.Nodes
{
    public class BaseNodeModel : BindableBase
    {
  

        private static SpotifyAuthApi? spotifyApiClient;

        public static SpotifyAuthApi? SpotifyApiClient
        {
            get { return spotifyApiClient; }
            set { spotifyApiClient = value; }
        }

        public BaseNodeViewModel ViewModelReference { get; set; }
        public INodeView ViewReference { get; set; }

        private Visibility selectedNodeBorder = Visibility.Hidden;
        public Visibility SelectedNodeBorder
        {
            get => selectedNodeBorder;
            set { selectedNodeBorder = value; OnPropertyChange(); }
        }


        private string userInputText;

        public string UserInputText
        {
            get { return userInputText; }
            set { userInputText = value; OnPropertyChange(); }
        }

        public string Id { get; set; }= Guid.NewGuid().ToString();

        private string name;
        public string Name
        {
            get { return name; } 
            set { name = value; OnPropertyChange(); } 
        }

        public int InputCount { get; set; }
        public int OutputCount { get; set; }

        public string NodeType { get; set; }


        public bool IsExecuted { get; set; } = false;

        private System.Drawing.Point position;
        public System.Drawing.Point Position
        {
            get { return position; }
            set { position = value; OnPropertyChange(); }
        }

        private ObservableCollection<InputPortModel> inputPorts;

        public ObservableCollection<InputPortModel>? InputPorts
        {
            get { return inputPorts; }
            set { inputPorts = value;  }
        }

        private ObservableCollection<OutputPortModel>? outputPorts;

        public ObservableCollection<OutputPortModel> OutputPorts
        {
            get { return outputPorts; }
            set { outputPorts = value; }
        }


        private Brush executeIndicatorColor = Brushes.Gold;

        public Brush ExecuteIndicatorColor
        {
            get { return executeIndicatorColor; }
            set { executeIndicatorColor = value;  OnPropertyChange(); }
        }

        public void ExecuteIndicator( )
        {
            if (this.IsExecuted)
            {
                this.ExecuteIndicatorColor = Brushes.LightGreen;
            }

        }
        public void ReverseIndicator()
        {
            if (!this.IsExecuted)
            {
                this.ExecuteIndicatorColor = Brushes.Gold;
            }
        }

        public virtual async Task Process(DataExecutionContext data)
        {

     
            await Task.Delay(200);
            IsExecuted = true;
            this.ExecuteIndicator();

        }

        public void WriteToOutputPorts(DataExecutionContext data)
        {

            foreach (var outputPort in this.OutputPorts)
            {
                outputPort.OutputPortData = data;
              
            }
        }


    }
}

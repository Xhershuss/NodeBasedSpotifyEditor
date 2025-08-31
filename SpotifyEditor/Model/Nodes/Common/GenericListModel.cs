using SpotifyEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Model.Nodes.Common
{
    public class GenericListModel:BindableBase
    {

		
        public string ItemType { get; set; }

        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();


        public string UserInputText { get; set; }
      

	}
}

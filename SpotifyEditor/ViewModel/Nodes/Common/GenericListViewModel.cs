using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.Common;
using SpotifyEditor.Model.Nodes.GeneratorNodes.BatchGeneratorNodes;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Windows;   

namespace SpotifyEditor.ViewModel.Nodes.Common
{
    public class GenericListViewModel : BindableBase
    {
        public GenericListModel ListModel { get; }

        public RelayCommand<string> AddItemCommand { get; }
        public RelayCommand<object> RemoveItemCommand { get; }
        public RelayCommand<object> ClearItemsCommand { get; }

        public RelayCommand<Window> ConfirmCommand { get; }

        public ObservableCollection<object> Items => ListModel.Items;

        public string ItemType
        {
            get => ListModel.ItemType;
            set { ListModel.ItemType = value; OnPropertyChange(); }
        }

        public string UserInputText
        {
            get => ListModel.UserInputText;
            set { ListModel.UserInputText = value; OnPropertyChange(); }
        }

        private string selectedItem;
        public string SelectedItem
        {
            get => selectedItem;
            set { selectedItem = value; OnPropertyChange(); }
        }

        public GenericListViewModel(BaseNodeModel nodeModel)
        {
            if(nodeModel is IBatchNodes node)
            {
                ListModel = node.ItemList;
            }

            AddItemCommand = new RelayCommand<string>(text =>
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    ListModel.Items.Add(text);
                    UserInputText = string.Empty;
                }
            });

            RemoveItemCommand = new RelayCommand<object>(param =>
            {
                
                if (param is IList selectedItems && selectedItems.Count > 0)
                {

                    var toRemove = selectedItems.Cast<object>().ToList();
                    foreach (var item in toRemove)
                    {
                        if (ListModel.Items.Contains(item))
                            ListModel.Items.Remove(item);
                    }

                    SelectedItem = null;
                    return;
                }

                
            });

            ClearItemsCommand = new RelayCommand<object>(_ => ListModel.Items.Clear());

            ConfirmCommand = new RelayCommand<Window>(window =>
            {
                if (window != null) window.Close();

            });
        }
    }
}

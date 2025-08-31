using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.Model.Nodes.CreatorNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.ViewModel.Nodes.Common
{
    public class PlaylistPreviewerViewModel : BindableBase
    {
        // Ana node modelini saklamak için bir referans tutalım
        private readonly PlaylistCreatorNodeModel _nodeModel;

        private string _userUrl;
        public string UserUrl
        {
            get => _userUrl;
            set { _userUrl = value; OnPropertyChange(); }
        }

        private string _playlistTitle;
        public string PlaylistTitle
        {
            get => _playlistTitle;
            set { _playlistTitle = value; OnPropertyChange(); }
        }

        private string _playlistDescription;
        public string PlaylistDescription
        {
            get => _playlistDescription;
            set { _playlistDescription = value; OnPropertyChange(); }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChange(); }
        }

        // Constructor: Model'den ViewModel'e veri yükler
        public PlaylistPreviewerViewModel(PlaylistCreatorNodeModel node)
        {
            _nodeModel = node;

            // 1. Model'deki mevcut verileri ViewModel'e yükle
            UserUrl = _nodeModel.UserUrl;
            PlaylistTitle = _nodeModel.PlaylistTitle;
            PlaylistDescription = _nodeModel.PlaylistDescription;
            ImagePath = _nodeModel.ImagePath;
        }

        // SaveChanges Metodu: ViewModel'den Model'e veriyi kaydeder
        public void SaveChanges()
        {
            // 2. ViewModel'deki güncel verileri Model'e geri kaydet
            _nodeModel.UserUrl = this.UserUrl;
            _nodeModel.PlaylistTitle = this.PlaylistTitle;
            _nodeModel.PlaylistDescription = this.PlaylistDescription;
            _nodeModel.ImagePath = this.ImagePath;
        }
    }
}

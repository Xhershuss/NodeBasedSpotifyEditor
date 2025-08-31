using SpotifyEditor.Model.Nodes;
using SpotifyEditor.View;
using SpotifyEditor.View.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using SpotifyEditor.Model.Nodes.GeneratorNodes;
using SpotifyEditor.ViewModel.Nodes.GeneratorNodes;
using SpotifyEditor.View.Nodes.GeneratorNodes;
using SpotifyEditor.ViewModel.Nodes.CreatorNodes;
using SpotifyEditor.View.Nodes.CreatorNodes;
using SpotifyEditor.View.Nodes.FilterNodes;
using SpotifyEditor.ViewModel.Nodes.FilterNodes;
using SpotifyEditor.ViewModel.Nodes.UtilityNodes;
using SpotifyEditor.View.Nodes.UtilityNodes;
using SpotifyEditor.Api.Models.Responses;



namespace SpotifyEditor.ViewModel.Nodes
{
    public static class NodeFactory 
    {

        public static Dictionary<string, object> ContextMenuContent { get; set; }
    = new Dictionary<string, object>()
    {
        { "Generators", new Dictionary<string, object>() 
            {
                { "BasicGenerator", new Dictionary<string, string>()
                    {
                        { "ArtistGenerator" ,"Artist"},
                        { "AlbumGenerator", "Album" },
                        { "TrackGenerator", "Track" },
                        { "PlaylistGenerator", "Playlist" }
                    }
                },
                { "BatchGenerator", new Dictionary<string, string>()
                    {
                        { "AlbumBatchGenerator", "Album Batch" },
                        { "ArtistBatchGenerator", "Artist Batch" },
                        { "TrackBatchGenerator", "Track Batch" },
                        { "NewAlbumReleasesGenerator" , "New Albums"},
                        { "ArtistsAlbumsGenerator", "Artist's Albums" },
                        { "ArtistsTopTracksGenerator", "Artist's Top Tracks" },
                        { "UsersPlaylistsGenerator", "User's Playlists" }
                    }
                },
                { "UserSpecificGenerator", new Dictionary<string, string>()
                    {
                        { "UserSavedAlbumsGenerator", "User Saved Albums" },
                        { "UserTopArtistsGenerator", "User Top Artists"},
                        { "UserTopTracksGenerator", "User Top Tracks" },
                        { "RecentlyPlayedTracksGenerator", "Recently Played Tracks" },
                        { "UsersQueueGenerator", "User's Queue" },
                        { "CurrentUsersPlaylistsGenerator", "Current Users Playlists" },
                        { "UsersSavedTracksGenerator" , "Saved Tracks (Your Music)"},
                        { "UsersFollowedArtistsGenerator", "Followed Artists" }
                    }
                }
            }
        },
        { "Utility", new Dictionary<string, string>() 
            {
                { "Printer", "Printer (For Debug)" },
                {"RemoveDuplicates", "Remove Duplicates" }
            }
        },
        { "Creators", new Dictionary<string, string>()
            {
                { "PlaylistCreator", "Playlist Creator" },
                { "AddToPlaylist" , "Add To Playlist" },
                {"RemoveFromPlaylist", "Remove From Playlist" },
                {"AddToSavedTracks",  "Add To Saved Tracks" },
                {"RemoveFromSavedTracks", "Remove From Saved Tracks" },
                {"AddToQueue", "Add To Queue" }
            }
        },
        {"Filters" , new Dictionary<string, string>()
            {
                {"PopularityFilter", "Popularity Filter" },
                {"DurationFilter", "Duration Filter" },
                {"AlbumReleaseDateFilter", "Album Release Date Filter" },
                {"ArtistFilter", "Artist Filter" },
                {"AlbumFilter", "Album Filter" }
            }
        }
    };
        public static BaseNodeViewModel CreateNodeViewModel(string nodeType, Canvas canvas)
        {
            return nodeType switch
            {
                "ArtistGenerator" => new ArtistGeneratorNodeViewModel(canvas),
                "AlbumGenerator" => new AlbumGeneratorNodeViewModel(canvas),
                "TrackGenerator" => new TrackGeneratorNodeViewModel(canvas),
                "PlaylistGenerator" => new PlaylistGeneratorNodeViewModel(canvas),


                "AlbumBatchGenerator" => new AlbumBatchGeneratorNodeViewModel(canvas),
                "ArtistBatchGenerator" => new ArtistBatchGeneratorNodeViewModel(canvas),
                "TrackBatchGenerator" => new TrackBatchGeneratorNodeViewModel(canvas),
                "NewAlbumReleasesGenerator" => new NewAlbumReleasesGeneratorNodeViewModel(canvas),
                "ArtistsAlbumsGenerator" => new ArtistsAlbumsGeneratorNodeViewModel(canvas),
                "ArtistsTopTracksGenerator" => new ArtistsTopTracksGeneratorNodeViewModel(canvas),
                "UsersPlaylistsGenerator" => new UsersPlaylistsGeneratorNodeViewModel(canvas),

                "UserSavedAlbumsGenerator" => new UserSavedAlbumsGeneratorNodeViewModel(canvas),
                "UserTopArtistsGenerator" => new UserTopArtistsGeneratorNodeViewModel(canvas),
                "UserTopTracksGenerator" => new UserTopTracksGeneratorNodeViewModel(canvas),
                "RecentlyPlayedTracksGenerator" => new RecentlyPlayedTracksGeneratorNodeViewModel(canvas),
                "UsersQueueGenerator" => new UsersQueueGeneratorNodeViewModel(canvas),
                "CurrentUsersPlaylistsGenerator" => new CurrentUsersPlaylistsGeneratorNodeViewModel(canvas),
                "UsersSavedTracksGenerator" => new UserSavedTracksGeneratorNodeViewModel(canvas),
                "UsersFollowedArtistsGenerator" => new UsersFollowedArtistsGeneratorNodeViewModel(canvas),

                "PlaylistCreator" => new PlaylistCreatorNodeViewModel(canvas),
                "AddToPlaylist" => new AddToPlaylistNodeViewModel(canvas),
                "RemoveFromPlaylist" => new DeleteFromPlaylistNodeViewModel(canvas),
                "AddToSavedTracks" => new AddToSavedTracksNodeViewModel(canvas),
                "RemoveFromSavedTracks" => new DeleteFromSavedTracksNodeViewModel(canvas),
                "AddToQueue" => new AddToQueueNodeViewModel(canvas),

                "Printer" => new PrinterNodeViewModel(canvas),
                "RemoveDuplicates" => new RemoveDuplicatesNodeViewModel(canvas),

                "PopularityFilter" => new PopularityFilterNodeViewModel(canvas),
                "DurationFilter" => new DurationFilterNodeViewModel(canvas),
                "AlbumReleaseDateFilter" => new AlbumReleaseDateFilterNodeViewModel(canvas),
                "ArtistFilter" => new ArtistFilterNodeViewModel(canvas),
                "AlbumFilter" => new AlbumFilterNodeViewModel(canvas),

                _ => throw new ArgumentException($"Unknown node type: {nodeType}")
            };
        }


        public static INodeView CreateNodeView(string nodeType, BaseNodeModel node,Canvas canvas)
        {
            {
                INodeView view = nodeType switch
                {
                    "ArtistGenerator" => new ArtistGeneratorNodeView() { DataContext= node},
                    "AlbumGenerator" => new AlbumGeneratorNodeView() { DataContext = node },
                    "TrackGenerator" => new TrackGeneratorNodeView() { DataContext = node },
                    "PlaylistGenerator" => new PlaylistGeneratorNodeView() { DataContext = node },

                    "AlbumBatchGenerator" => new AlbumBatchGeneratorNodeView() { DataContext = node },
                    "ArtistBatchGenerator" => new ArtistBatchGeneratorNodeView() { DataContext = node },
                    "TrackBatchGenerator" => new TrackBatchGeneratorNodeView() { DataContext = node },
                    "NewAlbumReleasesGenerator" => new NewAlbumReleasesGeneratorNodeView() { DataContext = node },
                    "ArtistsAlbumsGenerator" => new ArtistsAlbumsGeneratorNodeView() { DataContext = node },
                    "ArtistsTopTracksGenerator" => new ArtistsTopTracksGeneratorNodeView() { DataContext = node },
                    "UsersPlaylistsGenerator" => new UsersPlaylistsGeneratorNodeView() { DataContext = node },

                    "UserSavedAlbumsGenerator" => new UserSavedAlbumsGeneratorNodeView() { DataContext = node },
                    "UserTopArtistsGenerator" => new UserTopArtistsGeneratorNodeView() { DataContext = node },
                    "UserTopTracksGenerator" => new UserTopTracksGeneratorNodeView() { DataContext = node },
                    "RecentlyPlayedTracksGenerator" => new RecentlyPlayedTracksGeneratorNodeView() { DataContext = node },
                    "UsersQueueGenerator" => new UsersQueueGeneratorNodeView() { DataContext = node },
                    "CurrentUsersPlaylistsGenerator" => new CurrentUsersPlaylistsGeneratorNodeView() { DataContext = node },
                    "UsersSavedTracksGenerator" => new UserSavedTracksGeneratorNodeView() { DataContext = node },
                    "UsersFollowedArtistsGenerator" => new UsersFollowedArtistsGeneratorNodeView() { DataContext = node },

                    "PlaylistCreator" => new PlaylistCreatorNodeView() { DataContext = node },
                    "AddToPlaylist" => new AddToPlaylistNodeView() { DataContext = node },
                    "RemoveFromPlaylist" => new DeleteFromPlaylistNodeView() { DataContext = node },
                    "AddToSavedTracks" => new AddToSavedTracksNodeView() { DataContext = node },
                    "RemoveFromSavedTracks" => new DeleteFromSavedTracksNodeView() { DataContext = node },
                    "AddToQueue" => new AddToQueueNodeView() { DataContext = node },

                    "Printer" => new PrinterNodeView() { DataContext = node },
                    "RemoveDuplicates" => new RemoveDuplicatesNodeView() { DataContext = node },

                    "PopularityFilter" => new PopularityFilterNodeView() { DataContext = node },
                    "DurationFilter" => new DurationFilterNodeView() { DataContext = node },
                    "AlbumReleaseDateFilter" => new AlbumReleaseDateFilterNodeView() { DataContext = node },
                    "ArtistFilter" => new ArtistFilterNodeView() { DataContext = node },
                    "AlbumFilter" => new AlbumFilterNodeView() { DataContext = node },

                    _ => throw new ArgumentException($"Unknown type {nodeType}")
                };

                node.ViewReference = view ;
                view.nodeModel = node;
                view._canvas = canvas;
                return view;
            }
        }
    }
}

using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using SpotifyEditor.Api.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyEditor.Model.Nodes.UtilityNodes
{
    public class PrinterNodeModel:BaseNodeModel
    {
        public PrinterNodeModel()
        {
            Name = "Printer Node";
            InputCount = 1;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

           

            if (dataContext == null )
            {
                Debug.WriteLine("No data to print.");
                return;
            }


            IEnumerable<object> items;
            try
            {
                items = DataExecutionContext.TransformData<object>(dataContext);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TransformData Error] {ex.GetType().Name}: {ex.Message}");
                return;
            }

            foreach (var item in items)
            {

                switch (item)
                {
                    case Artist artist:
                        try
                        {
                            Debug.WriteLine($"[Artist] Name: {artist.Name}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Artist Print Error] {ex.Message}");
                        }
                        break;
                    
                    case List<Artist> artistList:
                        try
                        {
                        Debug.WriteLine($"[Artist List] Count: {artistList.Count}");
                        foreach (var art in artistList)
                        {
                            Debug.WriteLine($"\t[Artist] Name: {art.Name}");
                        }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Artist List Print Error] {ex.Message}");
                        }
                        break;

                    case Album album:
                        try
                        {
                            Debug.WriteLine($"[Album] Title: {album.Name} — Tracks: {album.Tracks?.Items.Count}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Album Print Error] {ex.Message}");
                        }
                        break;
                    case List<Album> albumList:
                        try
                        {
                            Debug.WriteLine($"[Album List] Count: {albumList.Count}");
                            foreach (var alb in albumList)
                            {
                                Debug.WriteLine($"\t[Album] Title: {alb.Name} — Tracks: {alb.Tracks?.Items.Count}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Album List Print Error] {ex.Message}");
                        }
                        break;

                    // For User Specific Albums
                    case List<DetailedAlbum> detailedAlbumList:
                        try
                        {
                            Debug.WriteLine($"[Detailed Album List] Count: {detailedAlbumList.Count}");
                            foreach (var dalb in detailedAlbumList)
                            {
                                Debug.WriteLine($"\t[Detailed Album] Title: {dalb.Album.Name} — Tracks: {dalb.Album.Tracks?.Items.Count}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Detailed Album List Print Error] {ex.Message}");
                        }
                        break;

                    case Track track:
                        try
                        {
                            Debug.WriteLine($"[Track] Title: {track.Name} — Artist: {track.Artists?.FirstOrDefault()?.Name}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Track Print Error] {ex.Message}");
                        }
                        break;

                    // For User Saved Tracks 
                    case List<TracksWithDate> tracksWithDateList:
                        try
                        {
                            Debug.WriteLine($"[Tracks With Date List] Count: {tracksWithDateList.Count}");
                            foreach (var twd in tracksWithDateList)
                            {
                                Debug.WriteLine($"\t[Track With Date] Added At: {twd.AddedAt} — Title: {twd.Track.Name} — Artist: {twd.Track.Artists?.FirstOrDefault()?.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Tracks With Date List Print Error] {ex.Message}");
                        }
                        break;
                    case List<Track> trackList:
                        try
                        {
                            Debug.WriteLine($"[Track List] Count: {trackList.Count}");
                            foreach (var trk in trackList)
                            {
                                Debug.WriteLine($"\t[Track] Title: {trk.Name} — Artist: {trk.Artists?.FirstOrDefault()?.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Track List Print Error] {ex.Message}");
                        }
                        break;

                    case Playlist playlist:
                        try
                        {
                            Debug.WriteLine($"[Playlist] Name: {playlist.Name} — Tracks: {playlist.tracks.Items.FirstOrDefault().Track.Name}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Playlist Print Error] {ex.Message}");
                        }
                        break;

                    case List<Playlist> playlistList:
                        try
                        {
                            Debug.WriteLine($"[Playlist List] Count: {playlistList.Count}");
                            foreach (var pl in playlistList)
                            {
                          
                                Debug.WriteLine($"\t[Playlist] Name: {pl.Name} — Tracks: {pl.tracks.Items.FirstOrDefault().Track.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Playlist List Print Error] {ex.Message}");
                        }
                        break;

                    default:
                        try
                        {
                            Debug.WriteLine($"[Unknown Type {item?.GetType().Name}] {item}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Unknown Print Error] {ex.Message}");
                        }
                        break;
                }
            }

   
            IsExecuted = true;
            ExecuteIndicator();
            WriteToOutputPorts(dataContext);
        }
    }

    
}

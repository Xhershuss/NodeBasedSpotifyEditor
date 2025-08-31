using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;





namespace SpotifyEditor.Model
{
    public class DataExecutionContext
    {

        public List<object>? Data { get; set; }
        public Dictionary<string, object>? Metadata { get; set; } = new();

        public DataMultiplicity? MultiplicityState { get; set; }

        public static IEnumerable<T> TransformData<T>(DataExecutionContext ctx)
        {
            if (ctx.Data == null)
                throw new InvalidOperationException("Data cannot be null.");

            return ctx.Data.SelectMany(d =>
            {
                if (d is DataExecutionContext inner)
                    return inner.Data.Cast<T>();
                else
                    return new[] { (T)d };
            });
        }
        public enum DataMultiplicity
        {
            Multiple,
            Single
        }

        public static Dictionary<string, object> DataContextToFilterData(DataExecutionContext dataContext)
        {
            var dict = new Dictionary<string, object>();
            var items = TransformData<object>(dataContext);

            var artistList = new List<Artist>();
            var trackList = new List<Track>();

            foreach (var item in items)
            {
                switch (item)
                {
                    case Artist artist:
                        artistList.Add(artist);
                        break;
                    case List<Artist> artists:
                        artistList.AddRange(artists);
                        break;
                    default:
                        trackList.AddRange(DataContextToTrackList(dataContext,true));
                        break;
                }
            }

            if (artistList.Any()) dict["artist"] = artistList;
            if (trackList.Any()) dict["track"] = trackList;

            return dict;
        }
        public static List<Track> DataContextToTrackList(DataExecutionContext dataContext,bool withArtist = false)
        {
            List<Track> tracks = new List<Track>();

            IEnumerable<object> items;
            try
            {
                items = DataExecutionContext.TransformData<object>(dataContext);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TransformData Error] {ex.GetType().Name}: {ex.Message}");
                return tracks;
            }

            foreach (var item in items)
            {

                switch (item)
                {
                    case Artist artist:
                        if(!withArtist)
                            throw new InvalidOperationException("Expected Track or Track List but found Artist.");
                        break;

                    case List<Artist> artistList:
                        if(!withArtist)
                            throw new InvalidOperationException("Expected Track or Track List but found Artist List.");
                        break;
                    case Album album:
                        try
                        {
                            tracks.AddRange(album.Tracks?.Items ?? new List<Track>());
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Failed to Fetch Album {album.Name}]  {ex.Message}");
                        }
                        break;
                    case List<Album> albumList:
                            foreach (var alb in albumList)
                            {
                                try
                                {
                                    tracks.AddRange(alb.Tracks?.Items ?? new List<Track>());
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"[Failed to Fetch Album {alb.Name}]  {ex.Message}");
                                }
                            }
                        break;

                    // For User Specific Albums
                    case List<DetailedAlbum> detailedAlbumList:
                            foreach (var dalb in detailedAlbumList)
                            {
                                try
                                {
                                    tracks.AddRange(dalb.Album?.Tracks?.Items ?? new List<Track>());
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"[Failed to Fetch Detailed Album {dalb.Album.Name}]  {ex.Message}");
                                }
                        }

                        break;

                    case Track track:
                        try
                        {
                            tracks.Add(track);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Failed to Add Track {track.Name}] {ex.Message}");
                        }
                        break;

                    case List<Track> trackList:
                        try
                        {
                            tracks.AddRange(trackList);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Failed to Add Track List] {ex.Message}");
                        }
                        break;

                    // For User Saved Tracks 
                    case List<TracksWithDate> tracksWithDateList:
                        foreach (var twd in tracksWithDateList)
                        {
                            try
                            {
                                tracks.Add(twd.Track);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"[TracksWithDate Add Track Error {twd.Track.Name}] {ex.Message}");
                            }
                        }
                        break;

                   

                    case Playlist playlist:
                        try
                        {
                            tracks.AddRange(playlist.tracks?.Items.Select(dt => dt.Track) ?? new List<Track>());
                        }
                        catch (Exception ex)
                        {
                           Debug.WriteLine($"[Failed to Fetch Playlist {playlist.Name}]  {ex.Message}");
                        }
                        break;

                    case List<Playlist> playlistList:
                        foreach (var pl in playlistList)
                        {
                            try
                            {
                                tracks.AddRange(pl.tracks?.Items.Select(dt => dt.Track) ?? new List<Track>());
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"[Failed to Fetch Playlist {pl.Name}]  {ex.Message}");
                            }
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

            return tracks;
        }
    }
}


using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;    
using SpotifyEditor.Api.Services;
using System.Configuration;
using System;

public class Program
{
    public static async Task Main(string[] args)
    {
        var clientId = ConfigurationManager.AppSettings["SpotifyClientId"];
        var clientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];

        var spotifyClient = SpotifyAuthApi.GetInstance;
        await spotifyClient.GetSpotifyClient(clientId, clientSecret);
        Console.WriteLine("Client Successfuly Summoned");

        //await AlbumServiceTests(spotifyClient);
        //await ArtistServiceTests(spotifyClient);
        //await PlayerServiceTests(spotifyClient);
        //await PlaylistServiceTests(spotifyClient);
        //await SearchServiceTests(spotifyClient);
        //await TrackServiceTests(spotifyClient);
        //await UserServiceTests(spotifyClient);
        //await CategoryServiceTests(spotifyClient);

        


        Console.ReadKey();

        
    }


    static async Task CategoryServiceTests(SpotifyAuthApi spotifyClient)
    {
        CategoryService categoryService = new CategoryService(spotifyClient._apiClient);
        try
        {
            //GET CATEGORIES
            Console.WriteLine(1);
            var categories = await categoryService.GetCategories();
            for(int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i} - Category Name: {categories[i].Name}, ID: {categories[i].Id}");
            }

            //GET CATEGORY
            Console.WriteLine(2);
            string categoryId = "rock"; // OR "0JQ5DAqbMKFDXXwE9BDJAr"
            var category = await categoryService.GetCategory(categoryId);
            Console.WriteLine($"Category Name: {category.Name}, ID: {category.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }


    static async Task UserServiceTests(SpotifyAuthApi spotifyClient)
    {
        UserService userService = new UserService(spotifyClient._apiClient);
        try
        {
            //GET CURRENT USERS PROFILE
            Console.WriteLine(1);
            var user = await userService.GetCurrentUser();
            Console.WriteLine($"User Name: {user.DisplayName}");

            //GET CURRENT USERS TOP TRACKS
            Console.WriteLine(2);
            int numberOfTracks = 5; // If not specified, you can get all tracks that spotify has for you.
                                      // But its taking a long time to get all of them.
                                      // Spotify API will return up to the total available items — if you request 100 but only have 38, you’ll get 38.

            var topTracks = await userService.GetUsersTopTracks(TimeRange.ShortTerm,numberOfTracks);
            for (int i = 0;i<topTracks.Count;i++)
            {
                Console.WriteLine($"{i}- Track Name: {topTracks[i].Name}, Popularity: {topTracks[i].Popularity}");
            }

            //GET CURRENT USERS TOP ARTISTS
            Console.WriteLine(3);
            int numberOfArtists = 5; // If not specified, you can get all tracks that spotify has for you.
                                       // But its taking a long time to get all of them.
                                       // Spotify API will return up to the total available items — if you request 100 but only have 38, you’ll get 38.

            var topArtists = await userService.GetUsersTopArtists(TimeRange.ShortTerm, numberOfArtists);
            for (int i = 0; i < topArtists.Count; i++)
            {
                Console.WriteLine($"{i} -Artist Name: {topArtists[i].Name}, Popularity: {topArtists[i].Popularity}");
            }



            //GET USER PROFILE BY USERNAME
            Console.WriteLine(4);
            string username = "eren_ozunlu";
            var profile = await userService.GetUserProfile(username);
            Console.WriteLine(profile.DisplayName);


            //FOLLOW PLAYLIST FOR CURRENT USER
            Console.WriteLine(5);
            string playlistId = "37i9dQZF1DZ06evO0ENBD2"; // THIS IS QUEEN
            await userService.FollowPlaylist(playlistId);

            //UNFOLLOW PLAYLIST FOR CURRENT USER
            Console.WriteLine(6);
            await userService.UnfollowPlaylist(playlistId);

            //GET CURRENT USER'S FOLLOWED ARTISTS
            Console.WriteLine(7);
            var followedArtists = await userService.GetFollowedArtists();
            foreach (var artist in followedArtists)
            {
                Console.WriteLine($"Artist Name: {artist.Name}");
            }

            //FOLLOW ARTISTS FOR CURRENT USER
            Console.WriteLine(8);
            List<string> artistIds = new List<string> { "0LcJLqbBmaGUft1e9Mm8HV", "08GQAI4eElDnROBrJRGE0X" };
            await userService.FollowArtists(artistIds);

            //UNFOLLOW ARTISTS FOR CURRENT USER
            Console.WriteLine(9);
            await userService.UnfollowArtists(artistIds);

            //CHECK IF USER FOLLOWS ARTISTS
            Console.WriteLine(10);
            var artistFollowStatus = await userService.CheckIfUserFollowsArtists(artistIds);
            foreach (var status in artistFollowStatus)
            {
                Console.WriteLine($"Artist Follow Status: {status}");
            }

            //UNFOLLOW USERS FOR CURRENT USER
            List<string> userIds = new List<string> { "elifoznl" };
            Console.WriteLine(12);
            await userService.UnfollowUsers(userIds);

            //FOLLOW USERS FOR CURRENT USER
            Console.WriteLine(11);
            await userService.FollowUsers(userIds);


            //CHECK IF USER FOLLOWS USERS
            Console.WriteLine(13);
            var userFollowStatus = await userService.CheckIfUserFollowsUsers(userIds);
            foreach (var status in userFollowStatus)
            {
                Console.WriteLine($"User Follow Status: {status}");
            }

            //CHECK IF USER FOLLOWS PLAYLIST
            Console.WriteLine(14);
            var playlistFollowStatus = await userService.CheckIfUserFollowsPlaylist(playlistId);
            Console.WriteLine($"Playlist Follow Status: {playlistFollowStatus}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    static async Task TrackServiceTests(SpotifyAuthApi spotifyClient)
    {
        TrackService trackService = new TrackService(spotifyClient._apiClient);
        try
        {
            string trackId = "7h360CLGnW68EYvKPSoQOb";
            string trackId2 = "62KZRwymXq9Rgm8Cm5UCfx";
            var trackList = new List<string> { trackId, trackId2 };

            //GET TRACK
            Console.WriteLine(1);
            Track track = await trackService.GetTrack(trackId);
            Console.WriteLine($"Track Name: {track.Name}");

            //GET SEVERAL TRACKS
            Console.WriteLine(2);
            var tracks = await trackService.GetTracks(trackList);
            foreach (var item in tracks)
            {
                Console.WriteLine($"Track Name: {item.Name}");
            }

            //GET CURRENT USERS SAVED TRACKS
            Console.WriteLine(3);
            var savedTracks = await trackService.GetCurrentUsersSavedTracks();
            foreach (var item in savedTracks)
            {
                Console.WriteLine($"Track Name: {item.Track.Name}, Added At: {item.AddedAt}");
            }


            //SOMETIMES YOU HAVE TO RESTART THE SPOTIFY APP TO SEE THE CHANGES MADE TO SAVED TRACKS
            //SAVE TRACKS FOR CURRENT USER
            Console.WriteLine(4);
            await trackService.SaveTrackForCurrentUser(trackList);

            //SOMETIMES YOU HAVE TO RESTART THE SPOTIFY APP TO SEE THE CHANGES MADE TO SAVED TRACKS
            //DELETE TRACKS FOR CURRENT USER
            Console.WriteLine(5);
            await trackService.RemoveUsersSavedTracks(trackList);

            //CHECK IF TRACKS ARE SAVED FOR CURRENT USER
            Console.WriteLine(6);   
            var savedStatus = await trackService.CheckUsersSavedTracks(trackList);
            foreach (var status in savedStatus)
            {
                Console.WriteLine($"Track Saved Status: {status}");
            }



        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task SearchServiceTests(SpotifyAuthApi spotifyClient)
    {
        
        SearchService searchService = new SearchService(spotifyClient._apiClient);
        try
        {
            Console.WriteLine(1);
            var types = new List<string>() { "track", "album" };
            var result = await searchService.SearchForItem(
                q: "remaster track:Doxy artist:Miles Davis",
                types: types,
                limit: 10,
                offset: 0
            );
            Console.WriteLine("Tracks found: " + result.Tracks.Items.Count);
            Console.WriteLine("Albums found: " + result.Albums.Items.Count);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task PlaylistServiceTests(SpotifyAuthApi spotifyClient)
    {
        PlaylistService playlistService = new PlaylistService(spotifyClient._apiClient);
        try
        {
            string playlistId = "2ID29H3KFyrJh5cAGSPW3S";
            string testPlaylistId = "6UVL2gcLU5ZVLBedeE12yg";

            //GET PLAYLIST
            Console.WriteLine(1);   // UZUN SÜRÜYO DİYE KAPATTIM  
            var playlist = await playlistService.GetPlaylist(playlistId);
            Console.WriteLine($"Playlist Name: {playlist.Name}");
            foreach (var track in playlist.tracks.Items)
            {
                //Console.WriteLine($"Track Name: {track.Track.Name}");
            }
                                

            //CHANGE PLAYLIST DETAILS 
            Console.WriteLine(2);
            // CHANGING PUBLIC STATE AND COLLABORATIVE STATE IS NOT WORKING FOR SOME REASON 
            // ITS SPOIFY BUG I GUESS
            await playlistService.ChangePlaylistDetails(testPlaylistId,"Test Playlist","Despspspps",false,false);


            //ADD TRACK TO PLAYLIST
            Console.WriteLine(3);
            List<string> tracks = new List<string>() { "46OKHucGhjhskazqD8tKnH" , "1brpdmqkx3kSxyqzqXfW7J" , "29SyMC0plk6qw8NMF7lfRL" };
            await playlistService.AddTracksToPlaylist(testPlaylistId, tracks);

            //REORDER PLAYLIST ITEMS
            Console.WriteLine(5);
            await playlistService.ReorderPlaylistTracks(testPlaylistId, 0, 1, 0);

            //REMOVE TRACKS FROM PLAYLIST
            Console.WriteLine(4);
            await playlistService.RemovePlaylistTracks(testPlaylistId, tracks.GetRange(0, 2));


            //UPDATE PLAYLIST ITEMS
            Console.WriteLine(6);
            await playlistService.ReplacePlaylistItems(testPlaylistId,tracks);

            //GET CURRENT USERS PLAYLISTS
            Console.WriteLine(7);
            var playlistsCurrent = await playlistService.GetCurrentUsersPlaylists();
            foreach (var pl in playlistsCurrent) {
                Console.WriteLine(pl.Name);
            }

            //GET USERS PLAYLISTS
            Console.WriteLine(8);
            var playlists = await playlistService.GetUsersPlaylists("eren_ozunlu");
            foreach (var pl in playlists)
            {
                Console.WriteLine(pl.Name);
            }

            //CREATE PLAYLIST
            Console.WriteLine(9);
            //var createdPlaylist = await playlistService.CreatePlaylist("eren_ozunlu", "test2",false,"xd");
            //Console.WriteLine(createdPlaylist.Name);

            //GET PLAYLISTS IMAGE
            Console.WriteLine(10);
            var images = await playlistService.GetPlaylistsImage(playlistId);
            Console.WriteLine(images.FirstOrDefault().Url);

            //UPLOAD IMAGE FOR PLAYLIST
            Console.WriteLine(11);
            await playlistService.UploadCustomPlaylistImage(testPlaylistId, @"C:\Users\eren_\Desktop\Denemeler\test1\Api\Api\example_cover.jpeg");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task PlayerServiceTests(SpotifyAuthApi spotifyClient)
    {
        PlayerService playerService = new PlayerService(spotifyClient._apiClient);
        try
        {
            //GET PLAYBACK STATE
            Console.WriteLine(1);
            NowPlayingInfo playerState = await playerService.GetPlaybackState();
            if (playerState != null)
            {
                Console.WriteLine($"{playerState.Device.Name}\n{playerState.Track.Name}");
            }   
            

            //GET AVAILABLE DEVICES
            //AVALİBLE DEVICES OLMAZSA HATA VERİYOR DÜZLET ONU
            Console.WriteLine(2);
            var devices = await playerService.GetAvailableDevices();
            if (devices != null)
            {
                foreach (var device in devices)
                {
                    Console.WriteLine($"Device Name: {device.Name}, Type: {device.Type}, ID: {device.Id}");
                }

            }

            //TRANSFER PLAYBACK
            Console.WriteLine(3);
            var computerId = devices.FirstOrDefault(d => d.Type == "Computer")?.Id;
            await playerService.TransferPlayback(computerId, true);

            //GET CURRENTLY PLAYING TRACK
            Console.WriteLine(4);
            var currentlyPlaying = await playerService.GetCurrentlyPlayingTrack();
            Console.WriteLine(currentlyPlaying.Track.Name);

            //START/RESUME PLAYBACK
            Console.WriteLine(5);
            string uri = "0uUtGVj0y9FjfKful7cABY";
            string uriType = "spotify:album:";
            string contextUri = uriType + uri; // can be a playlist,album or artists 
            List<string> uriList = new List<string> { "spotify:track:46OKHucGhjhskazqD8tKnH", "spotify:track:1brpdmqkx3kSxyqzqXfW7J" }; // can be only a list of tracks 
            OffsetFormat offset1 = new OffsetFormat(4); // this ofsett can be used with context_uri only and it will start playback from the 5th track
            OffsetFormat offset2 = new OffsetFormat("spotify:track:46OKHucGhjhskazqD8tKnH"); // this ofsett can be used with context_uri only and it will start playback from the track with this uri
            await playerService.StartResumePlayback(computerId, contextUri, uris : null,offset2, positionMs: 0); // you can try this with offset1,offset2 or offset : null 
            //await playerService.StartResumePlayback(computerId, contextUri: null, uris: uriList, offset: null, positionMs: 0);// => When You Danced With Me

            //PAUSE PLAYBACK
            Console.WriteLine(6);
            await playerService.PausePlayback(computerId);

            //SKIP TO NEXT
            Console.WriteLine(7);
            await playerService.SkipToNext(computerId);

            //SKIP TO PREVIOUS
            Console.WriteLine(8);
            await playerService.SkipToPrevious(computerId);

            //SEEK THE POSITION
            Console.WriteLine(9);
            await playerService.SeekThePosition(7000);

            //SET REPEAT MODE
            Console.WriteLine(10);
            await playerService.SetRepeatMode(RepeatState.Off);
            //await playerService.SetRepeatMode(RepeatState.Context,computerId);
            //await playerService.SetRepeatMode(RepeatState.Track);

            //SET PLAYBACK VOLUME
            Console.WriteLine(11);
            await playerService.SetPlaybackVolume(50); // Set volume to 50%

            //TOGGLE SHUFFLE MODE
            Console.WriteLine(12);
            await playerService.ToggleShuffleMode(false); // Enable shuffle mode


            //GET RECENTLY PLAYED TRACKS 
            Console.WriteLine(13);
            int index = 1;
            var tracks = await playerService.GetRecentlyPlayedTracks(20);
            foreach (var item in tracks)
            {
                Console.WriteLine($"{index++} Track Name: {item.Name}");
            }

            //GET USERS QUEUE
            Console.WriteLine(14);
            var queue = await playerService.GetUsersQueue();
            foreach (var item in queue)
            {
                Console.WriteLine($"Queue => Track Name: {item.Name}");
            }

            //ADD TO QUEUE
            Console.WriteLine(15);
            string trackUri = "0LPMWPwCHrIorXSdpnPVyv";
            await playerService.AddTrackToQueue(trackUri);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    static async Task ArtistServiceTests(SpotifyAuthApi spotifyClient)
    {
        ArtistService artistService = new ArtistService(spotifyClient._apiClient);
        try
        {
            string artistId = "1dfeR4HaWDbWqFHLkxsg1d";
            string artistId2 = "7Ey4PD4MYsKc5I2dolUwbH";
            var artistIds = new List<string> { artistId, artistId2 };

            //GET ARTIST
            Console.WriteLine(1);
            Artist artist = await artistService.GetArtist(artistId);
            artistService.PrintArtist(artist);

            //GET ARTISTS
            Console.WriteLine(2);
            var artists = await artistService.GetArtists(artistIds);
            foreach (var item in artists)
            {
                artistService.PrintArtist(item);
            }

            //GET ARTİSTS ALBUMS
            //VALID include: album,single,appears_on,compilation
            Console.WriteLine(3);
            List<string> include = new List<string> { "single"};
            var albums = await artistService.GetArtistsAlbums(artistId,include);
            AlbumService albumServiceForArtist = new AlbumService(spotifyClient._apiClient);
            foreach (var album in albums)
            {
                albumServiceForArtist.PrintAlbumDetails(album);
            }


            //GET ARTIST TOP TRACKS
            Console.WriteLine(4);
            var tracks = await artistService.GetArtistsTopTracks(artistId);
            foreach(var track in tracks)
            {
                Console.WriteLine(track.Name);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task AlbumServiceTests(SpotifyAuthApi spotifyClient)
    {
        AlbumService albumService = new AlbumService(spotifyClient._apiClient);
        try
        {

            string albumId = "3AieuV7WztobSMYG86Hdez"; // Example album ID
            string albumId2 = "3i67sGIVw8EBlgfSRv3Lj2"; // Example album ID

            List<string> albumList = new List<string> { albumId, albumId2 };


            //GET ALBUM
            Console.WriteLine(1);
            Album album = await albumService.GetAlbum(albumId);
            albumService.PrintAlbumDetails(album);

            //GET ALBUMS
            Console.WriteLine(2);
            var albums = await albumService.GetAlbums(albumList);
            foreach (var i in albums)
            {
                albumService.PrintAlbumDetails(i);
            }

            //GET ALBUM TRACKS
            Console.WriteLine(3);
            var tracks = await albumService.GetAlbumTracks(albumId);
            foreach (var track in tracks)
            {
                Console.WriteLine($"Track Name: {track.Name}");
                Console.WriteLine($"Track Duration: {track.DurationMs} ms");
                Console.WriteLine($"Track Preview URL: {track.PreviewUrl}");
                Console.WriteLine();
            }


            //GET USERS SAVED ALBUMS
            Console.WriteLine(4);
            var detailedAlbums = await albumService.GetUsersSavedAlbums();
            foreach(var detailedAlbum in detailedAlbums)
            {
                Console.WriteLine(detailedAlbum.Album.Name) ;
                Console.WriteLine(detailedAlbum.AddedAt);
            }


            //SAVE ALBUM FOR USER
            Console.WriteLine(5);
            await albumService.SaveAlbumsForUser(albumList);


            //DELETE ALBUMS FOR USER
            Console.WriteLine(6);
            await albumService.DeleteAlbumsForUser(albumList);


            //CHECK ALBUMS FOR USER
            Console.WriteLine(7);
            var boolList = await albumService.CheckUsersSavedAlbums(albumList);
            foreach (bool item in boolList)
            {
                Console.WriteLine(item);
            }


            //GET NEW RELEASES
            Console.WriteLine(8);
            var newAlbums = await albumService.GetNewReleases();
            foreach (var i in newAlbums)
            {
                albumService.PrintAlbumDetails(i);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

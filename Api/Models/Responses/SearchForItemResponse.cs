namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;

    public partial class SearchForItemResponse
    {
        public Tracks Tracks { get; set; }
        public Artists Artists { get; set; }
        public Albums Albums { get; set; }
        public Playlists Playlists { get; set; }
        public Audiobooks Shows { get; set; }
        public Episodes Episodes { get; set; }
        public Audiobooks Audiobooks { get; set; }
    }


    public partial class Artists
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
        public List<Artist> Items { get; set; }
    }

 

    public partial class Audiobooks
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
        public List<AudiobooksItem> Items { get; set; }
    }

    public partial class AudiobooksItem
    {
        public List<Author> Authors { get; set; }
        public List<Href> AvailableMarkets { get; set; }
        public List<Copyright> Copyrights { get; set; }
        public Href Description { get; set; }
        public Href HtmlDescription { get; set; }
        public string Edition { get; set; }
        public bool Explicit { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Href Href { get; set; }
        public Href Id { get; set; }
        public List<Image> Images { get; set; }
        public List<Href> Languages { get; set; }
        public Href MediaType { get; set; }
        public Href Name { get; set; }
        public List<Author> Narrators { get; set; }
        public Href Publisher { get; set; }
        public string Type { get; set; }
        public Href Uri { get; set; }
        public long? TotalChapters { get; set; }
        public bool? IsExternallyHosted { get; set; }
        public long? TotalEpisodes { get; set; }
    }

    public partial class Author
    {
        public Href Name { get; set; }
    }

    public partial class Copyright
    {
        public Href Text { get; set; }
        public Href Type { get; set; }
    }

    public partial class Episodes
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
        public List<EpisodesItem> Items { get; set; }
    }

    public partial class EpisodesItem
    {
        public Uri AudioPreviewUrl { get; set; }
        public string Description { get; set; }
        public string HtmlDescription { get; set; }
        public long DurationMs { get; set; }
        public bool Explicit { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public bool IsExternallyHosted { get; set; }
        public bool IsPlayable { get; set; }
        public string Language { get; set; }
        public List<string> Languages { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string ReleaseDatePrecision { get; set; }
        public ResumePoint ResumePoint { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public Restrictions Restrictions { get; set; }
    }

    public partial class ResumePoint
    {
        public bool FullyPlayed { get; set; }
        public long ResumePositionMs { get; set; }
    }

    public partial class Playlists
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
        public List<PlaylistsItem> Items { get; set; }
    }

    public partial class PlaylistsItem
    {
        public bool Collaborative { get; set; }
        public Href Description { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Href Href { get; set; }
        public Href Id { get; set; }
        public List<Image> Images { get; set; }
        public Href Name { get; set; }
        public Owner Owner { get; set; }
        public bool Public { get; set; }
        public Href SnapshotId { get; set; }
        public Followers Tracks { get; set; }
        public Href Type { get; set; }
        public Href Uri { get; set; }
    }


    public enum Href { String };
}

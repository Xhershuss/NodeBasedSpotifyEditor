namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;    

    public partial class AvailableDevicesResponse
    {
        public List<Device> Devices { get; set; }
    }


}

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class DeviceDto
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsRestricted { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int VolumePercent { get; set; }
    }
}
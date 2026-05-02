namespace Alumni_Portal.RAID.DTO
{
    public class APIResponse<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public T data { get; set; }
        public int total { get; set; }
    }

}

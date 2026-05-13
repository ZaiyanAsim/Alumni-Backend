namespace Entity_Directories.Services.DTO
{
    public class PaginatedResult<T>
    {
        public List<T> data { get; set; }
        public int totalRecords { get; set; }
        public int _page { get; set; }
        public int _size { get; set; }
    }
}

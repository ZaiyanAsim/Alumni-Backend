namespace Entity_Directories.Services.DTO
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int _page { get; set; }
        public int _size { get; set; }
    }
}

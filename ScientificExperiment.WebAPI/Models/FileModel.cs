namespace WebApi_CSV.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string? Author { get; set; }
        public DateTime? CreationDateTime { get; set; }
    }
}

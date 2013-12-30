namespace ApiDoc.Models
{
    public class ApiDescription : VersionedItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
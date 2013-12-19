namespace ApiDoc.Models
{
    public class MethodDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HttpMethod { get; set; }
        public string Desciption { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public bool Authenticated { get; set; }
        public bool Authorized { get; set; }
        public int ServiceId { get; set; }
    }
}
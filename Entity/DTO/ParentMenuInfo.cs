
namespace Entity.DTO
{
    public class ParentMenuInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Href { get; set; }
        public string Target { get; set; }
        public List<ParentMenuInfo> Child { get; set; }
    }
}

namespace DeAsis_LoginApp.Models
{
    public class Territory 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Parent { get; set; }
        public List<Territory> Children { get; set; }

        public Territory()
        {
            Children = new List<Territory>();
        }
    }
}

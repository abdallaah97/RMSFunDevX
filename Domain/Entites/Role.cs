namespace Domain.Entites
{
    public class Role : BaseEntity
    {
        public int Name { get; set; }
        public SystemRole Code { get; set; }
        public ICollection<User> Users { get; set; }
    }

    public enum SystemRole
    {
        TechnicianAdmin = 1,
        Technician = 2,
        Employee = 3
    }
}

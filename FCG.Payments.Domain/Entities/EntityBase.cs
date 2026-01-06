namespace FCG.Payments.Domain.Entities
{
    public class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

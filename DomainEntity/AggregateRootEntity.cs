namespace DomainEntity
{
    public record AggregateRootEntity :BaseEntity
    {
        public bool IsDeleted { get; private set; } = false;

        public DateTime CreateTime { get; private set; } = DateTime.Now;

        public DateTime? DeleteTime { get; private set; }

        public DateTime? LastModifyTime { get; private set; }

        public virtual void  SoftDelete()
        {
            this.IsDeleted = true;
            this.DeleteTime = DateTime.Now;
        }

        public void NotifyModified()
        {
            this.LastModifyTime = DateTime.Now;
        }
    }
}
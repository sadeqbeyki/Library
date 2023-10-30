using System;

namespace AppFramework.Domain
{
    public class BaseEntity
    {
        public int Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsDeleted { get; set; }

        public BaseEntity()
        {
            CreationDate = DateTime.Now;
            IsDeleted = false;
        }
    }
}

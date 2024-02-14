using System;

namespace AppFramework.Domain
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsDeleted { get; set; }

        public BaseEntity()
        {
            CreationDate = DateTime.Now;
            IsDeleted = false;
        }
    }
}

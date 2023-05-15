using AppFramework.Domain;
using LMS.Domain.BookAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.PublisherAgg
{
    public class Publisher:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Book> Books { get; set; }
        public List<PublisherBook> PublisherBooks { get; set; }

    }
}

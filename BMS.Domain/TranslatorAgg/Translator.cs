using AppFramework.Domain;
using BMS.Domain.BookAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.TranslatorAgg
{
    public class Translator:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Book> Books { get; set; }
    }
}

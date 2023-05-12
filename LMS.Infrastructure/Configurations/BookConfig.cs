using BMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Configurations
{
    internal class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(cs => new { cs.CategoryId, cs.Id });

            builder.HasOne(cs => cs.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(cs => cs.CategoryId);

            //builder.HasOne(cs => cs.Author)
            //    .WithMany(s => s.books)
            //    .HasForeignKey(cs => cs.SubjectId);
        }
    }
}

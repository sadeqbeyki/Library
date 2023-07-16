using LendBook.Domain.LendAgg;
using LendBook.Domain.RentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LendBook.Infrastructure.Configurations;

public class LendConfig : IEntityTypeConfiguration<Lend>
{
    public void Configure(EntityTypeBuilder<Lend> builder)
    {
        builder.ToTable("Lends");
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Operations, modelBuilder =>
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.ToTable("LendOperations");
            modelBuilder.Property(x => x.Descriotion).HasMaxLength(1000);
            modelBuilder.WithOwner(x => x.Lend).HasForeignKey(x => x.LendId);
        });
    }
}


public class RentConfig : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("Rents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IssueTrackingNo).HasMaxLength(8).IsRequired(false);

        builder.OwnsMany(x => x.Items, navigationBuilder =>
        {
            navigationBuilder.ToTable("RentItems");
            navigationBuilder.HasKey(x => x.Id);
            navigationBuilder.WithOwner(x => x.Rent).HasForeignKey(x => x.RentId);
        });
    }
}



//public class BorrowConfig : IEntityTypeConfiguration<Borrow>
//{
//    public void Configure(EntityTypeBuilder<Borrow> builder)
//    {
//        builder.HasKey(cs => new { cs.BookId, cs.UserId });
//        //builder.ToTable("PublisherBooks");

//        builder.HasOne(cs => cs.Book)
//            .WithMany(c => c.bo)
//            .HasForeignKey(cs => cs.);

//        builder.HasOne(cs => cs.Book)
//            .WithMany(s => s.PublisherBooks)
//            .HasForeignKey(cs => cs.BookId);
//    }
//}

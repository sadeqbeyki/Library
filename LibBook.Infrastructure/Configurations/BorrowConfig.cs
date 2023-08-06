﻿using LibBook.Domain.BorrowAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class BorrowConfig : IEntityTypeConfiguration<Borrow>
{
    public void Configure(EntityTypeBuilder<Borrow> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Borrows");
     
        builder.Property(b => b.ReturnDate)
            .IsRequired(false);

        builder.Property(b => b.ReturnEmployeeID)
            .IsRequired(false);

        builder.Property(b => b.Description)
            .IsRequired(false);
    }
}
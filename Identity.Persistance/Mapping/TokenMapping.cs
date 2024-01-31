using Identity.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Identity.Persistance.Mapping
{
    public class TokenMapping : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Token));

            //builder.HasOne(x => x.UserId).WithMany(x => x.Tokens).HasForeignKey(x => x.UserId);
        }
    }
}

using Identity.Domain.Entities.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistance.DatabaseMappings;

//public class RolesConfig : IEntityTypeConfiguration<ApplicationRole>
//{
//    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
//    {
//        builder.HasKey(x => x.Id);
//        builder.ToTable("Roles");

//        List<ApplicationRole> applicationRoles = new(){
//                new(){Id = Guid.NewGuid(),Name = "Member",NormalizedName="MEMBER"},
//                new(){Id = Guid.NewGuid(),Name = "Manager",NormalizedName="MANAGER"},
//                new(){Id = Guid.NewGuid(),Name = "Employee",NormalizedName="EMPLOYEE"},
//                new(){Id = Guid.NewGuid(),Name = "Admin",NormalizedName = "ADMIN"}
//            };

//        builder.HasData(new ApplicationRole { Id = Guid.NewGuid(), Name = "Member", NormalizedName = "MEMBER" });
//        builder.HasData(new ApplicationRole { Id = Guid.NewGuid(), Name = "Manager", NormalizedName = "MANAGER" });
//        builder.HasData(new ApplicationRole { Id = Guid.NewGuid(), Name = "Employee", NormalizedName = "EMPLOYEE" });
//        builder.HasData(new ApplicationRole { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" });
//    }
//}

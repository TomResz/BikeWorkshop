using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.Property(x => x.Id)
			.ValueGeneratedOnAdd();
		builder.HasData(GetBasicRoles());
	}
	private static IEnumerable<Role> GetBasicRoles()
	{
		var roles = new List<Role>();
		roles.Add(new Role { Id = (int)Roles.Worker, Name = "Worker" });
		roles.Add(new Role { Id = (int)Roles.Manager, Name = "Manager" });
		return roles;
	}

}

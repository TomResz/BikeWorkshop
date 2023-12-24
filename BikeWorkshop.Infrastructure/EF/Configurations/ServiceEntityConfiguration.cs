using BikeWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;

internal class ServiceEntityConfiguration
	: IEntityTypeConfiguration<Service>
{
	public void Configure(EntityTypeBuilder<Service> builder)
	{
		builder.HasIndex(x => x.Name)
			.IsUnique();
		builder.HasMany(x=>x.ServiceToOrders)
			.WithOne(x=>x.Service)
			.OnDelete(DeleteBehavior.Restrict);
	}
}

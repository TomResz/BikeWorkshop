using BikeWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;
internal class ServiceToOrderEntityConfiguration
	: IEntityTypeConfiguration<ServiceToOrder>
{
	public void Configure(EntityTypeBuilder<ServiceToOrder> builder)
	{
		builder.Property(x => x.Price)
			.HasPrecision(7, 2);
	}
}

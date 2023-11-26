using BikeWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.HasOne(x => x.Summary)
			.WithOne(x => x.Order)
			.OnDelete(DeleteBehavior.Cascade)
			.HasForeignKey<Summary>(x=>x.OrderId)
			.IsRequired(false);
	}
}

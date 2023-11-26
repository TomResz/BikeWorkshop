using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BikeWorkshop.Infrastructure.EF.Configurations;

internal class OrderStatusEntityConfiguration : IEntityTypeConfiguration<OrderStatus>
{
	
	public void Configure(EntityTypeBuilder<OrderStatus> builder)
	{
		builder.Property(x => x.Id)
			.ValueGeneratedOnAdd();
		builder.HasData(GetData());
	}
	private static IEnumerable<OrderStatus> GetData()
	{
		var statuses = new List<OrderStatus>();
		statuses.Add(new OrderStatus { Id = (int)Status.During, Name = "During" });
		statuses.Add(new OrderStatus { Id = (int)Status.Completed, Name = "Completed" });
		statuses.Add(new OrderStatus { Id = (int)Status.Retrieved, Name = "Retrieved" });
		return statuses;

	}
}

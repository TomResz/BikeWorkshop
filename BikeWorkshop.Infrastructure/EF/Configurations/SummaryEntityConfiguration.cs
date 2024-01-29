using BikeWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;
internal class SummaryEntityConfiguration
	: IEntityTypeConfiguration<Summary>
{
	public void Configure(EntityTypeBuilder<Summary> builder)
	{
		builder.Property(x => x.TotalPrice)
			.HasPrecision(7, 2);
	}
}

using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Data.Configurations;

public sealed class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("OrderLines");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.MenuItemName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.MenuItemType).HasConversion<int>().IsRequired();
        builder.Property(x => x.UnitPrice).HasPrecision(10, 2).IsRequired();
    }
}
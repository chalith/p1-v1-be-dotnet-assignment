using System;
using Domain.Aggregates.OrderAggregate;
using Domain.Aggregates.FlightAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class OrderFlightRateEntityTypeConfiguration
        : BaseEntityTypeConfiguration<OrderFlightRate>
    {
        public override void Configure(EntityTypeBuilder<OrderFlightRate> builder)
        {
            base.Configure(builder);

            builder.Property("Quantity").IsRequired();
            builder.Property<Guid>("OrderId").IsRequired();

            builder
                .HasOne<FlightRate>(o => o.FlightRate)
                .WithMany()
                .IsRequired()
                .HasForeignKey("FlightRateId");
        }
    }
}

using System;
using Domain.Common;
using Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : BaseEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            /**
            Order can have mutiple rates from multiple flights but in the same currency.
            Relationship between Order and FlightRate is many to many.
            // Since order's flight rate should contain a quantity we add this new entity OrderFlightRate.
            **/
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderFlightRates));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property<OrderStatus>("Status").IsRequired();
            builder.Property("Email").IsRequired();

            builder.OwnsOne(
                o => o.Price,
                a =>
                {
                    a.Property<Guid>("OrderId");
                    a.WithOwner();
                }
            );
        }
    }
}

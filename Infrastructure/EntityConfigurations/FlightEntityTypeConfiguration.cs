using Domain.Aggregates.AirportAggregate;
using Domain.Aggregates.FlightAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class FlightEntityTypeConfiguration : BaseEntityTypeConfiguration<Flight>
    {
        public override void Configure(EntityTypeBuilder<Flight> builder)
        {
            base.Configure(builder);

            var navigation = builder.Metadata.FindNavigation(nameof(Flight.Rates));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property("Arrival").IsRequired();
            builder.Property("Departure").IsRequired();
            
            builder.HasOne<Airport>(f => f.OriginAirport)
                .WithMany()
                .IsRequired()
                .HasForeignKey("OriginAirportId");
            
            builder.HasOne<Airport>(f => f.DestinationAirport)
                .WithMany()
                .IsRequired()
                .HasForeignKey("DestinationAirportId");
        }
    }
}
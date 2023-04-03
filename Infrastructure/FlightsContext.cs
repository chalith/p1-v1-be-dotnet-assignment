using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.AirportAggregate;
using Domain.Aggregates.FlightAggregate;
using Domain.Aggregates.OrderAggregate;
using Domain.SeedWork;
using Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class FlightsContext : DbContext, IUnitOfWork
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightRate> FlightRates { get; set; }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderFlightRate> OrderFlightRates { get; set; }
        
        private readonly IMediator _mediator;
        
        public FlightsContext(DbContextOptions<FlightsContext> options) : base(options) { }
        
        
        public FlightsContext(DbContextOptions<FlightsContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntityTypeConfiguration<>).Assembly);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
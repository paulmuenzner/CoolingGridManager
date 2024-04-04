using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Models.Data;

// dotnet ef migrations add <MigrationName>
// dotnet ef database update
public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    // DbSet property
    public DbSet<Billing> Bills { get; set; }
    public DbSet<Consumer> Consumers { get; set; }
    public DbSet<Grid> Grids { get; set; }
    public DbSet<CoolingGridParameterLog> CoolingGridParameterLogs { get; set; }
    public DbSet<GridSection> GridSections { get; set; }
    public DbSet<ConsumptionConsumer> ConsumptionConsumers { get; set; }
    public DbSet<ConsumptionGrid> ConsumptionGrids { get; set; }
    public DbSet<TicketModel> Tickets { get; set; }



    // Constructor for AppDbContext (optional)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    // Additional DbSet properties and configurations can be added here
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grid>()
                .HasIndex(g => g.GridName)
                .IsUnique();

        modelBuilder.Entity<GridSection>()
                .HasIndex(g => g.GridSectionName)
                .IsUnique();

        modelBuilder.Entity<GridSection>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.GridSection)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<CoolingGridParameterLog>()
            .HasIndex(g => g.EndDate)
            .IsUnique();

        modelBuilder.Entity<CoolingGridParameterLog>()
            .HasIndex(g => g.StartDate)
            .IsUnique();

        modelBuilder.Entity<CoolingGridParameterLog>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.CoolingGridParameterLog)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<CoolingGridParameterLog>()
            .Property(mb => mb.TemperatureIn)
            .HasColumnType("decimal(2, 5)");

        modelBuilder.Entity<CoolingGridParameterLog>()
            .Property(mb => mb.TemperatureOut)
            .HasColumnType("decimal(2, 5)");

        modelBuilder.Entity<ConsumptionGrid>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.ConsumptionGrid)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<ConsumptionGrid>()
            .Property(mb => mb.Consumption)
            .HasColumnType("decimal(15, 4)");

        modelBuilder.Entity<Billing>()
            .HasOne(gs => gs.Consumer)
            .WithMany()
            .HasForeignKey(gs => gs.ConsumerID);

        modelBuilder.Entity<Billing>()
            .Property(mb => mb.TotalConsumption)
            .HasColumnType("decimal(18, 3)");

        modelBuilder.Entity<Billing>()
            .Property(mb => mb.BillingAmount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ConsumptionConsumer>()
            .HasOne(gs => gs.Consumer)
            .WithMany()
            .HasForeignKey(gs => gs.ConsumerID);

        modelBuilder.Entity<ConsumptionConsumer>()
            .Property(mb => mb.ConsumptionValue)
            .HasColumnType("decimal(11, 3)");

        modelBuilder.Entity<Consumer>()
            .HasOne(gs => gs.GridSection)
            .WithMany()
            .HasForeignKey(gs => gs.GridSectionID);

        base.OnModelCreating(modelBuilder);
    }

}



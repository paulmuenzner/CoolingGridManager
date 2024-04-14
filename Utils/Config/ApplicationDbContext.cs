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
    public DbSet<GridParameterLog> GridParameterLog { get; set; }
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

        modelBuilder.Entity<GridParameterLog>()
            .HasIndex(g => g.DateTimeEnd)
            .IsUnique();

        modelBuilder.Entity<GridParameterLog>()
            .HasIndex(g => g.DateTimeStart)
            .IsUnique();

        modelBuilder.Entity<GridParameterLog>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.GridParameterLog)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<GridParameterLog>()
            .Property(mb => mb.MeanTemperatureIn)
            .HasColumnType("decimal(2, 2)");

        modelBuilder.Entity<GridParameterLog>()
            .Property(mb => mb.MeanTemperatureOut)
            .HasColumnType("decimal(2, 2)");

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

        modelBuilder.Entity<Consumer>()
            .Property(mb => mb.MonthlyBaseFee)
            .HasColumnType("decimal(5, 2)");

        modelBuilder.Entity<Consumer>()
            .Property(mb => mb.UnitPrice)
            .HasColumnType("decimal(5, 2)");

        base.OnModelCreating(modelBuilder);
    }

}



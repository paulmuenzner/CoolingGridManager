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
    public DbSet<GridEfficiency> GridEfficiencies { get; set; }
    public DbSet<GridSection> GridSections { get; set; }
    public DbSet<ConsumptionConsumer> ConsumptionConsumers { get; set; }
    public DbSet<GridEnergyTransfer> GridEnergyTransfers { get; set; }
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
            .HasIndex(gs => gs.GridSectionName)
            .IsUnique();

        modelBuilder.Entity<GridSection>()
            .HasOne(gs => gs.Grid)
            .WithMany(gs => gs.GridSection)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<GridEfficiency>()
            .HasOne(e => e.Grid)
            .WithMany(e => e.GridEfficiency)
            .HasForeignKey(e => e.GridID);

        modelBuilder.Entity<GridEfficiency>()
            .Property(e => e.EfficiencyRelative)
            .HasColumnType("decimal(1, 10)");

        modelBuilder.Entity<GridParameterLog>()
            .HasIndex(gpl => gpl.DateTimeEnd)
            .IsUnique();

        modelBuilder.Entity<GridParameterLog>()
            .HasIndex(g => g.DateTimeStart)
            .IsUnique();

        modelBuilder.Entity<GridParameterLog>()
            .HasOne(gpl => gpl.Grid)
            .WithMany(gpl => gpl.GridParameterLog)
            .HasForeignKey(gpl => gpl.GridID);

        modelBuilder.Entity<GridParameterLog>()
            .Property(gpl => gpl.MeanTemperatureIn)
            .HasColumnType("decimal(4, 2)");

        modelBuilder.Entity<GridParameterLog>()
            .Property(gpl => gpl.MeanTemperatureOut)
            .HasColumnType("decimal(4, 2)");

        modelBuilder.Entity<GridParameterLog>()
            .HasIndex(cc => cc.ElementID)
            .IsUnique();

        modelBuilder.Entity<GridEnergyTransfer>()
            .HasOne(cg => cg.Grid)
            .WithMany(cg => cg.GridEnergyTransfer)
            .HasForeignKey(cg => cg.GridID);

        modelBuilder.Entity<GridEnergyTransfer>()
            .Property(cg => cg.EnergyTransfer)
            .HasColumnType("decimal(20, 4)");

        modelBuilder.Entity<Billing>()
            .HasOne(b => b.Consumer)
            .WithMany()
            .HasForeignKey(b => b.ConsumerID);

        modelBuilder.Entity<Billing>()
            .Property(b => b.TotalConsumption)
            .HasColumnType("decimal(18, 3)");

        modelBuilder.Entity<Billing>()
            .Property(b => b.BillingAmount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ConsumptionConsumer>()
            .HasOne(cc => cc.Consumer)
            .WithMany()
            .HasForeignKey(cc => cc.ConsumerID);

        modelBuilder.Entity<ConsumptionConsumer>()
            .Property(cc => cc.ConsumptionValue)
            .HasColumnType("decimal(12, 3)");

        modelBuilder.Entity<ConsumptionConsumer>()
            .HasIndex(cc => cc.ElementID)
            .IsUnique();

        modelBuilder.Entity<Consumer>()
            .HasOne(c => c.GridSection)
            .WithMany()
            .HasForeignKey(c => c.GridSectionID);

        modelBuilder.Entity<Consumer>()
            .Property(c => c.MonthlyBaseFee)
            .HasColumnType("decimal(7, 2)");

        modelBuilder.Entity<Consumer>()
            .Property(c => c.UnitPrice)
            .HasColumnType("decimal(7, 2)");

        base.OnModelCreating(modelBuilder);
    }

}



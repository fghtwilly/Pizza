using Microsoft.EntityFrameworkCore;

namespace Pizza.Models.DBContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDish> orderDish { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderDish>().HasKey(od => new { od.IdOrder, od.IdDish }); ;
            /*modelBuilder
                .Entity<Order>(
                    eb =>
                    {
                        eb.HasNoKey();
                        eb.ToTable("OrderTable");
                    });*/
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PizzaBase;Username=postgres;Password=89887683814");
        }
    }
}

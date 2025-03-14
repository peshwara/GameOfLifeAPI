namespace GameOfLifeAPI.Data
{
    using GameOfLifeAPI.Models;
    using Microsoft.EntityFrameworkCore;

    public class GameOfLifeContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }

        public GameOfLifeContext(DbContextOptions<GameOfLifeContext> options) : base(options) { }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Board>()
        //         .HasKey(b => b.Id);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .Ignore(b => b.StateArray); // Ensure StateArray is ignored

            base.OnModelCreating(modelBuilder);
        }
    }
}

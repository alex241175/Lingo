using System;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Models
{
    public class LingoDbContext: DbContext
    {
        public LingoDbContext(DbContextOptions<LingoDbContext> options) : base (options)
        {
            
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Vocab>()
        //     .Property(e => e.EssayIds)
        //     .HasConversion(
        //         v => String.Join(',', v),   // set
        //         v => Array.ConvertAll(v.Split(',', StringSplitOptions.RemoveEmptyEntries), Int32.Parse)  // get
        //     );
        // }

        public DbSet<User> Users { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<Vocab> Vocabs { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ReviewSetting> ReviewSettings { get; set; }
    }
}
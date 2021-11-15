using System;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Models
{
    public class LingoDbContext: DbContext
    {
        public LingoDbContext(DbContextOptions<LingoDbContext> options) : base (options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<Vocab> Vocabs { get; set; }
        public DbSet<Sample> Samples { get; set; }
    }
}
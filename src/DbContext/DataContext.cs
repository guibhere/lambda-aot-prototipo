using DataBaseContext.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DataBaseContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        protected DataContext()
        {
        }
        public DbSet<Person> Persons { get; set; } = null!;
    }
}
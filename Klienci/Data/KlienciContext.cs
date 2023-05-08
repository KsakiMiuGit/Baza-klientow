using Klienci.Models;
using Microsoft.EntityFrameworkCore;

namespace Klienci.Data
{
    public class KlienciContext:DbContext
    {
     
        public KlienciContext(DbContextOptions options)
           : base(options)
        {
        }

        public DbSet<Klient> klienci { get; set; }
    }
}

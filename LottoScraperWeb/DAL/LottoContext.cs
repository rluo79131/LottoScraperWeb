using System.Data.Entity;

namespace LottoScraperWeb.DAL
{
    public class LottoContext : DbContext
    {
        public DbSet<Information> Informations { get; set; }

        public LottoContext() : base("name=DB")
        {
        }
    }
}
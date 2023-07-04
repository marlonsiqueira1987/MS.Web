using MS.Web.UI.Models;
using System.Data.Entity;

namespace MS.Web.UI.Data
{
    public class MSWebDataContext:DbContext
    {
        public MSWebDataContext():base("StoreConn")
        {
            Database.SetInitializer(new DbInitializer());
        }

        /// <summary>
        /// Tabelas do sistemas
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Enderecos> Enderecos { get; set; }

    }
}

using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Net;
using MSLServer.Data.DBSeed;
using MSLServer.Models;
using MSLServer.Models.User;
using MSLServer.Models.Server;

namespace MSLServer.Data
{
    public partial class ServerListDBContext : DbContext
    {
        #region DBSets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        #endregion
        public ServerListDBContext()
        {
            Database.EnsureCreated();
        }

        #region Configuring
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                string conn =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Data\ServerListDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=true";
                builder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(conn);
            }
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder mb)
        {
            UserDBSeed.LoadData(mb);
            ServerDBSeed.LoadData(mb);
        }
    }
}
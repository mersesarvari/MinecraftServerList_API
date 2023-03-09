using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Net;
using MSLServer.Data.DBSeed;
using MSLServer.Models;

namespace MSLServer.Data
{
    public partial class ServerListDBContext : DbContext
    {
        #region DBSets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<ServerThumbnail> ServerThumbnails { get; set; }
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
            //mb.Entity<User>().HasIndex(X => X.Email).IsUnique();

            //// DeleteBehavior.NoAction mindenhova
            ////mb.Entity<Server>(entity =>
            ////{
            ////    entity.HasOne(x => x.User)
            ////        .WithMany(y => y.Servers)
            ////        .HasForeignKey(x => x.UserID)
            ////        .OnDelete(DeleteBehavior.Restrict);
            ////});
            UserDBSeed.LoadData(mb);
            ServerDBSeed.LoadData(mb);
        }
    }
}
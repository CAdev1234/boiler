using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ykmWeb.Models;




namespace ykmWeb.Dal
{
    /// <summary>
    /// 数据上下文
    /// <remarks></remarks>
    /// </summary>
    public class ykmWebDbContext : DbContext
    {
        public DbSet<info> db_info { get; set; }
        public DbSet<menuClass> db_infoclass { get; set; }
        public DbSet<ggw> db_ggw { get; set; }
        public DbSet<webmanager> db_webmaster { get; set; }
        public DbSet<siteseo> db_site_seo { get; set; }
        public DbSet<link> db_link { get; set; }
        public DbSet<guestbook> db_guestbook { get; set; }
        public DbSet<visitor> db_visitor { get; set; }

        public ykmWebDbContext() : base("DbHelperConnectionString")
        {
            Database.SetInitializer<ykmWebDbContext>(null);//
                                                           //      Configuration.LazyLoadingEnabled = false; //延迟加载==false
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}

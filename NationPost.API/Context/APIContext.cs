using NationPost.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NationPost.API.Context
{

    public class APIContext : DbContext
    {
        public APIContext()
            : base("DefaultConnection")
        {
            //       Database.SetInitializer<APIContext>(
            //new MigrateDatabaseToLatestVersion<APIContext, NationPost.API.Migrations.Configuration>());
         //   Database.SetInitializer<APIContext>(new CreateDatabaseIfNotExists<APIContext>());
            //Disable initializer
            Database.SetInitializer<APIContext>(null);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleTags> ArticleTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleRatings> ArticleRatings { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
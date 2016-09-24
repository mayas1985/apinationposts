namespace NationPost.API.Migrations
{
    using NationPost.API.Context;
    using NationPost.API.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<APIContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(APIContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Users.AddOrUpdate(
              p => p.UserId,
                  new User { UserId = new Guid("02fdad93-2a4c-40d0-bc22-944af629f7c4"), CreatedOn = DateTime.Now, FirstName = "Admin", UserName = "Admin", coords = DbGeography.FromText("POINT(-122.336106 47.605049)"), Password="Admin" }
                //new Person { FullName = "Andrew Peters" },
                //new Person { FullName = "Brice Lambson" },
                //new Person { FullName = "Rowan Miller" }
            );
            context.ArticleTypes.AddOrUpdate(

                j => j.ArticleTypeId,
                new ArticleType { ArticleTypeId = 1, Name = "News" },
                new ArticleType { ArticleTypeId = 2, Name = "Opinion" },
                new ArticleType { ArticleTypeId = 3, Name = "Event" }
                );

            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                //This will show up in nuget window 
                throw new Exception(sb.ToString());
            }

            //
        }
    }
}

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
            AutomaticMigrationsEnabled = false;
                //; AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(APIContext context)
        {
            ////  This method will be called after migrating to the latest version.

            ////  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            ////  to avoid creating duplicate seed data. E.g.
            ////
            ////context.Users.AddOrUpdate(
            ////  p => p.UserId,
            ////      new User { UserId = new Guid("02fdad93-2a4c-40d0-bc22-944af629f7c4"), CreatedOn = DateTime.Now, FirstName = "Admin", UserName = "Admin", coords = DbGeography.FromText("POINT(-122.336106 47.605049)"), Password="Admin" }
            ////    //new Person { FullName = "Andrew Peters" },
            ////    //new Person { FullName = "Brice Lambson" },
            ////    //new Person { FullName = "Rowan Miller" }
            ////);
            //context.ArticleTypes.AddOrUpdate(
            //    j => j.ArticleTypeId,
            //    new ArticleType
            //    {
            //        ArticleTypeId = 1,
            //        Name = "News"

            //    }
            //    );
            //context.ArticleTypes.AddOrUpdate(
            //    j => j.ArticleTypeId,
            //    new ArticleType
            //    {
            //        ArticleTypeId = 2,
            //        Name = "Opinion"

            //    }
            //    );

            //var usr = context.Users.FirstOrDefault(k => k.UserId == new Guid("02fdad93-2a4c-40d0-bc22-944af629f7c4"));
            //if (usr != null)
            //{
            //    context.Users.Remove(usr);

            //    context.SaveChanges();

            //}

            //context.Articles.AddOrUpdate(
            //    j => j.ArticleId,
            //    new Article
            //    {
            //        ArticleId = new Guid("20a9102b-696d-45b5-94c9-92889d630b74"),
            //        ArticleTypeId = new ArticleType { ArticleTypeId = 1, Name = "News" },
            //        Body = "Seed data",
            //        CreatedBy = new User { UserId = new Guid("02fdad93-2a4c-40d0-bc22-944af629f7c4"), CreatedOn = DateTime.Now, FirstName = "Admin", UserName = "Admin", coords = DbGeography.FromText("POINT(-122.336106 47.605049)"), Password = "Admin" },
            //        CreatedOn = DateTime.Now,
            //        Description = "Seed data"

            //    }
            //    );
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Legal Mishaps in India", Name = "Legal Mishaps in India" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Government Policies", Name = "Government Policies" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Politics", Name = "Politics" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "SOCIO-POLITICAL", Name = "SOCIO-POLITICAL" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Economic Development ", Name = "Economic Development " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Finance ", Name = "Finance " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Equitable GDP growth ", Name = "Equitable GDP growth " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Balanced politics", Name = "Balanced politics" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Make UN effective", Name = " Make UN effective" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "POLITICS OF GANDHIAN", Name = "POLITICS OF GANDHIAN" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "FURTHER PANCHAYAT RAJ", Name = " FURTHER PANCHAYAT RAJ" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "RAMRAJYA OF GANDHIAN DREAMS ", Name = " RAMRAJYA OF GANDHIAN DREAMS " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "ELECTION WITH NIL EXPENDITURE EMPOWERED PANCHAYAT", Name = "ELECTION WITH NIL EXPENDITURE EMPOWERED PANCHAYAT" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "RIGHT TO Recall", Name = "RIGHT TO Recall" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "politics culture ", Name = "politics culture " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "world news ", Name = "world news " });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "science", Name = "science" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Demonetization", Name = "Demonetization" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Spirituality", Name = "Spirituality" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Current Affairs", Name = "Current Affairs" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Sports", Name = "Sports" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Cinema", Name = "Cinema" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Modi", Name = "Modi" });
            context.Tags.AddOrUpdate(j => j.Id, new Tag { Description = "Economy", Name = "Economy" });


            //try
            //{
            //    context.SaveChanges();
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            //{
            //    var sb = new System.Text.StringBuilder();
            //    foreach (var failure in ex.EntityValidationErrors)
            //    {
            //        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
            //        foreach (var error in failure.ValidationErrors)
            //        {
            //            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
            //            sb.AppendLine();
            //        }
            //    }
            //    //This will show up in nuget window 
            //    throw new Exception(sb.ToString());
            //}

            context.ArticleTypes.AddOrUpdate(

                j => j.ArticleTypeId,
                new ArticleType { ArticleTypeId = 1, Name = "News" },
                new ArticleType { ArticleTypeId = 2, Name = "Opinion" }
                );

            context.SaveChanges();
            //
        }
    }
}

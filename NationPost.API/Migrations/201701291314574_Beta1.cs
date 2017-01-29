namespace NationPost.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Beta1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "NP.ArticleRatings",
                c => new
                    {
                        ArticleRatingId = c.Int(nullable: false, identity: true),
                        ArticleId = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        Rating = c.Int(nullable: false),
                        ratingType = c.Int(nullable: false),
                        IPAdditionalInfo = c.String(),
                    })
                .PrimaryKey(t => t.ArticleRatingId);
            
            CreateTable(
                "NP.Articles",
                c => new
                    {
                        ArticleId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        Summary = c.String(maxLength: 250),
                        Body = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Rating = c.Int(nullable: false),
                        TotalRating = c.Int(nullable: false),
                        Like = c.Int(nullable: false),
                        Dislike = c.Int(nullable: false),
                        Longtitude = c.String(),
                        Latitude = c.String(),
                        Country = c.String(),
                        administrative_area_level_1 = c.String(),
                        administrative_area_level_2 = c.String(),
                        locality = c.String(),
                        sublocality_level_1 = c.String(),
                        sublocality_level_2 = c.String(),
                        sublocality_level_3 = c.String(),
                        IP = c.String(),
                        ArticleTypeId_ArticleTypeId = c.Int(nullable: false),
                        CreatedBy_UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("NP.ArticleType", t => t.ArticleTypeId_ArticleTypeId, cascadeDelete: true)
                .ForeignKey("NP.Users", t => t.CreatedBy_UserId, cascadeDelete: true)
                .Index(t => t.ArticleTypeId_ArticleTypeId)
                .Index(t => t.CreatedBy_UserId);
            
            CreateTable(
                "NP.ArticleTags",
                c => new
                    {
                        ArticleTagId = c.Int(nullable: false, identity: true),
                        article_ArticleId = c.Guid(),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ArticleTagId)
                .ForeignKey("NP.Articles", t => t.article_ArticleId)
                .ForeignKey("NP.Tag", t => t.Tag_Id)
                .Index(t => t.article_ArticleId)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "NP.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "NP.ArticleType",
                c => new
                    {
                        ArticleTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ArticleTypeId);
            
            CreateTable(
                "NP.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 15),
                        LastName = c.String(maxLength: 15),
                        Email = c.String(maxLength: 350),
                        Password = c.String(nullable: false, maxLength: 15),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        AboutMe = c.String(),
                        IsAboutMeVisible = c.Boolean(nullable: false),
                        FacebookLink = c.String(),
                        IsFacebookLinkVisible = c.Boolean(nullable: false),
                        TwitterLink = c.String(),
                        IsTwitterLinkVisible = c.Boolean(nullable: false),
                        Contact = c.String(),
                        IsContactVisible = c.String(),
                        Token = c.String(),
                        GoogleLink = c.String(),
                        IsGoogleLinkVisible = c.String(),
                        UserName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("NP.Articles", "CreatedBy_UserId", "NP.Users");
            DropForeignKey("NP.Articles", "ArticleTypeId_ArticleTypeId", "NP.ArticleType");
            DropForeignKey("NP.ArticleTags", "Tag_Id", "NP.Tag");
            DropForeignKey("NP.ArticleTags", "article_ArticleId", "NP.Articles");
            DropIndex("NP.Users", new[] { "Email" });
            DropIndex("NP.ArticleTags", new[] { "Tag_Id" });
            DropIndex("NP.ArticleTags", new[] { "article_ArticleId" });
            DropIndex("NP.Articles", new[] { "CreatedBy_UserId" });
            DropIndex("NP.Articles", new[] { "ArticleTypeId_ArticleTypeId" });
            DropTable("NP.Users");
            DropTable("NP.ArticleType");
            DropTable("NP.Tag");
            DropTable("NP.ArticleTags");
            DropTable("NP.Articles");
            DropTable("NP.ArticleRatings");
        }
    }
}

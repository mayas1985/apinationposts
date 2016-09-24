namespace NationPost.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "NP.Articles",
                c => new
                    {
                        ArticleId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Rating = c.Int(nullable: false),
                        Like = c.Int(nullable: false),
                        Dislike = c.Int(nullable: false),
                        coords = c.Geography(),
                        ArticleTypeId_ArticleTypeId = c.Int(),
                        CreatedBy_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("NP.ArticleType", t => t.ArticleTypeId_ArticleTypeId)
                .ForeignKey("NP.Users", t => t.CreatedBy_UserId)
                .Index(t => t.ArticleTypeId_ArticleTypeId)
                .Index(t => t.CreatedBy_UserId);
            
            CreateTable(
                "NP.ArticleTags",
                c => new
                    {
                        ArticleTagId = c.Int(nullable: false, identity: true),
                        article_ArticleId = c.Guid(),
                        Tag_TagId = c.Int(),
                    })
                .PrimaryKey(t => t.ArticleTagId)
                .ForeignKey("NP.Articles", t => t.article_ArticleId)
                .ForeignKey("NP.Tag", t => t.Tag_TagId)
                .Index(t => t.article_ArticleId)
                .Index(t => t.Tag_TagId);
            
            CreateTable(
                "NP.Tag",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        TagDescription = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
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
                        UserName = c.String(nullable: false, maxLength: 15),
                        FirstName = c.String(maxLength: 15),
                        LastName = c.String(maxLength: 15),
                        Email = c.String(),
                        Password = c.String(nullable: false, maxLength: 15),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        coords = c.Geography(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("NP.Articles", "CreatedBy_UserId", "NP.Users");
            DropForeignKey("NP.Articles", "ArticleTypeId_ArticleTypeId", "NP.ArticleType");
            DropForeignKey("NP.ArticleTags", "Tag_TagId", "NP.Tag");
            DropForeignKey("NP.ArticleTags", "article_ArticleId", "NP.Articles");
            DropIndex("NP.ArticleTags", new[] { "Tag_TagId" });
            DropIndex("NP.ArticleTags", new[] { "article_ArticleId" });
            DropIndex("NP.Articles", new[] { "CreatedBy_UserId" });
            DropIndex("NP.Articles", new[] { "ArticleTypeId_ArticleTypeId" });
            DropTable("NP.Users");
            DropTable("NP.ArticleType");
            DropTable("NP.Tag");
            DropTable("NP.ArticleTags");
            DropTable("NP.Articles");
        }
    }
}

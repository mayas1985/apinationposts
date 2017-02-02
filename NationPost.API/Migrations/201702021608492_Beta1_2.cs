namespace NationPost.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Beta1_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("NP.Users", "IsContactVisible", c => c.Boolean(nullable: false));
            AlterColumn("NP.Users", "IsGoogleLinkVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("NP.Users", "IsGoogleLinkVisible", c => c.String());
            AlterColumn("NP.Users", "IsContactVisible", c => c.String());
        }
    }
}

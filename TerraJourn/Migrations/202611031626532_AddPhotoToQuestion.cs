namespace TerraJourn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoToQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "PhotoUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "PhotoUrl");
        }
    }
}

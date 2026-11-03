namespace TerraJourn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutUs",
                c => new
                    {
                        AboutUsId = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.AboutUsId);
            
            CreateTable(
                "dbo.BlogCategories",
                c => new
                    {
                        BlogCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IconClass = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.BlogCategoryId);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.ContactInformations",
                c => new
                    {
                        ContactInfoId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ContactInfoId);
            
            CreateTable(
                "dbo.PlaceCategories",
                c => new
                    {
                        PlaceCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImageUrl = c.String(maxLength: 500),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PlaceCategoryId);
            
            CreateTable(
                "dbo.TouristAttractions",
                c => new
                    {
                        TouristAttractionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        Country = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.TouristAttractionId);
            
            CreateTable(
                "dbo.BlogBlogCategories",
                c => new
                    {
                        Blog_BlogId = c.Int(nullable: false),
                        BlogCategory_BlogCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Blog_BlogId, t.BlogCategory_BlogCategoryId })
                .ForeignKey("dbo.Blogs", t => t.Blog_BlogId, cascadeDelete: true)
                .ForeignKey("dbo.BlogCategories", t => t.BlogCategory_BlogCategoryId, cascadeDelete: true)
                .Index(t => t.Blog_BlogId)
                .Index(t => t.BlogCategory_BlogCategoryId);
            
            CreateTable(
                "dbo.TouristAttractionPlaceCategories",
                c => new
                    {
                        TouristAttraction_TouristAttractionId = c.Int(nullable: false),
                        PlaceCategory_PlaceCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TouristAttraction_TouristAttractionId, t.PlaceCategory_PlaceCategoryId })
                .ForeignKey("dbo.TouristAttractions", t => t.TouristAttraction_TouristAttractionId, cascadeDelete: true)
                .ForeignKey("dbo.PlaceCategories", t => t.PlaceCategory_PlaceCategoryId, cascadeDelete: true)
                .Index(t => t.TouristAttraction_TouristAttractionId)
                .Index(t => t.PlaceCategory_PlaceCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TouristAttractionPlaceCategories", "PlaceCategory_PlaceCategoryId", "dbo.PlaceCategories");
            DropForeignKey("dbo.TouristAttractionPlaceCategories", "TouristAttraction_TouristAttractionId", "dbo.TouristAttractions");
            DropForeignKey("dbo.Blogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.BlogBlogCategories", "BlogCategory_BlogCategoryId", "dbo.BlogCategories");
            DropForeignKey("dbo.BlogBlogCategories", "Blog_BlogId", "dbo.Blogs");
            DropIndex("dbo.TouristAttractionPlaceCategories", new[] { "PlaceCategory_PlaceCategoryId" });
            DropIndex("dbo.TouristAttractionPlaceCategories", new[] { "TouristAttraction_TouristAttractionId" });
            DropIndex("dbo.BlogBlogCategories", new[] { "BlogCategory_BlogCategoryId" });
            DropIndex("dbo.BlogBlogCategories", new[] { "Blog_BlogId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Comments", new[] { "BlogId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Blogs", new[] { "UserId" });
            DropTable("dbo.TouristAttractionPlaceCategories");
            DropTable("dbo.BlogBlogCategories");
            DropTable("dbo.TouristAttractions");
            DropTable("dbo.PlaceCategories");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Blogs");
            DropTable("dbo.BlogCategories");
            DropTable("dbo.AboutUs");
        }
    }
}

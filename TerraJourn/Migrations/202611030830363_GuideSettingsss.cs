namespace TerraJourn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuideSettingsss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TouristAttractionPlaceCategories", "TouristAttraction_TouristAttractionId", "dbo.TouristAttractions");
            DropForeignKey("dbo.TouristAttractionPlaceCategories", "PlaceCategory_PlaceCategoryId", "dbo.PlaceCategories");
            DropIndex("dbo.TouristAttractionPlaceCategories", new[] { "TouristAttraction_TouristAttractionId" });
            DropIndex("dbo.TouristAttractionPlaceCategories", new[] { "PlaceCategory_PlaceCategoryId" });
            DropTable("dbo.PlaceCategories");
            DropTable("dbo.TouristAttractions");
            DropTable("dbo.TouristAttractionPlaceCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TouristAttractionPlaceCategories",
                c => new
                    {
                        TouristAttraction_TouristAttractionId = c.Int(nullable: false),
                        PlaceCategory_PlaceCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TouristAttraction_TouristAttractionId, t.PlaceCategory_PlaceCategoryId });
            
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
                "dbo.PlaceCategories",
                c => new
                    {
                        PlaceCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImageUrl = c.String(maxLength: 500),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PlaceCategoryId);
            
            CreateIndex("dbo.TouristAttractionPlaceCategories", "PlaceCategory_PlaceCategoryId");
            CreateIndex("dbo.TouristAttractionPlaceCategories", "TouristAttraction_TouristAttractionId");
            AddForeignKey("dbo.TouristAttractionPlaceCategories", "PlaceCategory_PlaceCategoryId", "dbo.PlaceCategories", "PlaceCategoryId", cascadeDelete: true);
            AddForeignKey("dbo.TouristAttractionPlaceCategories", "TouristAttraction_TouristAttractionId", "dbo.TouristAttractions", "TouristAttractionId", cascadeDelete: true);
        }
    }
}

namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBreedIdToPetAdoptionApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetAdoptionApplications", "BreedId", c => c.Int());
            CreateIndex("dbo.PetAdoptionApplications", "BreedId");
            AddForeignKey("dbo.PetAdoptionApplications", "BreedId", "dbo.Breeds", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetAdoptionApplications", "BreedId", "dbo.Breeds");
            DropIndex("dbo.PetAdoptionApplications", new[] { "BreedId" });
            DropColumn("dbo.PetAdoptionApplications", "BreedId");
        }
    }
}

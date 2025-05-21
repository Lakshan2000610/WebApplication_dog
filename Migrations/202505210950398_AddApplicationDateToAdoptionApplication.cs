namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationDateToAdoptionApplication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PetAdoptionApplications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PetAdoptionApplications", "BreedId", "dbo.Breeds");
            DropForeignKey("dbo.PetAdoptionApplications", "PetTypeId", "dbo.PetTypes");
            DropIndex("dbo.PetAdoptionApplications", new[] { "UserId" });
            DropIndex("dbo.PetAdoptionApplications", new[] { "PetTypeId" });
            DropIndex("dbo.PetAdoptionApplications", new[] { "BreedId" });
            AddColumn("dbo.AdoptionApplications", "ApplicationDate", c => c.DateTime(nullable: false));
            DropTable("dbo.PetAdoptionApplications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PetAdoptionApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PetName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        HasPets = c.String(nullable: false),
                        NumberOfPetsOwned = c.Int(nullable: false),
                        HasVeterinarian = c.String(nullable: false),
                        VetClinicName = c.String(),
                        VetPhoneNumber = c.String(),
                        Signature = c.String(),
                        ImagePath = c.String(),
                        ApplicationDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Status = c.Boolean(nullable: false),
                        PetTypeId = c.Int(),
                        BreedId = c.Int(),
                        Sex = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AdoptionApplications", "ApplicationDate");
            CreateIndex("dbo.PetAdoptionApplications", "BreedId");
            CreateIndex("dbo.PetAdoptionApplications", "PetTypeId");
            CreateIndex("dbo.PetAdoptionApplications", "UserId");
            AddForeignKey("dbo.PetAdoptionApplications", "PetTypeId", "dbo.PetTypes", "Id");
            AddForeignKey("dbo.PetAdoptionApplications", "BreedId", "dbo.Breeds", "Id");
            AddForeignKey("dbo.PetAdoptionApplications", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}

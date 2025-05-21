namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdoptionApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdoptionApplications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        PetId = c.Int(nullable: false),
                        PetName = c.String(nullable: false),
                        PetType = c.String(nullable: false),
                        Breed = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        HasOtherPet = c.Boolean(nullable: false),
                        DisciplineMethod = c.String(),
                        HasVeterinarian = c.Boolean(nullable: false),
                        ClinicName = c.String(),
                        ClinicPhoneNumber = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        Signature = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdoptionApplications");
        }
    }
}

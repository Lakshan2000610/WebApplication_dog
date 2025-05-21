namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPetAddTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetAdds",
                c => new
                    {
                        PetId = c.Int(nullable: false, identity: true),
                        PetName = c.String(nullable: false),
                        BreedId = c.Int(),
                        Age = c.Int(),
                        StreetAddress = c.String(nullable: false),
                        Sex = c.String(),
                        Vaccinations = c.String(),
                        PetTypeId = c.Int(),
                        PetImagePath = c.String(),
                    })
                .PrimaryKey(t => t.PetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PetAdds");
        }
    }
}

namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBreedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Breeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImagePath = c.String(),
                        PetTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PetTypes", t => t.PetTypeId, cascadeDelete: true)
                .Index(t => t.PetTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Breeds", "PetTypeId", "dbo.PetTypes");
            DropIndex("dbo.Breeds", new[] { "PetTypeId" });
            DropTable("dbo.Breeds");
        }
    }
}

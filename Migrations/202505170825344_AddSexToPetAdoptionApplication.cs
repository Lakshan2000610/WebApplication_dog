namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSexToPetAdoptionApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetAdoptionApplications", "Sex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PetAdoptionApplications", "Sex");
        }
    }
}

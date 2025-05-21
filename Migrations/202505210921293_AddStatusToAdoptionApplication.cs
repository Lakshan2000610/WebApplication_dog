namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToAdoptionApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdoptionApplications", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdoptionApplications", "Status");
        }
    }
}

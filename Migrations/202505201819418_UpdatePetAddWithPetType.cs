namespace WebApplication_dog.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePetAddWithPetType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PetAdds", "BreedId", c => c.Int(nullable: false));
            AlterColumn("dbo.PetAdds", "Age", c => c.Int(nullable: false));
            AlterColumn("dbo.PetAdds", "Sex", c => c.String(nullable: false));
            AlterColumn("dbo.PetAdds", "PetTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PetAdds", "PetTypeId", c => c.Int());
            AlterColumn("dbo.PetAdds", "Sex", c => c.String());
            AlterColumn("dbo.PetAdds", "Age", c => c.Int());
            AlterColumn("dbo.PetAdds", "BreedId", c => c.Int());
        }
    }
}

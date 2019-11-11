namespace PetRego.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPetFood : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "PetFood", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "PetFood");
        }
    }
}

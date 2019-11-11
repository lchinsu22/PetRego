namespace PetRego.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerId = c.Int(nullable: false, identity: true),
                        Ownername = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.OwnerId);
            
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        PetId = c.Int(nullable: false, identity: true),
                        PetName = c.String(nullable: false, maxLength: 50),
                        PetType = c.String(nullable: false, maxLength: 50),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PetId)
                .ForeignKey("dbo.Owners", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pets", "OwnerId", "dbo.Owners");
            DropIndex("dbo.Pets", new[] { "OwnerId" });
            DropTable("dbo.Pets");
            DropTable("dbo.Owners");
        }
    }
}

namespace Images.Collect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FullName = c.String(maxLength: 30),
                        BaseName = c.String(maxLength: 30),
                        Path = c.String(),
                        Extension = c.String(maxLength: 10),
                        Drive = c.String(maxLength: 3),
                        Hash = c.String(maxLength: 80),
                        Unreadable = c.Boolean(nullable: false),
                        FileSize = c.Long(nullable: false),
                        FileDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Images");
        }
    }
}

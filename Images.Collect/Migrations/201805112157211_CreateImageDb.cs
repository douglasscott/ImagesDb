namespace Images.Collect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImageDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        BaseName = c.String(),
                        Path = c.String(),
                        Extension = c.String(maxLength: 10),
                        Drive = c.String(maxLength: 3),
                        Hash = c.String(),
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

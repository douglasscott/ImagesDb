namespace Images.Collect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileNameLen : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "FullName", c => c.String(maxLength: 30));
        }
    }
}

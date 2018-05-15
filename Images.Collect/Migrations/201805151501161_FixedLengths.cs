namespace Images.Collect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "BaseName", c => c.String());
            AlterColumn("dbo.Images", "Hash", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "Hash", c => c.String(maxLength: 80));
            AlterColumn("dbo.Images", "BaseName", c => c.String(maxLength: 30));
        }
    }
}

namespace Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Price");
        }
    }
}

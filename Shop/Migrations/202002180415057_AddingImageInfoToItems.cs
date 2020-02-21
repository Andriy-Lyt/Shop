namespace Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingImageInfoToItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "PicExtension", c => c.String());
            AddColumn("dbo.Items", "HasPic", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Items", "PicExtension");
            DropColumn("dbo.Items", "HasPic");
        }
    }
}

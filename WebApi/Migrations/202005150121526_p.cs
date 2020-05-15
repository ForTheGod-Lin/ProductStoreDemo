namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class p : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menus", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropIndex("dbo.Menus", new[] { "ApplicationRole_Id" });
            DropColumn("dbo.Menus", "ApplicationRole_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "ApplicationRole_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Menus", "ApplicationRole_Id");
            AddForeignKey("dbo.Menus", "ApplicationRole_Id", "dbo.AspNetRoles", "Id");
        }
    }
}

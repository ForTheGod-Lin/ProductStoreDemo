namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Menus", "MenuGroupId");
            CreateIndex("dbo.RoleMenuGroups", "MenuGroupId");
            AddForeignKey("dbo.Menus", "MenuGroupId", "dbo.MenuGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleMenuGroups", "MenuGroupId", "dbo.MenuGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleMenuGroups", "MenuGroupId", "dbo.MenuGroups");
            DropForeignKey("dbo.Menus", "MenuGroupId", "dbo.MenuGroups");
            DropIndex("dbo.RoleMenuGroups", new[] { "MenuGroupId" });
            DropIndex("dbo.Menus", new[] { "MenuGroupId" });
            DropTable("dbo.MenuGroups");
        }
    }
}

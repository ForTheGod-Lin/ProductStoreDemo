namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RealName = c.String(),
                        Address = c.String(),
                        RegisterTime = c.DateTime(),
                        RegisterIP = c.String(),
                        LastLoginTime = c.DateTime(),
                        LastLoginIP = c.String(),
                        BirthDate = c.DateTime(),
                        Sex = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Icon = c.String(),
                        MenuGroupId = c.Int(nullable: false),
                        ApplicationRoleId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuGroups", t => t.MenuGroupId, cascadeDelete: true)
                .Index(t => t.MenuGroupId);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Href = c.String(),
                        Icon = c.String(),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.RoleMenuGroups",
                c => new
                    {
                        MenuGroupId = c.Int(nullable: false),
                        ApplicationRoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.MenuGroupId, t.ApplicationRoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRoleId, cascadeDelete: true)
                .ForeignKey("dbo.MenuGroups", t => t.MenuGroupId, cascadeDelete: true)
                .Index(t => t.MenuGroupId)
                .Index(t => t.ApplicationRoleId);
            
            CreateTable(
                "dbo.ApplicationRoleMenuItems",
                c => new
                    {
                        ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
                        MenuItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationRole_Id, t.MenuItem_Id })
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItem_Id, cascadeDelete: true)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.MenuItem_Id);
            
            CreateTable(
                "dbo.ApplicationRoleMenus",
                c => new
                    {
                        ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
                        Menu_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationRole_Id, t.Menu_Id })
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.Menus", t => t.Menu_Id, cascadeDelete: true)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.Menu_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleMenuGroups", "MenuGroupId", "dbo.MenuGroups");
            DropForeignKey("dbo.Menus", "MenuGroupId", "dbo.MenuGroups");
            DropForeignKey("dbo.MenuItems", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.ApplicationRoleMenus", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.ApplicationRoleMenus", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ApplicationRoleMenuItems", "MenuItem_Id", "dbo.MenuItems");
            DropForeignKey("dbo.ApplicationRoleMenuItems", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleMenuGroups", "ApplicationRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderDetails", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItems", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropIndex("dbo.ApplicationRoleMenus", new[] { "Menu_Id" });
            DropIndex("dbo.ApplicationRoleMenus", new[] { "ApplicationRole_Id" });
            DropIndex("dbo.ApplicationRoleMenuItems", new[] { "MenuItem_Id" });
            DropIndex("dbo.ApplicationRoleMenuItems", new[] { "ApplicationRole_Id" });
            DropIndex("dbo.RoleMenuGroups", new[] { "ApplicationRoleId" });
            DropIndex("dbo.RoleMenuGroups", new[] { "MenuGroupId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MenuItems", new[] { "MenuId" });
            DropIndex("dbo.Menus", new[] { "MenuGroupId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.OrderDetails", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CartItems", new[] { "ProductId" });
            DropIndex("dbo.CartItems", new[] { "UserId" });
            DropTable("dbo.ApplicationRoleMenus");
            DropTable("dbo.ApplicationRoleMenuItems");
            DropTable("dbo.RoleMenuGroups");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MenuItems");
            DropTable("dbo.Menus");
            DropTable("dbo.MenuGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Products");
            DropTable("dbo.CartItems");
        }
    }
}

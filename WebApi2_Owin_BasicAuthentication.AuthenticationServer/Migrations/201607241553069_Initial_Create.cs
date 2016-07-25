namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AppUser.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "AppUser.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("AppUser.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("AppUser.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "AppUser.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Level = c.Byte(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
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
                "AppUser.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("AppUser.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "AppUser.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("AppUser.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("AppUser.AspNetUserRoles", "UserId", "AppUser.AspNetUsers");
            DropForeignKey("AppUser.AspNetUserLogins", "UserId", "AppUser.AspNetUsers");
            DropForeignKey("AppUser.AspNetUserClaims", "UserId", "AppUser.AspNetUsers");
            DropForeignKey("AppUser.AspNetUserRoles", "RoleId", "AppUser.AspNetRoles");
            DropIndex("AppUser.AspNetUserLogins", new[] { "UserId" });
            DropIndex("AppUser.AspNetUserClaims", new[] { "UserId" });
            DropIndex("AppUser.AspNetUsers", "UserNameIndex");
            DropIndex("AppUser.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("AppUser.AspNetUserRoles", new[] { "UserId" });
            DropIndex("AppUser.AspNetRoles", "RoleNameIndex");
            DropTable("AppUser.AspNetUserLogins");
            DropTable("AppUser.AspNetUserClaims");
            DropTable("AppUser.AspNetUsers");
            DropTable("AppUser.AspNetUserRoles");
            DropTable("AppUser.AspNetRoles");
        }
    }
}

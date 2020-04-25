using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public enum Status
    {
        Normal,
        Forbidden
    }
    public enum Sex
    {
        Boy,
        Girl
    }
    public class ApplicationUser : IdentityUser {
        public ApplicationUser()
        {
            
        }
        [Display(Name ="真实姓名")]
        public string RealName { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "注册时间")]
        public Nullable<DateTime> RegisterTime { get; set; }
        [Display(Name = "注册IP")]
        public string RegisterIP { get; set; }
        [Display(Name = "上次登录时间")]
        public Nullable<DateTime> LastLoginTime { get; set; }
        [Display(Name = "上次登录IP")]
        public string LastLoginIP { get; set; }
        [Display(Name = "出生日期")]
        public Nullable<DateTime> BirthDate { get; set; }
        [Display(Name = "性别")]
        public Sex Sex { get; set; }
        [Display(Name ="账号状态")]
        public Status Status { get; set; }
        [NotMapped]
        public string StatusString
        {
            get { return Status.ToString(); }
        }
       
     
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("ProductStore", throwIfV1Schema: false) {
        }

        static ApplicationDbContext() {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }
}
 namespace Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity;

    public sealed class Configuration : DbMigrationsConfiguration<BlogDbContext>
    {
        private DbContext context;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Blog.Models.BlogDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole("Admin", context);
                this.CreateRole("User", context);
            }
            if (!context.Users.Any())
            {
                this.CreateUser("admin@admin.com", "Admin", "123", context);
                this.SetUserRole("admin@admin.com", "Admin", context);

                this.CreateUser("gosho@admin.com", "Gosho", "123", context);
                this.SetUserRole("gosho@admin.com", "User", context);
            }
        }

        private void CreateRole(string roleName, BlogDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var result = roleManager.Create(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void SetUserRole(string email, string role, BlogDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var userId = context.Users.FirstOrDefault(u => u.Email.Equals(email)).Id;
            var result = userManager.AddToRole(userId , role);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(string email, string fullName, string pass, BlogDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };
            var user = new ApplicationUser()
            {
                Email = email,
                FullName = fullName,
                UserName = email,
            };

          var result =  userManager.Create(user , pass);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

       }
    }

        

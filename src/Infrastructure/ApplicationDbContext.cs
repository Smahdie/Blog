using Core.Models;
using Core.Models.Manager;
using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<Manager, Role, string, ManagerClaim, ManagerRole, ManagerLogin, RoleClaim, ManagerToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Content2Category> Content2Categories { get; set; }
        public DbSet<Content2Tag> Content2Tags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuMember> MenuMembers { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            CreateDefaultData(builder);

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CategoryTranslationConfiguration());
            builder.ApplyConfiguration(new Content2CategoryConfiguration());
            builder.ApplyConfiguration(new Content2TagConfiguration());
            builder.ApplyConfiguration(new ContentConfiguration());
            builder.ApplyConfiguration(new LanguageConfiguration());
            builder.ApplyConfiguration(new MenuConfiguration());
            builder.ApplyConfiguration(new MenuMemberConfiguration());
            builder.ApplyConfiguration(new PageConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new SliderConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
        }

        private static void CreateDefaultData(ModelBuilder builder)
        {
            var date = DateTime.Parse("2021-01-04");
            var hasher = new PasswordHasher<Manager>();
            builder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "Email@email.com",
                    NormalizedUserName = "email@email.com",
                    Email = "Email@email.com",
                    NormalizedEmail = "email@email.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEMdDRzlE89HWOsK6MSH6m7kWtn61fxl+A9wBp4s7Zps93fmMP5KdlXZbIgnqWoXv7g==",//Pa$$w0rd
                    ConcurrencyStamp = "058d1d6d-0797-4f64-a05f-ff939153f492",
                    SecurityStamp = "VAPZU2UUTN3YJWCM2F3FWOHEBYB6MQJR",
                    CreatedOn = date
                }
            );

            builder.Entity<Language>().HasData(new Language {
                Id= 1,
                Code = "fa",
                Name = "فارسی",
                IsActive = true,
                IsDefault = true,
                CreatedOn = date
            });


            builder.Entity<Menu>().HasData(new Menu
            {
                Id = 1,
                Keyword = "header",
                Title = "هدر",
                Language = "fa",                
                CreatedOn = date
            }, new Menu {
                Id = 2,
                Keyword = "useful_link",
                Title = "لینک های مفید	",
                Language = "fa",
                CreatedOn = date
            }, new Menu
            {
                Id = 3,
                Keyword = "our_services",
                Title = "خدمات ما",
                Language = "fa",
                CreatedOn = date
            }, new Menu
            {
                Id = 4,
                Keyword = "featured",
                Title = "زیر اسلایدر	",
                Language = "fa",
                CreatedOn = date
            });
        }
    }
}

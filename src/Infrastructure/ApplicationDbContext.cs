using Core.Models;
using Core.Models.Manager;
using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zuhid.Base;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<User, Role, Guid,
    UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        const string schema = "identity";
        builder.ToSnakeCase(schema);
        builder.Entity<User>(entity => entity.ToTable(name: "users", schema));
        builder.Entity<Role>(entity => entity.ToTable(name: "role", schema));
        builder.Entity<UserRole>(entity => entity.ToTable("user_role", schema));
        builder.Entity<UserClaim>(entity => entity.ToTable("user_claim", schema));
        builder.Entity<UserLogin>(entity => entity.ToTable("user_login", schema));
        builder.Entity<RoleClaim>(entity => entity.ToTable("role_claim", schema));
        builder.Entity<UserToken>(entity => entity.ToTable("user_token", schema));

        builder.LoadCsvData<User>();
        builder.LoadCsvData<Role>();
        builder.LoadCsvData<UserRole>();
        builder.LoadCsvData<UserClaim>();
        builder.LoadCsvData<UserLogin>();
        builder.LoadCsvData<RoleClaim>();
        builder.LoadCsvData<UserToken>();
    }
}


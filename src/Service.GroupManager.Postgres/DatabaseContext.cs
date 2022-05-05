using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;
using Service.GroupManager.Domain.Models;

namespace Service.GroupManager.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "groups";

        public const string ProfilesTableName = "profiles";
        public const string GroupsTableName = "groups";

        public DbSet<ClientGroupsProfile> ClientProfiles { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<ClientGroupsProfile>().ToTable(ProfilesTableName);
            modelBuilder.Entity<ClientGroupsProfile>().HasKey(e => e.ClientId);
            modelBuilder.Entity<ClientGroupsProfile>().HasIndex(e => e.GroupId);

            modelBuilder.Entity<Group>().ToTable(GroupsTableName);
            modelBuilder.Entity<Group>().HasKey(e => e.GroupId);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> UpsertAsync(IEnumerable<ClientGroupsProfile> entities)
        {
            var result = await ClientProfiles.UpsertRange(entities).AllowIdentityMatch().RunAsync();
            return result;
        }
        
        public async Task<int> UpsertAsync(IEnumerable<Group> entities)
        {
            var result = await Groups.UpsertRange(entities).AllowIdentityMatch().RunAsync();
            return result;
        }


        
    }
}

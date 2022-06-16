using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyNoSqlServer.Abstractions;
using Service.Fees.Domain.Models;
using Service.GroupManager.Domain.Models;
using Service.GroupManager.Domain.Models.NoSql;
using Service.GroupManager.Postgres;
using Service.Liquidity.ConverterMarkups.Domain.Models;

namespace Service.GroupManager.Helpers
{
    public class GroupsRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IMyNoSqlServerDataWriter<ClientGroupsProfileNoSqlEntity> _profileWriter;
        private readonly IMyNoSqlServerDataWriter<GroupNoSqlEntity> _groupWriter;

        public GroupsRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, IMyNoSqlServerDataWriter<GroupNoSqlEntity> groupWriter, IMyNoSqlServerDataWriter<ClientGroupsProfileNoSqlEntity> profileWriter)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _groupWriter = groupWriter;
            _profileWriter = profileWriter;
        }

        public async Task UpsertProfile(IEnumerable<ClientGroupsProfile> profiles)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await context.UpsertAsync(profiles);
            await _profileWriter.BulkInsertOrReplaceAsync(profiles.Select(ClientGroupsProfileNoSqlEntity.Create).ToList());
            await _profileWriter.CleanAndKeepMaxRecords(ClientGroupsProfileNoSqlEntity.GeneratePartitionKey(),
                Program.Settings.MaxCachedEntities);
        }
        
        
        public async Task UpsertGroups(IEnumerable<Group> groups)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await context.UpsertAsync(groups);
            await RefreshGroupCache();
        }
        
        public async Task<List<Group>> GetGroups()
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var group = await context.Groups.ToListAsync();
            if (!group.Any())
                group.Add(await CreateDefaultGroup());
            
            await RefreshGroupCache();
            return group;
        }
        public async Task<Group> GetFirstGroup()
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var group = await context.Groups.FirstOrDefaultAsync() ?? await CreateDefaultGroup();
            return group;
        }

        public async Task<Group> GetGroupById(string groupId)
        {
            var entity = await _groupWriter.GetAsync(GroupNoSqlEntity.GeneratePartitionKey(),
                GroupNoSqlEntity.GenerateRowKey(groupId));
            if (entity != null)
                return entity.Group;
            
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var group = await context.Groups.FirstOrDefaultAsync(t => t.GroupId == groupId);
            if (group != null)
                await RefreshGroupCache();

            return group;
        }
        
        public async Task RemoveGroupById(string groupId)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var group = await context.Groups.FirstOrDefaultAsync(t => t.GroupId == groupId);
            if (group != null)
            {
                context.Groups.Remove(group);
                await context.SaveChangesAsync();
                await RefreshGroupCache();
            }
        }

        private async Task RefreshGroupCache()
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var groups = await context.Groups.ToListAsync();
            await _groupWriter.CleanAndBulkInsertAsync(groups.Select(GroupNoSqlEntity.Create).ToList());
        }

        public async Task<ClientGroupsProfile> GetProfile(string clientId)
        {
            var entity = await _profileWriter.GetAsync(ClientGroupsProfileNoSqlEntity.GeneratePartitionKey(),
                ClientGroupsProfileNoSqlEntity.GenerateRowKey(clientId));
            if (entity != null)
                return entity.ClientProfile;
            
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var profile = await context.ClientProfiles.FirstOrDefaultAsync(t => t.ClientId == clientId);
            
            if (profile != null)
            {
                await _profileWriter.InsertOrReplaceAsync(ClientGroupsProfileNoSqlEntity.Create(profile));
                await _profileWriter.CleanAndKeepMaxRecords(ClientGroupsProfileNoSqlEntity.GeneratePartitionKey(),
                    Program.Settings.MaxCachedEntities);
            }

            return profile;
        }
        
        public async Task<List<ClientGroupsProfile>> GetProfilesByGroup(string groupId, int skip, int take, string searchText)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var query = context.ClientProfiles.Where(t => t.GroupId == groupId);
            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(t => t.ClientEmail.Contains(searchText) || t.ClientId.Contains(searchText));
            query = query.Skip(skip).Take(take);
            return await query.ToListAsync();
        }

        private async Task<Group> CreateDefaultGroup()
        {
            var group = new Group
            {
                GroupId = "DEFAULT",
                WithdrawalProfileId = FeeProfileConsts.DefaultProfile,
                ConverterProfileId = MarkupProfileConsts.DefaultProfile,
                InterestRateProfileId = "DEFAULT",
                DepositProfileId = FeeProfileConsts.DefaultProfile
            };
            await UpsertGroups(new[] {group});
            return group;
        }
    }
}
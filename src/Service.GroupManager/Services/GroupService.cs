using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.Fees.MyNoSql;
using Service.GroupManager.Domain.Models;
using Service.GroupManager.Grpc;
using Service.GroupManager.Grpc.Models;
using Service.GroupManager.Helpers;
using Service.GroupManager.Postgres;
using Service.Liquidity.ConverterMarkups.Domain.Models;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;

namespace Service.GroupManager.Services
{
    public class GroupService
    {
        private readonly ILogger<GroupService> _logger;
        private readonly GroupsRepository _repository;
        private readonly IPersonalDataServiceGrpc _personalData;
        private readonly IMyNoSqlServerDataReader<FeeProfilesNoSqlEntity> _feesReader;
        private readonly IMyNoSqlServerDataReader<MarkupProfilesNoSqlEntity> _markupReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public GroupService(GroupsRepository repository, ILogger<GroupService> logger, IPersonalDataServiceGrpc personalData, IMyNoSqlServerDataReader<FeeProfilesNoSqlEntity> feesReader, IMyNoSqlServerDataReader<MarkupProfilesNoSqlEntity> markupReader, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _repository = repository;
            _logger = logger;
            _personalData = personalData;
            _feesReader = feesReader;
            _markupReader = markupReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<ClientGroupsProfile> GetOrCreateClientGroupProfile(string clientId)
        {
            try
            {
                var profile = await _repository.GetProfile(clientId);
                if (profile != null)
                    return profile;

                var group = await _repository.GetFirstGroup();
                var pd = await _personalData.GetByIdAsync(new GetByIdRequest()
                {
                    Id = clientId
                });
                profile = new ClientGroupsProfile()
                {
                    ClientId = clientId,
                    GroupId = group.GroupId,
                    ClientEmail = pd.PersonalData?.Email ?? String.Empty
                };
                
                await _repository.UpsertProfile(new[] {profile});
                return profile;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetUsersResponse> GetUsersByGroup(GetUsersRequest request)
        {
            try
            {
                if (request.Take == 0)
                    request.Take = 20;
                
                var users = await _repository.GetProfilesByGroup(request.GroupId, request.Skip, request.Take, request.SearchText);
                return new GetUsersResponse()
                {
                    Profiles = users
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<OperationResponse> AssignUsersToGroup(AssignUsersToGroupRequest request)
        {
            try
            {
                var group = await _repository.GetGroupById(request.GroupId);
                if (group == null)
                    return new OperationResponse("GroupNotFound");
                
                var profiles = new List<ClientGroupsProfile>();
                foreach (var clientId in request.ClientIds)
                {
                    var profile = await GetOrCreateClientGroupProfile(clientId);
                    profile.GroupId = request.GroupId;
                    profiles.Add(profile);
                }

                await _repository.UpsertProfile(profiles);
                return new OperationResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GroupsResponse> GetGroups()
        {
            try
            {
                var response = new List<GroupResponse>();
                var groups = await _repository.GetGroups();
                await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

                foreach (var group in groups)
                {
                    response.Add(new GroupResponse()
                    {
                        Group = group,
                        UserCount = await context.ClientProfiles.Where(t=>t.GroupId == group.GroupId).CountAsync()
                    });
                }
                return new GroupsResponse()
                {
                    Groups = response
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<OperationResponse> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request)
        {
            try
            {
                var group = await _repository.GetGroupById(request.GroupId) ?? new Group()
                {
                    GroupId = request.GroupId
                };

                if (string.IsNullOrWhiteSpace(request.ConverterGroupId) ||
                    string.IsNullOrWhiteSpace(request.WithdrawalGroupId) ||
                    string.IsNullOrWhiteSpace(request.InterestRateGroupId))
                {
                    return new OperationResponse("ProfilesIds are required");
                }

                group.ConverterProfileId = request.ConverterGroupId;
                group.WithdrawalProfileId = request.WithdrawalGroupId;
                group.InterestRateProfileId = request.InterestRateGroupId;

                await _repository.UpsertGroups(new[] {group});
                return new OperationResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<OperationResponse> DeleteGroup(DeleteGroupRequest request)
        {
            try
            {
                var users = await _repository.GetProfilesByGroup(request.GroupId, 0, 100, null);
                if (users.Any())
                    return new OperationResponse("Group is not empty");
                
                await _repository.RemoveGroupById(request.GroupId);
                return new OperationResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ProfilesListResponse> GetAvailableProfiles()
        {
            var fees = _feesReader.Get(FeeProfilesNoSqlEntity.GeneratePartitionKey(), FeeProfilesNoSqlEntity.GenerateRowKey());
            var markups = _markupReader.Get(MarkupProfilesNoSqlEntity.GeneratePartitionKey(), MarkupProfilesNoSqlEntity.GenerateRowKey());
            return new ProfilesListResponse
            {
                ConverterProfiles = markups?.Profiles ?? new List<string>(),
                WithdrawalProfiles = fees?.Profiles ?? new List<string>(),
                InterestProfiles = new List<string>() {"NOT_IMPLEMENTED_YET"}
            };
        }

        public async Task<ProfileWithGroupResponse> GetOrCreateClientProfileWithGroup(string requestClientId)
        {
            var profile = await GetOrCreateClientGroupProfile(requestClientId);
            var group = await _repository.GetGroupById(profile.GroupId);
            return new ProfileWithGroupResponse()
            {
                Profile = profile,
                Group = group
            };
        }
    }
}
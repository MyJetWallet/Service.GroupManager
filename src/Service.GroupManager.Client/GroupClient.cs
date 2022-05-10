using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using Service.GroupManager.Domain.Models;
using Service.GroupManager.Domain.Models.NoSql;
using Service.GroupManager.Grpc;
using Service.GroupManager.Grpc.Models;

namespace Service.GroupManager.Client;

public class GroupClient : IGroupsService
{
    private readonly IGroupsService _grpcService;
    private readonly IMyNoSqlServerDataReader<GroupNoSqlEntity> _groupsReader;
    private readonly IMyNoSqlServerDataReader<ClientGroupsProfileNoSqlEntity> _profileReader;

    public GroupClient(IGroupsService grpcService, IMyNoSqlServerDataReader<GroupNoSqlEntity> groupsReader, IMyNoSqlServerDataReader<ClientGroupsProfileNoSqlEntity> profileReader)
    {
        _grpcService = grpcService;
        _groupsReader = groupsReader;
        _profileReader = profileReader;
    }

    public async Task<ClientGroupsProfile> GetOrCreateClientGroupProfile(GetOrClientProfileRequest request)
    {
        var entity = _profileReader.Get(ClientGroupsProfileNoSqlEntity.GeneratePartitionKey(),
            ClientGroupsProfileNoSqlEntity.GenerateRowKey(request.ClientId));
        if (entity != null)
            return entity.ClientProfile;

        return await _grpcService.GetOrCreateClientGroupProfile(request);
    }

    public async Task<GetUsersResponse> GetUsersByGroup(GetUsersRequest request) => await _grpcService.GetUsersByGroup(request);
    public async Task<OperationResponse> AssignUsersToGroup(AssignUsersToGroupRequest request)=> await _grpcService.AssignUsersToGroup(request);
    public async Task<GroupsResponse> GetGroups()=> await _grpcService.GetGroups();
    public async Task<OperationResponse> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request)=> await _grpcService.CreateOrUpdateGroup(request);
    public async Task<OperationResponse> DeleteGroup(DeleteGroupRequest request)=> await _grpcService.DeleteGroup(request);
    public async Task<ProfilesListResponse> GetAvailableProfiles()=> await _grpcService.GetAvailableProfiles();

    public async Task<ProfileWithGroupResponse> GetOrCreateClientProfileWithGroup(GetOrClientProfileRequest request)
    {
        var entity = _profileReader.Get(ClientGroupsProfileNoSqlEntity.GeneratePartitionKey(),
            ClientGroupsProfileNoSqlEntity.GenerateRowKey(request.ClientId));
        
        if (entity != null)
        {
            var profile = entity.ClientProfile;
            var group = _groupsReader.Get(GroupNoSqlEntity.GeneratePartitionKey(),
                GroupNoSqlEntity.GenerateRowKey(profile.GroupId));

            if (group != null)
                return new ProfileWithGroupResponse
                {
                    Profile = profile,
                    Group = group.Group
                };
        }

        return await _grpcService.GetOrCreateClientProfileWithGroup(request);
    }
}
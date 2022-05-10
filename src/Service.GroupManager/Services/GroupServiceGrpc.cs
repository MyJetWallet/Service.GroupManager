using System;
using System.Threading.Tasks;
using Service.GroupManager.Domain.Models;
using Service.GroupManager.Grpc;
using Service.GroupManager.Grpc.Models;
using Service.GroupManager.Settings;

namespace Service.GroupManager.Services
{
    public class GroupServiceGrpc: IGroupsService
    {
        private readonly GroupService _groupService;
        
        public GroupServiceGrpc(GroupService groupService)
        {
            _groupService = groupService;
        }

        public async Task<ClientGroupsProfile> GetOrCreateClientGroupProfile(GetOrClientProfileRequest request) => await _groupService.GetOrCreateClientGroupProfile(request.ClientId);

        public async Task<ProfileWithGroupResponse> GetOrCreateClientProfileWithGroup(GetOrClientProfileRequest request) => await _groupService.GetOrCreateClientProfileWithGroup(request.ClientId);

        public async Task<GetUsersResponse> GetUsersByGroup(GetUsersRequest request) => await _groupService.GetUsersByGroup(request);

        public async Task<OperationResponse> AssignUsersToGroup(AssignUsersToGroupRequest request) => await _groupService.AssignUsersToGroup(request);

        public async Task<GroupsResponse> GetGroups() => await _groupService.GetGroups();

        public async Task<OperationResponse> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request) => await _groupService.CreateOrUpdateGroup(request);

        public async Task<OperationResponse> DeleteGroup(DeleteGroupRequest request) => await _groupService.DeleteGroup(request);
        public async Task<ProfilesListResponse> GetAvailableProfiles() => await _groupService.GetAvailableProfiles();
    }
}

using System.ServiceModel;
using System.Threading.Tasks;
using Service.GroupManager.Domain.Models;
using Service.GroupManager.Grpc.Models;

namespace Service.GroupManager.Grpc
{
    [ServiceContract]
    public interface IGroupsService
    {
        [OperationContract]
        Task<ClientGroupsProfile> GetOrCreateClientGroupProfile(GetOrClientProfileRequest request);
        
        [OperationContract]
        Task<GetUsersResponse> GetUsersByGroup(GetUsersRequest request);
        
        [OperationContract]
        Task<OperationResponse> AssignUsersToGroup(AssignUsersToGroupRequest request);
        
        [OperationContract]
        Task<GroupsResponse> GetGroups();
        
        [OperationContract]
        Task<OperationResponse> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request);
        
        [OperationContract]
        Task<OperationResponse> DeleteGroup(DeleteGroupRequest request);
        
        [OperationContract]
        Task<ProfilesListResponse> GetAvailableProfiles();

        [OperationContract]
        Task<ProfileWithGroupResponse> GetOrCreateClientProfileWithGroup(GetOrClientProfileRequest request);
    }
}
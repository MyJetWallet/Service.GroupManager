using System.ServiceModel;
using System.Threading.Tasks;
using Service.GroupManager.Grpc.Models;

namespace Service.GroupManager.Grpc;

[ServiceContract]
public interface IGroupSubmitService
{
    [OperationContract]
    public Task SubmitGroups(SendGroupsRequest request);
}
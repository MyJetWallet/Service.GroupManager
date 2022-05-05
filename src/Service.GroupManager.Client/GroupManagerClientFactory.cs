using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.GroupManager.Grpc;

namespace Service.GroupManager.Client
{
    [UsedImplicitly]
    public class GroupManagerClientFactory: MyGrpcClientFactory
    {
        public GroupManagerClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IGroupsService GetHelloService() => CreateGrpcService<IGroupsService>();
    }
}

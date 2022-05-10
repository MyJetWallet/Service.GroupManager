using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using MyNoSqlServer.DataReader;
using Service.GroupManager.Domain.Models.NoSql;
using Service.GroupManager.Grpc;

namespace Service.GroupManager.Client
{
    [UsedImplicitly]
    public class GroupManagerClientFactory : MyGrpcClientFactory
    {
        private readonly MyNoSqlReadRepository<ClientGroupsProfileNoSqlEntity> _clientReader;
        private readonly MyNoSqlReadRepository<GroupNoSqlEntity> _groupReader;

        public GroupManagerClientFactory(string grpcServiceUrl,
            MyNoSqlReadRepository<ClientGroupsProfileNoSqlEntity> clientReader,
            MyNoSqlReadRepository<GroupNoSqlEntity> groupReader) : base(grpcServiceUrl)
        {
            _clientReader = clientReader;
            _groupReader = groupReader;
        }

        public IGroupsService GetGroupService()
        {
            if(_clientReader==null || _groupReader==null)
                return CreateGrpcService<IGroupsService>();

            return new GroupClient(CreateGrpcService<IGroupsService>(), _groupReader, _clientReader);
        }
    }
}

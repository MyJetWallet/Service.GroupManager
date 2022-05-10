using Autofac;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using Service.GroupManager.Domain.Models.NoSql;
using Service.GroupManager.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.GroupManager.Client
{
    public static class AutofacHelper
    {
        public static void RegisterGroupManagerClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new GroupManagerClientFactory(grpcServiceUrl, null, null);

            builder.RegisterInstance(factory.GetGroupService()).As<IGroupsService>().SingleInstance();
        }
        
        public static void RegisterGroupManagerClientCached(this ContainerBuilder builder, string grpcServiceUrl, IMyNoSqlSubscriber myNoSqlSubscriber)
        {
            var groupSubs = new MyNoSqlReadRepository<GroupNoSqlEntity>(myNoSqlSubscriber, GroupNoSqlEntity.TableName);
            var clientSubs = new MyNoSqlReadRepository<ClientGroupsProfileNoSqlEntity>(myNoSqlSubscriber, ClientGroupsProfileNoSqlEntity.TableName);

            var factory = new GroupManagerClientFactory(grpcServiceUrl, clientSubs, groupSubs);

            builder
                .RegisterInstance(groupSubs)
                .As<IMyNoSqlServerDataReader<GroupNoSqlEntity>>()
                .SingleInstance();
            
            builder
                .RegisterInstance(clientSubs)
                .As<IMyNoSqlServerDataReader<ClientGroupsProfileNoSqlEntity>>()
                .SingleInstance();
            
            builder.RegisterInstance(factory.GetGroupService()).As<IGroupsService>().SingleInstance();
        }
    }
}

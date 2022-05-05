using Autofac;
using Service.GroupManager.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.GroupManager.Client
{
    public static class AutofacHelper
    {
        public static void RegisterGroupManagerClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new GroupManagerClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IGroupsService>().SingleInstance();
        }
    }
}

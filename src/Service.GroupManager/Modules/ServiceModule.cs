using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.Fees.MyNoSql;
using Service.GroupManager.Domain.Models.NoSql;
using Service.GroupManager.Helpers;
using Service.GroupManager.Services;
using Service.Liquidity.ConverterMarkups.Domain.Models;
using Service.PersonalData.Client;
using Service.AssetSettings.Client;
using Service.AssetSettings.MyNoSql;

namespace Service.GroupManager.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterPersonalDataClient(Program.Settings.PersonalDataGrpcServiceUrl);
            var myNoSqlClient = builder.CreateNoSqlClient(Program.Settings.MyNoSqlReaderHostPort, Program.LogFactory);

            builder.RegisterMyNoSqlReader<FeeProfilesNoSqlEntity>(myNoSqlClient, FeeProfilesNoSqlEntity.TableName);
            builder.RegisterMyNoSqlReader<MarkupProfilesNoSqlEntity>(myNoSqlClient, MarkupProfilesNoSqlEntity.TableName);
            builder.RegisterMyNoSqlReader<ProfilesNoSqlEntity>(myNoSqlClient, ProfilesNoSqlEntity.TableName);
            builder.RegisterMyNoSqlReader<AssetSettingsNoSqlEntity>(myNoSqlClient, AssetSettingsNoSqlEntity.TableName);


            builder.RegisterMyNoSqlWriter<GroupNoSqlEntity>(() => Program.Settings.MyNoSqlWriterUrl,
                GroupNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<ClientGroupsProfileNoSqlEntity>(() => Program.Settings.MyNoSqlWriterUrl,
                ClientGroupsProfileNoSqlEntity.TableName);

            builder.RegisterType<GroupsRepository>().AsSelf().SingleInstance().AutoActivate();
            builder.RegisterType<GroupService>().AsSelf().SingleInstance().AutoActivate();
        }
    }
}
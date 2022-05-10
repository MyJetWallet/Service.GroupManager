using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.GroupManager.Settings
{
    public class SettingsModel
    {
        [YamlProperty("GroupManager.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("GroupManager.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("GroupManager.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("GroupManager.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }
        
        [YamlProperty("GroupManager.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }
        
        [YamlProperty("GroupManager.SpotServiceBusHostPort")]
        public string SpotServiceBusHostPort { get; set; }
        
        [YamlProperty("GroupManager.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }
        
        [YamlProperty("GroupManager.MaxCachedEntities")]
        public int MaxCachedEntities { get; set; }

        [YamlProperty("GroupManager.PersonalDataGrpcServiceUrl")]
        public string PersonalDataGrpcServiceUrl { get; set; }
    }
}

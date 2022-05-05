using MyJetWallet.Sdk.Postgres;
using Service.GroupManager.Postgres;

namespace Service.ClientProfile.Postgres.DesignTime
{
    public class ContextFactory : MyDesignTimeContextFactory<DatabaseContext>
    {
        public ContextFactory() : base(options => new DatabaseContext(options))
        {

        }
    }
}
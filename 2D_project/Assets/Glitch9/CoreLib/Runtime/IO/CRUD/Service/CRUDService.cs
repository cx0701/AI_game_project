
namespace Glitch9.IO.RESTApi
{
    public abstract class CRUDService<TClient> where TClient : CRUDClient<TClient>
    {
        public TClient Client { get; }
        public bool IsBeta { get; }
        public RESTHeader[] BetaHeaders { get; private set; }

        protected CRUDService(TClient client, params RESTHeader[] betaHeaders)
        {
            Client = client;
            BetaHeaders = betaHeaders;
            IsBeta = !betaHeaders.IsNullOrEmpty();
        }

        protected CRUDService(TClient client, bool isBeta)
        {
            Client = client;
            IsBeta = isBeta;
        }
    }
}
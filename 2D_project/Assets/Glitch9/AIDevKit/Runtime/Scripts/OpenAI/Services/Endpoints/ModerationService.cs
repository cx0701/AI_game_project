using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Moderation Service: https://platform.openai.com/docs/api-reference/moderations
    /// </summary>
    public class ModerationService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/moderations";
        public ModerationService(OpenAI client) : base(client) { }

        /// <summary>
        /// Classifies if text is potentially harmful.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async UniTask<Moderation> Create(ModerationRequest req)
        {
            req.Model ??= OpenAISettings.DefaultMOD;
            return await OpenAI.CRUD.CreateAsync<ModerationRequest, Moderation>(kEndpoint, this, req);
        }
    }
}
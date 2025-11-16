using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Thread object that assistants can interact with.
    /// <para>Renamed from ThreadObject to Thread (2024.06.14)</para>
    /// </summary>
    public class Thread : ModelResponse { }
    public static class ThreadExtensions
    {
        public static async UniTask<ThreadMessage[]> GetMessagesAsync(this Thread thread, int limit = 20)
        {
            if (thread == null || string.IsNullOrEmpty(thread.Id))
            {
                OpenAI.DefaultInstance.Logger.Error("Thread or Thread ID is null or empty.");
                return null;
            }

            QueryResponse<ThreadMessage> queryResponse = await OpenAI.DefaultInstance.Beta.Threads.Messages.List(thread.Id, limit);
            return queryResponse?.Data;
        }
    }
}
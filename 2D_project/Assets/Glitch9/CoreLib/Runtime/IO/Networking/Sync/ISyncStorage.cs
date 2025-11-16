using Cysharp.Threading.Tasks;

namespace Glitch9.IO.Networking
{
    public interface ISyncStorage
    {
        UniTask<object> GetDataAsync(string fieldName);
        void SetDataAsync(string fieldName, object value);
    }
}
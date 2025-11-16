using Cysharp.Threading.Tasks;
using System;

namespace Glitch9.IO.RESTApi
{
    public interface IObjectProvider<T>
    {
        EventHandler<T> OnCreate { get; set; }
        EventHandler<T> OnRetrieve { get; set; }
        EventHandler<T[]> OnList { get; set; }
        EventHandler<bool> OnDelete { get; set; }

        UniTask<IResult> CreateAsync(params object[] args);
        UniTask<IResult> RetrieveAsync(string id, params object[] args);
        UniTask<IResult> RetrieveOrCreateAsync(string id, params object[] args);
        UniTask<IResult> ListAsync(params object[] args);
        UniTask<IResult> DeleteAsync(string id, params object[] args);
    }
}
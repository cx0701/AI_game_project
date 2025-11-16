using Cysharp.Threading.Tasks;
using System;

namespace Glitch9.IO.RESTApi
{
    public abstract class ObjectProvider<T> : IObjectProvider<T>
    {
        public EventHandler<T> OnCreate { get; set; }
        public EventHandler<T> OnRetrieve { get; set; }
        public EventHandler<T> OnUpdate { get; set; }
        public EventHandler<T[]> OnList { get; set; }
        public EventHandler<bool> OnDelete { get; set; }

        private readonly string _objectName;
        private readonly ILogger _logger;

        protected ObjectProvider(ILogger logger)
        {
            _objectName = typeof(T).Name;
            _logger = logger;
        }

        public async UniTask<IResult> CreateAsync(params object[] args)
        {
            try
            {
                T obj = await CreateInternalAsync(args);
                if (obj == null) return Result.Fail($"{_objectName} creation failed.");

                OnCreate?.Invoke(this, obj);
                return Result<T>.Success(obj);
            }
            catch (Exception e)
            {
                return Result.Fail($"{_objectName} creation failed. {e.Message}");
            }
        }

        public async UniTask<IResult> RetrieveAsync(string id, params object[] args)
        {
            try
            {
                T obj = await RetrieveInternalAsync(id, args);
                if (obj == null) return Result.Fail($"{_objectName}({id}) not found.");
                OnRetrieve?.Invoke(this, obj);
                return Result<T>.Success(obj);
            }
            catch (Exception e)
            {
                return Result.Fail($"{_objectName} retrieval failed. {e.Message}");
            }
        }

        public async UniTask<IResult> RetrieveOrCreateAsync(string id, params object[] args)
        {
            IResult result = await RetrieveAsync(id, args);
            if (result.IsSuccess) return result;
            _logger?.Warning($"{_objectName}({id}) retrieval failed. Creating new {_objectName}.");
            await UniTask.Delay(RESTApiV3.Config.kMinOperationDelayInMillis);
            return await CreateAsync(args);
        }

        public async UniTask<IResult> UpdateAsync(string id, params object[] args)
        {
            try
            {
                T obj = await UpdateInternalAsync(id, args);
                if (obj == null) return Result.Fail($"{_objectName}({id}) update failed.");

                OnUpdate?.Invoke(this, obj);
                return Result<T>.Success(obj);
            }
            catch (Exception e)
            {
                return Result.Fail($"{_objectName}({id}) update failed. {e.Message}");
            }
        }

        public async UniTask<IResult> ListAsync(params object[] args)
        {
            try
            {
                T[] objs = await ListInternalAsync(args);
                if (objs == null || objs.Length == 0) return Result.Fail($"{_objectName} list is empty.");
                OnList?.Invoke(this, objs);
                return Result<T[]>.Success(objs);
            }
            catch (Exception e)
            {
                return Result.Fail($"{_objectName} list retrieval failed. {e.Message}");
            }
        }

        public async UniTask<IResult> DeleteAsync(string id, params object[] args)
        {
            try
            {
                bool deleted = await DeleteInternalAsync(id, args);
                if (!deleted)
                {
                    OnDelete?.Invoke(this, false);
                    return Result.Fail($"{_objectName}({id}) deletion failed.");
                }

                OnDelete?.Invoke(this, true);
                return Result<T>.Success(null);
            }
            catch (Exception e)
            {
                return Result.Fail($"{_objectName}({id}) deletion failed. {e.Message}");
            }
        }

        protected abstract UniTask<T> CreateInternalAsync(params object[] args);
        protected abstract UniTask<T> RetrieveInternalAsync(string id, params object[] args);
        protected abstract UniTask<T> UpdateInternalAsync(string id, params object[] args);
        protected abstract UniTask<T[]> ListInternalAsync(params object[] args);
        protected abstract UniTask<bool> DeleteInternalAsync(string id, params object[] args);
    }
}
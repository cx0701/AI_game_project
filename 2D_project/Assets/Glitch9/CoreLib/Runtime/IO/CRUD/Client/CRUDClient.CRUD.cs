using System;
using Cysharp.Threading.Tasks;

namespace Glitch9.IO.RESTApi
{
    public abstract partial class CRUDClient<TSelf>
        where TSelf : CRUDClient<TSelf>
    {
        public class CRUD
        {
            public static async UniTask<TResBody> CreateAsync<TReqBody, TResBody>(string url, CRUDService<TSelf> service, TReqBody reqBody, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest<TReqBody> req = new(url, reqBody);
                    (RESTRequest<TReqBody> req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.CreateAsync<TReqBody, TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<RESTResponse> CreateAsync<TReqBody>(string url, CRUDService<TSelf> service, TReqBody reqBody, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest<TReqBody> req = new(url, reqBody);
                    (RESTRequest<TReqBody> req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.CreateAsync(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<QueryResponse<TResBody>> ListAsync<TReqBody, TResBody>(string url, CRUDService<TSelf> service, TReqBody reqBody, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest<TReqBody> req = new(url, reqBody);
                    (RESTRequest<TReqBody> req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.ListAsync<TReqBody, TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<QueryResponse<TResBody>> ListAsync<TResBody>(string url, CRUDService<TSelf> service, RESTRequestOptions reqOptions = null, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest req = RESTRequest.Temp(url, reqOptions);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.ListAsync<TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<TResBody> UpdateAsync<TReqBody, TResBody>(string url, CRUDService<TSelf> service, TReqBody reqBody, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest<TReqBody> req = new(url, reqBody);
                    (RESTRequest<TReqBody> req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.UpdateAsync<TReqBody, TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<TResBody> RetrieveAsync<TResBody>(string url, CRUDService<TSelf> service, RESTRequestOptions reqOptions = null, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest req = RESTRequest.Temp(url, reqOptions);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.RetrieveAsync<TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<TResBody> PatchAsync<TResBody>(string url, CRUDService<TSelf> service, RESTRequestOptions reqOptions = null, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest req = RESTRequest.Temp(url, reqOptions);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.PatchAsync<TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<bool> DeleteAsync<TResBody>(string url, CRUDService<TSelf> service, RESTRequestOptions reqOptions = null, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest req = RESTRequest.Temp(url, reqOptions);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.DeleteAsync<TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<bool> DeleteAsync<TReqBody>(string url, CRUDService<TSelf> service, TReqBody reqBody, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest<TReqBody> req = new(url, reqBody);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.DeleteAsync<TReqBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }

            public static async UniTask<TResBody> CancelAsync<TResBody>(string url, CRUDService<TSelf> service, RESTRequestOptions reqOptions = null, params IPathParam[] pathParams)
            {
                try
                {
                    RESTRequest req = RESTRequest.Temp(url, reqOptions);
                    (RESTRequest req, IPathParam[] pathParams) tuple = AutoParamHelper.HandleAutoParams(service, req, pathParams);

                    string endpoint = RouteBuilder.Build(service, url, tuple.pathParams);
                    return await service.Client.CancelAsync<TResBody>(tuple.req, endpoint);
                }
                catch (Exception e)
                {
                    service.Client.HandleException(e);
                    return default;
                }
            }
        }
    }
}
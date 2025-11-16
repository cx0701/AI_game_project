using System;
using System.Collections.Generic;

namespace Glitch9.IO.RESTApi
{
    public abstract partial class CRUDClient<TSelf> where TSelf : CRUDClient<TSelf>
    {
        public static class AutoParamHelper
        {
            public static (TReq, IPathParam[]) HandleAutoParams<TReq>(CRUDService<TSelf> service, TReq req, IPathParam[] pathParams)
                where TReq : RESTRequest
            {
                try
                {
                    ThrowIf.ArgumentIsNull(
                        (service, $"Service"),
                        (service.Client, $"Client-{typeof(TSelf).Name}"),
                        (req, typeof(TReq).Name));

                    string apiName = service.Client.Name;
                    ThrowIf.IsNullOrEmpty(apiName, "API Name");
                    List<IPathParam> newPathParams = new(pathParams);

                    if (service.Client.autoApiKey != AutoParam.Unset)
                    {
                        string apiKey = service.Client.apiKeyGetter.Invoke();
                        if (string.IsNullOrEmpty(apiKey)) throw new NoApiKeyException(apiName);

                        if (service.Client.autoApiKey == AutoParam.Header)
                        {
                            string authHeaderKey = service.Client.apiKeyHeaderKey;
                            string authHeaderValue = string.Format(service.Client.apiKeyHeaderFormat, apiKey);

                            req.AddHeader(new(authHeaderKey, authHeaderValue));
                        }
                        else if (service.Client.autoApiKey == AutoParam.Query)
                        {
                            string key = service.Client.apiKeyQueryKey;
                            //InternalDebug.Pink($"Query API Key: {apiKey}");
                            if (string.IsNullOrEmpty(key)) throw new NoApiKeyQueryKeyException(apiName);

                            newPathParams.Add(PathParam.Query(key, apiKey));
                        }
                    }

                    bool betaVersionParamSet = false;

                    if (service.Client.autoBetaParam != AutoParam.Unset)
                    {
                        if (service.Client.autoBetaParam == AutoParam.Header)
                        {
                            if (service.BetaHeaders.IsNullOrEmpty())
                            {
                                foreach (RESTHeader header in service.BetaHeaders)
                                {
                                    req.AddHeader(header);
                                }
                            }
                            else
                            {
                                if (service.Client.betaHeader == null) throw new NoBetaHeaderException(apiName);
                                req.AddHeader(service.Client.betaHeader.Value);
                            }
                        }
                        else if (service.Client.autoBetaParam == AutoParam.Path)
                        {
                            if (string.IsNullOrEmpty(service.Client.BetaVersion)) throw new NoBetaVersionException(apiName);
                            newPathParams.Add(PathParam.Version(service.Client.BetaVersion));
                            betaVersionParamSet = true;
                        }
                    }

                    if (!betaVersionParamSet && service.Client.autoVersionParam == AutoParam.Path)
                    {
                        if (string.IsNullOrEmpty(service.Client.Version)) throw new NoVersionException(apiName);
                        newPathParams.Add(PathParam.Version(service.Client.Version));
                    }

                    if (!service.Client.additionalHeaders.IsNullOrEmpty())
                    {
                        foreach (RESTHeader header in service.Client.additionalHeaders)
                        {
                            req.AddHeader(header);
                        }
                    }

                    return (req, newPathParams.ToArray());
                }
                catch (Exception e)
                {
                    service.Client.Logger.Error($"Error in AutoParamHelper: {e.Message}");
                    return (req, pathParams);
                }
            }
        }
    }
}
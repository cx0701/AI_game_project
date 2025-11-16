using Newtonsoft.Json;

namespace Glitch9.IO.RESTApi
{
    public class QueryRequest<TModel> : RESTRequestBody
    {
        // Token-based pagination -----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Optional. The maximum number of data to return (per page). The service may return fewer data.
        /// If unspecified, at most 10 data will be returned.The maximum size limit is 20 data per page.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary> 
        /// Optional. A page token, received from a previous corpora.list call.
        /// Provide the nextPageToken returned in the response as an argument to the next request to retrieve the next page.
        /// When paginating, all other parameters provided to corpora.list must match the call that provided the page token.
        /// </summary>
        public string PageToken { get; set; }

        // Cursor-based pagination -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Optional. A limit on the number of objects to be returned. 
        /// Limit can range between 1 and 100, and the default is 20.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// [Optional]
        /// Sort order by the created_at timestamp of the objects. 
        /// asc for ascending order and desc for descending order.
        /// </summary>
        public QueryOrder? Order { get; set; }

        /// <summary>
        /// Optional. A cursor for use in pagination. 
        /// after is an object ID that defines your place in the list. 
        /// For instance, if you make a list request and receive 100 objects, 
        /// ending with obj_foo, your subsequent call can include after=obj_foo in order to fetch the next page of the list.
        /// </summary>
        public string After { get; set; }

        /// <summary>
        /// Optional. A cursor for use in pagination. 
        /// Before is an object ID that defines your place in the list. 
        /// For instance, if you make a list request and receive 100 objects, 
        /// ending with obj_foo, your subsequent call can include Before=obj_foo in order to fetch the previous page of the list.
        /// </summary>
        public string Before { get; set; }

        [JsonConstructor] public QueryRequest() { }

        public QueryRequest(int? pageSize = null, string pageToken = null)
        {
            PageSize = pageSize;
            PageToken = pageToken;
        }

        public class Builder : RequestBodyBuilder<Builder, QueryRequest<TModel>>
        {
            public Builder SetLimit(int limit)
            {
                if (limit < 1 || limit > 100)
                {
                    LogService.Error($"List of {typeof(TModel).Name}'s limit must be between 1 and 100. Defaulting to 20.");
                    return this;
                }

                if (limit == 20) return this;

                _req.Limit = limit;
                return this;
            }

            public Builder SetOrder(QueryOrder order)
            {
                if (order == QueryOrder.Unset) return this;
                _req.Order = order;
                return this;
            }

            public Builder SetAfter(string after)
            {
                if (string.IsNullOrEmpty(after)) return this;
                _req.After = after;
                return this;
            }

            public Builder SetBefore(string before)
            {
                if (string.IsNullOrEmpty(before)) return this;
                _req.Before = before;
                return this;
            }

            public Builder SetCursor(QueryCursor cursor)
            {
                if (cursor == null) return this;
                _req.After = cursor.After;
                _req.Before = cursor.Before;
                return this;
            }
        }
    }

    /// <summary>
    /// A cursor for use in pagination.
    /// </summary>
    public class QueryCursor
    {
        /// <summary>
        /// An object ID that defines your place in the list.
        /// For instance, if you make a list request and receive 100 objects,
        /// ending with obj_foo, your subsequent call can include after=obj_foo
        /// in order to fetch the next page of the list.
        /// </summary>
        public string After { get; set; }

        /// <summary>
        /// An object ID that defines your place in the list.
        /// For instance, if you make a list request and receive 100 objects,
        /// ending with obj_foo, your subsequent call can include before=obj_foo
        /// in order to fetch the previous page of the list.
        /// </summary>
        public string Before { get; set; }

        public QueryCursor(string after, string before)
        {
            After = after;
            Before = before;
        }
    }

    public enum QueryOrder
    {
        Unset,
        [ApiEnum("desc")] Descending,
        [ApiEnum("asc")] Ascending,
        [ApiEnum("created_at")] CreatedAt,
    }
}

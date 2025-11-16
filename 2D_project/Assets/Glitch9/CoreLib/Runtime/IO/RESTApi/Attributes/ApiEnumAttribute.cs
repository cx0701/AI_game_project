using System;
using UnityEngine;

namespace Glitch9.IO.RESTApi
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ApiEnumAttribute : InspectorNameAttribute
    {
        public string ApiName { get; protected set; }

        public ApiEnumAttribute(string apiName) : base(apiName)
        {
            ApiName = apiName;
        }

        public ApiEnumAttribute(string displayName, string apiName) : base(displayName)
        {
            //DisplayName = displayName;
            ApiName = apiName;
        }
    }
}
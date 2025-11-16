using System;
using UnityEngine;

namespace Glitch9.IO.RESTApi
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class QueryParameterAttribute : PropertyAttribute
    {
        public string Name { get; protected set; }

        public QueryParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
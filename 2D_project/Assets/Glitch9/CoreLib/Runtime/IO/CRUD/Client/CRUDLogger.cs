using System.Collections.Generic;
using System.Reflection;

namespace Glitch9.IO.RESTApi
{
    public class CRUDLogger : RESTLogger
    {
        internal static class CRUDOps
        {
            internal const string UNKNOWN = "Unknown CRUD operation executing";
            internal const string CREATE = "Creating";
            internal const string UPDATE = "Updating";
            internal const string PATCH = "Patching";
            internal const string RETRIEVE = "Retrieving";
            internal const string LIST = "Querying (Getting list of)";
            internal const string DELETE = "Deleting";
            internal const string CANCEL = "Cancelling";
        }

        private static readonly Dictionary<CRUDMethod, string> _messages = new()
        {
            {CRUDMethod.Create, CRUDOps.CREATE},
            {CRUDMethod.Update, CRUDOps.UPDATE},
            {CRUDMethod.Patch, CRUDOps.PATCH},
            {CRUDMethod.Retrieve, CRUDOps.RETRIEVE},
            {CRUDMethod.List, CRUDOps.LIST},
            {CRUDMethod.Delete, CRUDOps.DELETE},
            {CRUDMethod.Cancel, CRUDOps.CANCEL}
        };


        public CRUDLogger(string tag, RESTLogLevel logLevel) : base(tag, logLevel) { }

        public void Info(CRUDMethod method, MemberInfo type)
        {
            if (_messages.TryGetValue(method, out string message))
            {
                LogMessage(message, type);
            }

            LogMessage(CRUDOps.UNKNOWN, type);
        }

        public void Create(MemberInfo type)
        {
            LogMessage(CRUDOps.CREATE, type);
        }

        public void Update(MemberInfo type)
        {
            LogMessage(CRUDOps.UPDATE, type);
        }

        public void Patch(MemberInfo type)
        {
            LogMessage(CRUDOps.PATCH, type);
        }

        public void Retrieve(MemberInfo type)
        {
            LogMessage(CRUDOps.RETRIEVE, type);
        }

        public void List(MemberInfo type)
        {
            LogMessage(CRUDOps.LIST, type);
        }

        public void Delete(MemberInfo type)
        {
            LogMessage(CRUDOps.DELETE, type);
        }

        public void Cancel(MemberInfo type)
        {
            LogMessage(CRUDOps.CANCEL, type);
        }

        private void LogMessage(string action, MemberInfo type)
        {
            Info($"{action} {type.Name}.");
        }
    }
}
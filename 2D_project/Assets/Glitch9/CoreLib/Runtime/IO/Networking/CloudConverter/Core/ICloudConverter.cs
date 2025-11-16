using System;

namespace Glitch9.IO.Networking
{
    public interface ICloudConverter
    {
        object ToLocalFormat(Type type, string propertyName, object propertyValue);
        object ToCloudFormat(Type type, object propertyValue);
    }

    public abstract class CloudConverter<T> : ICloudConverter
    {
        public object ToLocalFormat(Type type, string propertyName, object propertyValue) => ToLocalFormat(propertyName, propertyValue);
        public object ToCloudFormat(Type type, object propertyValue) => ToCloudFormat((T)propertyValue);
        public abstract T ToLocalFormat(string propertyName, object propertyValue);
        public abstract object ToCloudFormat(T propertyValue);
    }
}
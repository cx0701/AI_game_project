using System;
using System.Globalization;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [Serializable]
    public class FileData : IData
    {
        [SerializeField] private string id;
        [SerializeField] private string name;
        [SerializeField] private int bytes;
        [SerializeField] private string purpose;
        [SerializeField] private UnixTime created;
        [SerializeField] private AIProvider api;
        [SerializeField] private Metadata metadata;

        public AIProvider Api { get => api; set => api = value; }
        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Bytes { get => bytes; set => bytes = value; }
        public string Purpose { get => purpose; set => purpose = value; }
        public UnixTime CreatedAt { get => created; set => created = value; }
        public Metadata Metadata { get => metadata; set => metadata = value; }


        public bool Equals(FileData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Name == other.Name && Bytes == other.Bytes && Nullable.Equals(CreatedAt, other.CreatedAt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Bytes, CreatedAt);
        }

        #region Utility Methods

        private static class MetaKeys
        {
            internal const string UploadPurpose = "Upload Purpose";
        }

        public FileData AddMetadata(string key, string value)
        {
            ThrowIf.IsNullOrEmpty(key, "Log Metadata Key");

            Metadata ??= new();
            Metadata.AddOrUpdate(key, value);

            return this;
        }

        public FileData AddMetadata<TEnum>(TEnum value) where TEnum : Enum
        {
            string key = value.GetType().Name.ToPascalCase();
            return AddMetadata(key, value.GetInspectorName());
        }

        public FileData AddMetadata(string key, float value) => AddMetadata(key, value.ToString(CultureInfo.InvariantCulture));
        public FileData AddMetadata<TEnum>(string key, TEnum value) where TEnum : Enum => AddMetadata(key, value.GetInspectorName());
        public FileData AddPurpose(float purpose) => AddMetadata(MetaKeys.UploadPurpose, purpose);
        public FileData AddPurpose<TEnum>(TEnum purpose) where TEnum : Enum => AddMetadata(MetaKeys.UploadPurpose, purpose.GetInspectorName());

        public FileData SetName(string fileName)
        {
            name = fileName;
            return this;
        }

        public void Save() => FileLibrary.Add(this);

        #endregion Utility Methods
    }
}
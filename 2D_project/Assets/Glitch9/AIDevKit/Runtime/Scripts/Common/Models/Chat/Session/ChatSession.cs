using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Glitch9.IO.RESTApi;
using System.IO;
using System;

namespace Glitch9.AIDevKit
{
    [JsonObject]
    public class ChatSession : IData
    {
        private static readonly JsonSerializerSettings _localJsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ContractResolver = new RESTContractResolver { NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true } },
            Converters = new List<JsonConverter>
            {
                new ApiEnumConverter(),
                new ChatMessageConverter(AIProvider.None),
                new ChatRoleConverter(AIProvider.None),
                new ContentConverter(AIProvider.None),
            },
        };

        // chat session properties -------------------------------------------------------------------------------
        [JsonProperty] public string Id { get; private set; }
        [JsonProperty] public string Name { get; set; } = "New Chat";
        [JsonProperty] public UnixTime CreatedAt { get; private set; } = UnixTime.Now;
        [JsonProperty] public UnixTime UpdatedAt { get; private set; } = UnixTime.Now;
        [JsonIgnore] public List<ChatMessage> Messages => _messages ??= new();

        // chat session settings ---------------------------------------------------------------------------------
        [JsonProperty] public bool AutoSave { get; set; } = true;
        [JsonProperty] public bool Stream { get; set; } = true;
        [JsonProperty] public int MaxContextMessages { get; set; } = 20;
        [JsonProperty] public Model Model { get; set; } = AIDevKitConfig.kDefault_Chat_Model;
        [JsonProperty] public ModelOptions ModelOptions { get; set; }
        [JsonProperty] public ReasoningOptions ReasoningOptions { get; set; }
        [JsonProperty] public WebSearchOptions WebSearchOptions { get; set; }
        [JsonProperty] public SpeechOutputOptions SpeechOutputOptions { get; set; }
        [JsonProperty] public string SystemInstruction { get; set; }
        [JsonProperty] public string StartingMessage { get; set; }

        // summary related ---------------------------------------------------------------------------------------
        [JsonProperty] public Model SummaryModel { get; set; } = AIDevKitConfig.kDefault_Chat_SummaryModel;
        [JsonProperty] public string Summary { get; private set; } = "No summary available.";
        [JsonProperty] public UnixTime LastSummaryUpdate { get; private set; } = UnixTime.MinValue;

        [JsonProperty] private List<ChatMessage> _messages = new(); // This should never ever be null 
        [JsonProperty] private List<ChatMessage> _recentMessages = new(); // This should never ever be null
        [JsonIgnore] public List<ChatMessage> RecentMessages => _recentMessages ??= new(); // This should never ever be null
        [JsonIgnore] private readonly ChatSummarizer _summarizer = new();
        [JsonIgnore] private DateTime _lastSaveTime = DateTime.MinValue;
        [JsonIgnore] private bool _saveScheduled = false;

        [JsonConstructor] public ChatSession() { }
        private ChatSession(string id, string name = null, string startingMessage = null)
        {
            Id = id;

            if (string.IsNullOrWhiteSpace(name))
            {
                Name = "New Chat";
            }
            else
            {
                Name = name;
            }

            if (!string.IsNullOrWhiteSpace(startingMessage))
            {
                StartingMessage = startingMessage;
                PushMessage(ChatRole.Assistant, startingMessage);
            }
        }

        public static ChatSession CreateFile(string id = null, string name = null, string startingMessage = null)
        {
            ChatSession session = new(id ?? CreateID(), name, startingMessage);
            Debug.Log($"Created new chat session: {session.Id}");
            session.SaveFile();
            return session;
        }

        public static ChatSession LoadFile(string id = null, string name = null, string startingMessage = null)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            string path = GetChatSavePath();
            string fileName = id + ".json";
            string filePath = Path.Combine(path, fileName);

            if (!File.Exists(filePath)) return null;

            string json = File.ReadAllText(filePath);
            ChatSession session = JsonConvert.DeserializeObject<ChatSession>(json, _localJsonSerializerSettings);

            if (session == null) return null;
            //session.InitializeRecentMessages();
            return session;
        }

        public static bool DeleteChatSessionFile(string id)
        {
            string path = GetChatSavePath();
            string fileName = id + ".json";
            string filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        private static string CreateID() => $"chat_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
        internal static string GetChatSavePath() => Path.Combine(Application.persistentDataPath, "Chats");

        public void PushMessage(ChatMessage message)
        {
            //GNDebug.Pink($"PushMessage(ChatMessage message): {message}");

            Messages.Add(message);
            UpdatedAt = UnixTime.Now;
            if (AutoSave) ScheduleAutoSave();

            if (message.Role == ChatRole.Assistant)
            {
                UpdateSummaryIfNecessary();
            }
        }

        public ChatMessage PushMessage(ChatRole role, string message)
        {
            //GNDebug.Pink($"PushMessage(ChatRole role, string message):{message}");

            ChatMessage chatMessage = new(role, message);
            PushMessage(chatMessage);

            return chatMessage;
        }

        private async void UpdateSummaryIfNecessary()
        {
            if (RecentMessages.IsNullOrEmpty() || RecentMessages.Count < MaxContextMessages) return;

            try
            {
                string newSummary = await _summarizer.UpdateSummaryAsync(SummaryModel, Summary, RecentMessages);
                if (!string.IsNullOrWhiteSpace(newSummary)) Summary = newSummary;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating summary: {e.Message}");
            }
            finally
            {
                LastSummaryUpdate = UnixTime.Now;
                RecentMessages.Clear();
                if (AutoSave) ScheduleAutoSave();
            }
        }

        public void ClearMessages()
        {
            Messages.Clear();
            UpdatedAt = UnixTime.Now;
            if (AutoSave) SaveFile();
        }

        private async void ScheduleAutoSave()
        {
            if (_saveScheduled) return;

            _saveScheduled = true;

            await System.Threading.Tasks.Task.Delay(500); // 0.5초 딜레이로 디바운스

            if ((DateTime.UtcNow - _lastSaveTime).TotalSeconds >= 0.3)
            {
                SaveFileAsync();
                _lastSaveTime = DateTime.UtcNow;
            }

            _saveScheduled = false;

            //Debug.Log($"Auto-saved chat session: {Id} at {_lastSaveTime}");
        }

        public void SaveFile(string customPath = null)
        {
            string path = customPath ?? GetChatSavePath();
            string fileName = Id + ".json";
            string filePath = Path.Combine(path, fileName);

            string json = JsonConvert.SerializeObject(this, _localJsonSerializerSettings);

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(filePath, json);
        }

        public async void SaveFileAsync(string customPath = null)
        {
            string path = customPath ?? GetChatSavePath();
            string fileName = Id + ".json";
            string filePath = Path.Combine(path, fileName);

            string json = JsonConvert.SerializeObject(this, _localJsonSerializerSettings);

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(filePath, json);
        }

        public void DeleteFile()
        {
            string path = GetChatSavePath();
            string fileName = Id + ".json";
            string filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public bool Equals(ChatSession other) => other != null && Id == other.Id;
        public override int GetHashCode() => Id?.GetHashCode() ?? 0;
    }
}
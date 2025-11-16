using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ChatRoom : MonoBehaviour
{
    private string apiKey = "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"; 
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    [Header("Chat Settings")]
    public string characterName = "AI Assistant"; // 角色名稱
    public string systemMessage = "You are a helpful and friendly AI."; // AI 的人格設定

    // 用於顯示聊天結果
    public TMP_Text chatOutput;
    public TMP_InputField  userInput;

    public void SendMessageToAI()
    {
        string userMessage = userInput.text;

        // 檢查是否輸入了內容
        if (string.IsNullOrEmpty(userMessage))
        {
            Debug.Log("userMessage" + userMessage);
            return;
        }

        StartCoroutine(SendChatRequest(userMessage));
    }

    IEnumerator SendChatRequest(string message)
    {
        // 構建請求的 JSON 輸入
        var requestData = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "system", content = systemMessage },
                new { role = "user", content = message }
            },
            max_tokens = 150,
            temperature = 0.7f
        };

        // 序列化為 JSON
        string jsonData = JsonUtility.ToJson(requestData);

        // 構建請求
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // 發送請求
        yield return request.SendWebRequest();

        // 處理回應
        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<ChatResponse>(request.downloadHandler.text);
            string reply = response.choices[0].message.content.Trim();
            DisplayMessage(characterName, reply);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            DisplayMessage(characterName, "I'm sorry, I couldn't process your request.");
        }
    }

    void DisplayMessage(string sender, string message)
    {
        chatOutput.text += $"{sender}: {message}\n";
    }

    [System.Serializable]
    public class ChatResponse
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public Message message;

            [System.Serializable]
            public class Message
            {
                public string role;
                public string content;
            }
        }
    }
}


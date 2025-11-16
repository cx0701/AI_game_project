using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
public class TextConversation : MonoBehaviour
{


    public Animator showStart;
    public Animator showBoss;
    public string jsonText;
    public TMP_Text outputText;
    public TMP_InputField myInputField;
    public GameObject Boss;
    public Coroutine SendCoroutine;
    public Coroutine SpeechCoroutine;
    string userInput;
    string responseText;
    [SerializeField] private string questionText;
    bool isFirstReqt = false;
    string apiKey = "AIzaSyCAz79y4OS54s6yFDX_oTLIydZgYbSAvY0";  // 使用你的API金鑰
    string endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";  // 正確的 API 端

    //815061892723-uiqrqh1c7oia32cbblbt4t9d5t2vv1mc.apps.googleusercontent.com
    [SerializeField] textToSpeech textToSpeech;
    [SerializeField] ComfyUIClient ComfyUIClient;
    void Start()
    {
        isFirstReqt = false;
    }
    public IEnumerator ShowStart()
    {
        showStart.Play("Start_Ani");
        yield return new WaitForSeconds(1.5f);
        showBoss.Play("ShowBoss");

        string RequestText = @"你現在是猜謎遊戲的關主:
            1.個性:外表高冷，但是個性溫柔的帥哥、不會使用驚嘆號和表情符號
            2.請以動物為題目出題
            3..讓我猜，先不要告訴我答案
            4.出題格式請遵守以下原則:
            開頭為請仔細聆聽我的問題。
            請描述題目20字就好且小學生也能看懂的
            請用繁體中文給提示:題目類型是動物
            出題請勿重複
            6.如果我的回答是問題請回達我的問題
            7.如果我答對了請回答要包含「恭喜你答對了」、「題目中文和英文」，
            但是我的答錯請回答包含「答錯了」和給予一句話提示，不要回答是或不是";
        SendCoroutine = StartCoroutine(SendRequest(RequestText));
        myInputField.gameObject.SetActive(true);
        Boss.SetActive(true);
    }
    public void StartGame()
    {
        StartCoroutine(ShowStart());
    }
    public void sendAns()
    {
        string userInput = "我的回答/問題:" + myInputField.text
        + "以下是你設定的「題目」給我"
        + questionText + @"
        如果我的 回答 不包含動物名稱,請你依照設定的「題目」來回答我的問題和給予10字提示,
        相反如果我的 回答 是 包含動物名稱 請簡單檢查答案是否符合題目敘述，不須告訴我檢查過程
        ,檢查答案後 如果我答對了請告訴我動物的英文單字和答對了!,
        但是我的答錯回答請包含「答錯了」和給予一句話提示，不用補充內容";
        if (SpeechCoroutine != null) StopCoroutine(SpeechCoroutine); // 停止之前的語音合成
        StartCoroutine(SendRequest(userInput));
    }

    IEnumerator SendRequest(string userInput)
    {
        // 建立 POST 請求的 JSON 資料
        string jsonRequest = @"{
            'contents': [
                {
                    'parts': [
                        {
                            'text': '" + userInput + @"'
                        }
                    ]
                }
            ]
        }";

        byte[] postData = Encoding.UTF8.GetBytes(jsonRequest);

        string urlWithApiKey = endpoint + "?key=" + apiKey;

        UnityWebRequest www = new UnityWebRequest(urlWithApiKey, "POST");
        www.uploadHandler = new UploadHandlerRaw(postData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("完整 Response: " + www.downloadHandler.text);

            // 使用 Newtonsoft.Json 解析 JSON 字串
            try
            {
                JObject jsonResponse = JObject.Parse(www.downloadHandler.text);

                //提取陣列
                JArray AllText = jsonResponse["candidates"] as JArray;
                if (AllText != null && AllText.Count > 0)
                {
                    // 提取第一個候選者的 content
                    JObject content = AllText[0]["content"] as JObject;
                    if (content != null)
                    {
                        // 提取 parts 陣列
                        JArray getText = content["parts"] as JArray;
                        if (getText != null && getText.Count > 0)
                        {
                            // 提取第一個 part 的 text
                            responseText = getText[0]["text"]?.ToString();
                            responseText = Regex.Replace(responseText, @"\*", "");
                            if (isFirstReqt == false)
                            {
                                questionText = responseText;
                                isFirstReqt = true;
                            }
                            if (!string.IsNullOrEmpty(responseText))
                            {
                                Debug.Log("提取到的 Text: " + responseText);
                                outputText.text = responseText;
                                if (SendCoroutine != null) StopCoroutine(SendCoroutine);

                                if (responseText.Contains("答對了") || responseText.Contains("正確") || responseText.Contains("符合"))
                                {
                                    Debug.Log("答對了，遊戲結束");

                                    string lettersOnly = Regex.Replace(responseText, @"[^A-Za-z]", "");
                                    Debug.Log("英文字母：" + lettersOnly);

                                    ComfyUIClient.CheckResult.gameObject.SetActive(false);
                                    ComfyUIClient.promptText = "a cute " + lettersOnly;
                                    ComfyUIClient.SendPrompt();
                                    ComfyUIClient.CheckTip.SetActive(true);
                                    myInputField.gameObject.SetActive(false);
                                }
                                else
                                { 
                                    SpeechCoroutine = StartCoroutine(textToSpeech.SendTTSRequest(responseText));
                                }
                            }
                            else
                            {
                                Debug.LogWarning("回應中未找到 text 欄位或為空。");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("回應中未找到 parts 陣列。");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("回應中未找到 content 物件。");
                    }
                }
                else
                {
                    Debug.LogWarning("回應中未找到 candidates 陣列或為空。");
                }
            }

            catch (Exception e)
            {
                Debug.LogError("JSON 解析錯誤: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + www.error);
        }
    }


}

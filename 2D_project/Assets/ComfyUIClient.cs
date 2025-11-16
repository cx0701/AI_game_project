using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using TMPro;
using System.IO;          // 提供 Path, File 等檔案操作類別
using System.Text.RegularExpressions;  // 提供 Regex 類別
using UnityEngine.UI;
[System.Serializable]
public class PromptResponse
{
  public string prompt_id;
  public int number;
}
public class ComfyUIClient : MonoBehaviour
{

  public string outputFolder = "C:/Users/廖雨潔/Documents/ComfyUI/output"; 
  public RawImage targetRenderer;
  public string promptText = "a cute cat";
  public Coroutine ComfyCoroutine;
  public GameObject CheckTip;
  public TMP_Text CheckResult;

  string apiUrl = "http://127.0.0.1:8000/prompt";
  [SerializeField] textToSpeech textToSpeech;

  [ContextMenu("Send Prompt")]
  private void Start()
  {
    //SendPrompt();

  }
  void Awake()
  {
    if (CheckResult == null)
      Debug.LogError("CheckResult 尚未指定，請在 Inspector 中拖拉對應的 TMP_Text UI 元素。");
  }
  public void SendPrompt()
  {
    if (ComfyCoroutine != null) StopCoroutine(ComfyCoroutine);
    ComfyCoroutine = StartCoroutine(SendComfyUIPrompt());
  }

  IEnumerator SendComfyUIPrompt()
  {
    yield return new WaitForSeconds(0.5f); // 等待一段時間確保 Unity Web Request 可以正常發送

    if (string.IsNullOrEmpty(promptText))
    {
      Debug.LogError("請先設定 promptText。");
      yield break;
    }
    Debug.Log("開始送出 ComfyUI Prompt：" + promptText);
    // 準備 JSON 請求內容
    string jsonPayload = @"
{
  ""prompt"": {
    ""3"": {
      ""class_type"": ""KSampler"",
      ""inputs"": {
        ""seed"": 25646387861978,
        ""steps"": 20,
        ""cfg"": 8,
        ""sampler_name"": ""euler"",
        ""scheduler"": ""normal"",
        ""denoise"": 1,
        ""model"": [""4"", 0],
        ""positive"": [""6"", 0],
        ""negative"": [""7"", 0],
        ""latent_image"": [""5"", 0]
      }
    },
    ""4"": {
      ""class_type"": ""CheckpointLoaderSimple"",
      ""inputs"": {
        ""ckpt_name"": ""v1-5-pruned-emaonly-fp16.safetensors""
      }
    },
    ""5"": {
      ""class_type"": ""EmptyLatentImage"",
      ""inputs"": {
        ""width"": 512,
        ""height"": 512,
        ""batch_size"": 1
      }
    },
    ""6"": {
      ""class_type"": ""CLIPTextEncode"",
      ""inputs"": {
        ""text"": ""{PROMPT}"",
        ""clip"": [""4"", 1]
      }
    },
    ""7"": {
      ""class_type"": ""CLIPTextEncode"",
      ""inputs"": {
        ""text"": ""text, low quality, deformed, text, watermark, cropped, error, bad anatomy,"",
        ""clip"": [""4"", 1]
      }
    },
    ""8"": {
      ""class_type"": ""VAEDecode"",
      ""inputs"": {
        ""samples"": [""3"", 0],
        ""vae"": [""4"", 2]
      }
    },
    ""22"": {
      ""class_type"": ""SaveImage"",
      ""inputs"": {
        ""filename_prefix"": ""unity_test"",
        ""images"": [""8"", 0]
      }
    }
  }
}";
    jsonPayload = jsonPayload.Replace("{PROMPT}", promptText);



    UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      string jsonText = request.downloadHandler.text;
      Debug.Log("成功送出 ComfyUI Prompt：" + jsonText);

      // 解析 number
      PromptResponse response = JsonUtility.FromJson<PromptResponse>(jsonText);
      int number = response.number;

      // 嘗試載入圖片
      LoadImageByNumber(number);
    }
    else
    {
      Debug.LogError("ComfyUI API 錯誤：" + request.error + "\n回應：" + request.downloadHandler.text);
    }
  }

  public void LoadImageByNumber(int number)
  {
    StartCoroutine(LoadLatestImage());
  }
  IEnumerator LoadLatestImage()
  {
    yield return new WaitForSeconds(5f);
    CheckResult.gameObject.SetActive(true);
    string responseText = CheckResult.text;
    StartCoroutine(textToSpeech.SendTTSRequest(responseText));
    Debug.Log("開始載入最新圖片...");

    if (!Directory.Exists(outputFolder))
    {
      Debug.LogWarning("找不到資料夾：" + outputFolder);
      yield break;
    }

    // 取得所有 unity_test_ 開頭且符合格式的檔案
    var files = Directory.GetFiles(outputFolder, "unity_test_*.png");
    if (files.Length == 0)
    {
      Debug.LogWarning("找不到任何符合條件的圖片檔案");
      yield break;
    }

    int maxNumber = -1;
    string targetFile = "";
    CheckTip.SetActive(false);
    // 用 Regex 取出檔名中的數字部分
    Regex regex = new Regex(@"unity_test_(\d{5})_\.png");

    foreach (var file in files)
    {
      string filename = Path.GetFileName(file);
      var match = regex.Match(filename);
      if (match.Success)
      {
        int num = int.Parse(match.Groups[1].Value);
        if (num > maxNumber)
        {
          maxNumber = num;
          targetFile = file;
        }
      }
    }

    if (string.IsNullOrEmpty(targetFile))
    {
      Debug.LogWarning("找不到符合命名規則的圖片檔案");
      yield break;
    }

    byte[] fileData = File.ReadAllBytes(targetFile);
    Texture2D tex = new Texture2D(2, 2);
    tex.LoadImage(fileData);

    if (targetRenderer != null)
    {
      targetRenderer.texture = tex;
      targetRenderer.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
      Debug.Log("已載入最大序號圖片：" + targetFile);
      targetRenderer.gameObject.SetActive(true);
    }
    else
    {
      Debug.LogWarning("請指定 targetRenderer（RawImage）。");
    }

    yield return null;
  }
}

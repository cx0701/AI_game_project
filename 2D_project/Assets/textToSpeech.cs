using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

//處理 WAV 檔案
public static class WavUtility
{
     //WAV 檔案 轉成 Unity 的 AudioClip
    public static AudioClip ToAudioClip(byte[] wavFile, string name = "wav")
    {
        int channels = BitConverter.ToInt16(wavFile, 22); //第 22 位元組開始讀取兩個位元組，代表聲道數（1 表示單聲道，2 表示立體聲）
        int sampleRate = BitConverter.ToInt32(wavFile, 24); // 取得取樣率
        int subchunk2 = BitConverter.ToInt32(wavFile, 40); //第 40 位元組取得音訊資料的 byte 長度
        float[] data = ConvertByteToFloat(wavFile, 44, subchunk2, channels); //音訊的 byte 陣列轉成浮點數陣列（float[]），表示聲音波形
        AudioClip audioClip = AudioClip.Create(name, data.Length / channels, channels, sampleRate, false);
        audioClip.SetData(data, 0);
        return audioClip;
    }

    static float[] ConvertByteToFloat(byte[] array, int startIndex, int length, int channels)
    {
        float[] floatArr = new float[length / 2];
        int i = 0;
        while (i < floatArr.Length)
        {
            short sample = BitConverter.ToInt16(array, startIndex + i * 2);
            floatArr[i] = sample / 32768.0f;
            i++;
        }
        return floatArr;
    }
}

public class textToSpeech : MonoBehaviour
{
    string apiKey = "AIzaSyB1FIpTptnnF616GHhRlHZd2moDGtt6pnc";
    bool isEnglish = true;
    AudioSource audioSource; // 儲存 AudioSource
    private void Start()
    {
        //string text = "我警告過你了！";
        //StartCoroutine(SendTTSRequest(text));
    }
    bool IsEnglish(string text)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[A-Za-z\s]+$");
    }
    public IEnumerator SendTTSRequest(string text)
    {
        string url = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;

        isEnglish = IsEnglish(text);

        yield return null;
        // 'voice': {'languageCode': 'cmn-TW', 'ssmlGender': 'FEMALE'}, 
        Debug.Log("英文" + isEnglish);
        string json;

        json = "{\"input\": {\"text\": \"" + text + "\"}," +
            "\"voice\": {\"languageCode\": \"cmn-TW\", \"ssmlGender\": \"FEMALE\"}," +
            "\"audioConfig\": {\"audioEncoding\": \"LINEAR16\"}}";

        string jsonAngry = "{\"input\": {\"ssml\": \"<speak><prosody rate=\\\"medium\\\" pitch=\\\"+4st\\\" volume=\\\"+3dB\\\">" + text + "</prosody></speak>\"}," +
                   "\"voice\": {\"languageCode\": \"cmn-TW\", \"ssmlGender\": \"MALE\"}," +
                   "\"audioConfig\": {\"audioEncoding\": \"LINEAR16\"}}";


        byte[] postData = Encoding.UTF8.GetBytes(jsonAngry);

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(postData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("TTS Success!");
            string responseText = www.downloadHandler.text;
            string audioContent = ExtractAudioContent(responseText); //得到音訊內容

            byte[] audioBytes = Convert.FromBase64String(audioContent);
            yield return StartCoroutine(PlayAudio(audioBytes)); //播放音訊
        }
        else
        {
            Debug.LogError("TTS Error: " + www.error);
        }
    }

    // 提取 JSON 音訊內容
    string ExtractAudioContent(string jsonResponse)
    {
        string audioContentPrefix = "\"audioContent\": \"";
        int startIndex = jsonResponse.IndexOf(audioContentPrefix) + audioContentPrefix.Length;
        int endIndex = jsonResponse.IndexOf("\"", startIndex);
        return jsonResponse.Substring(startIndex, endIndex - startIndex);
    }

    // 播放音訊
    IEnumerator PlayAudio(byte[] audioBytes)
    {
        AudioClip clip = WavUtility.ToAudioClip(audioBytes);

        // 如果 audioSource 尚未建立才建立
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = clip;
        audioSource.Play();
        yield return null;
    }
    public void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}


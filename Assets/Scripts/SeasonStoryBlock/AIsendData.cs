using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class AIsendData : MonoBehaviour
{
    public StoryBlockOrganize storyBlockOrganize;

    private List<Dictionary<string, string>> DialogueData = new();
    private string url = "https://api.siliconflow.cn/v1/chat/completions";
    private string apiKey = "sk-cjktrxbohzgcvvcgkeppefasertnysxdmerrowgadqkciews";
    private string _response;

    [System.Serializable]
    public class ApiSilicion
    {
        public List<Choice> choices;
    }
    [System.Serializable]
    public class Choice
    {
        public Message message;
    }
    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    public void SendToSilicon()
    {
        if(DialogueData.Count == 0)
        {
            var newmessage = new Dictionary<string, string>
            {
                {"role","user" },
                {"content", "��������һ��ѧϰ����ĺ��ӣ��û���һ��������ʦ����Ҫ�����û�ÿ�����ȵĳɳ����¡��������ɵ�һ���ȵĹ��¡�"}
            };

            DialogueData.Add(newmessage);
        }

        var newmessage1 = new Dictionary<string, string>
        {
            {"role","user" },
            {"content", "���ڵ�ʱ���ǵ�һ���ȣ���"}
        };

        //DialogueData.Add(newmessage1);

        var payload = new
        {
            model = "meta-llama/Llama-3.3-70B-Instruct",
            messages = DialogueData,
            stream = false,
        };
        //string jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
        string jsonPayload = JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.Indented);
        StartCoroutine(postRequestSilicon(url, jsonPayload));
    }

    IEnumerator postRequestSilicon(string url,string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "Bearer " + apiKey);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            _response = uwr.downloadHandler.text;
            ApiSilicion apiResponse = JsonUtility.FromJson<ApiSilicion>(_response);
            string responseJson = apiResponse.choices[0].message.content;
            Debug.Log(responseJson);

            //����content
            storyBlockOrganize.CreatStoryBlock(responseJson);
        }
    }
}




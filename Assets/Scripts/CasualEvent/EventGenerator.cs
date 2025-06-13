using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private Player player;
    private List<Dictionary<string, string>> dialogueData = new();
    private const string url = "https://api.siliconflow.cn/v1/chat/completions";
    private const string apiKey = "sk-cjktrxbohzgcvvcgkeppefasertnysxdmerrowgadqkciews";
    private bool apiCallInProgress = false;

    private string curMingsha;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        }
        boxCollider2D.isTrigger = true;
    }

    private void OnMouseDown()
    {
        if (player != null && gameObject.activeInHierarchy && !apiCallInProgress)
        {
            apiCallInProgress = true;
            GenerateEvent();
        }
    }


    [System.Serializable]
    private class ApiResponse
    {
        public List<Choice> choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class ResponseFormat
    {
        public string type = "json_object";
    }

    private void UpdateCurrentMingsha()
    {
        int playerAge = player.curYear;

        switch (playerAge)
        {
            case 0:
                curMingsha = player.Mingsha_0;
                break;
            case 1:
                curMingsha = player.Mingsha_1;
                break;
            case 2:
                curMingsha = player.Mingsha_2;
                break;
            case 3:
                curMingsha = player.Mingsha_3;
                break;
            case 4:
                curMingsha = player.Mingsha_4;
                break;
            default:
                curMingsha = "超过年龄范围";
                break;
        }
    }

    public void GenerateEvent()
    {
        UpdateCurrentMingsha();
        StartCoroutine(SendToSiliconCoroutine());
    }

    private IEnumerator SendToSiliconCoroutine()
    {
        dialogueData.Clear();

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "system"},
            {"content", "你是一个精通周易的大师"}
        });

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "user"},
            {"content", $@"根据以下信息生成一个随机事件，要求事件符合角色当年的命煞，且尽可能详细，但是不要描述这个事件的结果（结局），字数在250字左右：
                玩家名字：{player.PlayerName}
                命煞：{curMingsha}

                Please respond in the format:
                {{
                    ""event"": {{
                        ""description"": ""事件描述"",
                        ""impact"": [
                            {{""event"": ""具体的事件""}}
                        ]
                    }}
                }}"}
        });

        var payload = new
        {
            model = "deepseek-ai/DeepSeek-V3",
            messages = dialogueData,
            response_format = new ResponseFormat(),
            stream = false
        };

        string jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);

        using var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(uwr.downloadHandler.text);
            string responseJson = apiResponse.choices[0].message.content;
            ProcessResponse(responseJson);
        }
        else
        {
            Debug.LogError($"API请求失败: {uwr.error}");
        }
    }

    private void ProcessResponse(string responseJson)
    {
        try
        {
            JObject response = JObject.Parse(responseJson);

            string description = response["event"]["description"].ToString();

            //Debug.Log($"生成的事件: {description}");
            player.SetCurMingshaEvent(description);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析响应时出错: {e.Message}\n原始响应:\n{responseJson}");
        }
    }
}
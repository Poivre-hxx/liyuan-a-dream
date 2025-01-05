using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ButtonAnPai : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Player player;
    private GetStory getStory;

    //public Dictionary<float, float> cirleChange = new();
    public List<cirles> changecirles = new();

    private BoxCollider2D boxCollider2D;

    private List<Dictionary<string, string>> dialogueData = new();
    private const string url = "https://api.siliconflow.cn/v1/chat/completions";
    private const string apiKey = "sk-cjktrxbohzgcvvcgkeppefasertnysxdmerrowgadqkciews";
    private string curMingsha;


    void Start()
    {
        SetupCollider();
    }

    private void SetupCollider()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCollider2D.isTrigger = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        }
    }

    private void OnMouseDown()
    {
        if (targetObject != null)
        {

            ////时间增加
            ////TimeControl();
            //gameControl.SetObjectVisible(targetObject);
            //Debug.Log($"已经切换为: {targetObject.activeSelf}");
            StartCoroutine(TrainChange());
        }
        else
        {
            Debug.LogWarning("失败");
        }
    }

    public void TimeControl()
    {
        if(player.season == Seasons.winter)
        {
            player.Year++;
            player.season = Seasons.spring;
        }
        else
        {
            player.season++;
        }
    }

    IEnumerator TrainChange()
    {
        foreach(var cirle in changecirles)
        {
            yield return cirle.circleProcess.StartProgress(cirle.curFill, cirle.targetFill);
        }

        yield return new WaitForSeconds(0.5f);

        TimeControl();

        foreach(var cirle in changecirles)
        {
            cirle.curFill = cirle.targetFill;   
        }

        GenerateEvent();

        gameControl.SetObjectVisible(targetObject);


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
            {"content", $@"根据以下信息生成一个随机事件，要求事件符合角色当年的命煞，且尽可能详细，可以是积极的事情、也可以是消极的事件。但是，请不要描述这个事件的结果（也就是结局），字数控制在250字，谢谢您：
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
            model = "meta-llama/Llama-3.3-70B-Instruct",
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

            Debug.Log($"生成的事件: {description}");
            getStory.UpdateStoryText();
            player.SetCurMingshaEvent(description);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析响应时出错: {e.Message}\n原始响应:\n{responseJson}");
        }
    }
}

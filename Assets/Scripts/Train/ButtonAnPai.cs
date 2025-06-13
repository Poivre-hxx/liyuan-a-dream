using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class ButtonAnPai : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Player player;

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

    /// <summary>
    /// 这里改变circle，然后选择进入唱念做打，不要 yield return StartCoroutine(GenerateEvent());
    /// </summary>
    /// <returns></returns>
    IEnumerator TrainChange()
    {
        foreach (var cirle in changecirles)
        {
            yield return cirle.circleProcess.StartProgress(cirle.curFill, cirle.targetFill);
        }
        yield return new WaitForSeconds(0.5f);
        TimeControl();
        foreach (var cirle in changecirles)
        {
            cirle.curFill = cirle.targetFill;
        }
        yield return StartCoroutine(GenerateEvent());
        yield return new WaitForSeconds(2f);
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
            case 1:
                curMingsha = player.Mingsha_0;
                break;
            case 2:
                curMingsha = player.Mingsha_1;
                break;
            case 3:
                curMingsha = player.Mingsha_2;
                break;
            case 4:
                curMingsha = player.Mingsha_3;
                break;
            case 5:
                curMingsha = player.Mingsha_4;
                break;
            default:
                curMingsha = "超过年龄范围";
                break;
        }
    }

    public IEnumerator GenerateEvent()
    {
        UpdateCurrentMingsha();
        yield return StartCoroutine(SendToSiliconCoroutine());
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
            {"content", $@"根据以下信息生成一个随机事件，要求如下：
                1. 事件必须符合角色当年的命煞，但是故事中不要提及具体的命煞名字
                2. 事件描述必须完全使用中文，不要出现数字等符号
                3. 请不要包含任何特殊符号或英文
                4. 字数限制在120-150字之间
                5. 所述故事请符合中国这个年代的时代背景
                6. 请以第一人称“我”的视角叙述这个故事，是一个10岁左右，已经拜师学习京剧，努力成为“武生”的孩子
        
                玩家信息：
                - 名字：{player.PlayerName}
                - 命煞：{curMingsha}
                - 事件的发生时间：{player.BirthDateYear}
        
                请按照以下格式回复：
                {{
                    ""event"": {{
                        ""description"": ""<在这里填写中文描述的事件>""
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
            responseJson = CleanJsonResponse(responseJson);
            //Debug.Log($"清理后的 JSON: {responseJson}");

            JObject response = JObject.Parse(responseJson);

            string description = response["event"]["description"]?.ToString();
            if (string.IsNullOrEmpty(description))
            {
                Debug.LogError("无法获取有效的事件描述");
                return;
            }

            //description = CleanDescription(description);
            //Debug.Log($"清理后的描述: {description}");

            if (player != null)
            {
                player.SetCurMingshaEvent(description);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"解析响应时出错: {e.Message}\n异常堆栈: {e.StackTrace}\n原始响应:\n{responseJson}");
        }
    }

    private string CleanJsonResponse(string json)
    {
        try
        {
            if (!json.TrimEnd().EndsWith("}"))
            {
                json = json.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries)[0] + "\"}}}";
            }
            return json;
        }
        catch (Exception ex)
        {
            Debug.LogError($"清理 JSON 时出错: {ex.Message}");
            return json;
        }
    }

    private string CleanDescription(string description)
    {
        try
        {
            description = Regex.Replace(description, @"[\(\)_\[\]{}]", " ");
            description = Regex.Replace(description, @"[^\u4e00-\u9fa5。，！？、；：""'']+", " ");
            description = Regex.Replace(description, @"\s+", " ").Trim();

            return description;
        }
        catch (Exception ex)
        {
            Debug.LogError($"清理描述时出错: {ex.Message}");
            return "描述有错误";
        }
    }
}

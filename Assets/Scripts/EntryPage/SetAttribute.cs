using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class SetAttribute : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputField nameInput, teacherInput, yearInput, monthInput, dayInput, hourInput;
    [SerializeField] private GameObject targetObject;

    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private GameControl gameControl;
    private List<Dictionary<string, string>> dialogueData = new();
    private const string url = "https://api.siliconflow.cn/v1/chat/completions";
    private const string apiKey = "sk-cjktrxbohzgcvvcgkeppefasertnysxdmerrowgadqkciews";
    private bool apiCallInProgress = false;

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

    private void Start()
    {
        InitializeComponents();
        gameControl = FindObjectOfType<GameControl>();
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
            if (nameInput != null && !string.IsNullOrEmpty(nameInput.text))
            {
                apiCallInProgress = true;
                UpdatePlayerInfo();
                StartCoroutine(CallAPIAndSwitchScene());
            }
            else
            {
                Debug.LogWarning("请输入姓名");
            }
        }
    }

    private void UpdatePlayerInfo()
    {
        string newName = nameInput.text.Replace("\n", "");
        string teacherName = teacherInput.text.Replace("\n", "");
        player.SetPlayerName(newName, teacherName);

        int year = ParseVerticalNumber(yearInput.text);
        int month = ParseVerticalNumber(monthInput.text);
        int day = ParseVerticalNumber(dayInput.text);
        int hour = ParseVerticalNumber(hourInput.text);
        player.SetBirthTime(year, month, day, hour);
        Debug.Log($"已更新玩家信息：{player.PlayerName}, 出生时间：{player.BirthDateTime}");
    }

    private IEnumerator CallAPIAndSwitchScene()
    {
        yield return StartCoroutine(SendToSiliconCoroutine());

        if (targetObject != null && gameControl != null)
        {
            gameControl.SetObjectVisible(targetObject);
            Debug.Log($"切换到场景: {targetObject.name}");
        }
        else
        {
            Debug.LogError($"场景切换失败 - Target: {targetObject != null}, GameControl: {gameControl != null}");
        }

        apiCallInProgress = false;
    }

    private IEnumerator SendToSiliconCoroutine()
    {
        dialogueData.Clear();
        var message = new Dictionary<string, string>
       {
           {"role", "user"},
           {"content", $@"请直接返回以下格式的JSON数据，不要添加任何额外说明：
               {{
                   ""mingshas"": [
                       {{""type"": ""命煞名称""}},
                       {{""type"": ""命煞名称""}},
                       {{""type"": ""命煞名称""}},
                       {{""type"": ""命煞名称""}}
                   ],
                   ""attributes"": {{
                       ""daode"": 数值(40-70),
                       ""chushi"": 数值(40-70),
                       ""rongmao"": 数值(40-70),
                       ""wencai"": 数值(40-70),
                       ""tipo"": 数值(40-70),
                       ""mingqi"": 数值(40-70)
                   }}
               }}
               
               请根据以下信息生成数据：
               玩家名字：{player.PlayerName}
               出生时间：{player.BirthDateTime}
               可选的命煞包括：天乙贵人、天德贵人、月德贵人、文昌贵人、太极贵人、天厨贵人、三奇贵人、德秀贵人、福星贵人、国印贵人、学堂、天医、将星、金舆、十恶大败、天罗、地网、桃花煞、孤辰、寡宿、灾煞、亡神、丧门、吊客、披麻、劫煞、孤鸾煞、红艳煞、九丑、童子煞、羊刃、血刃、流霞、华盖、驿马、红鸾。"}
       };
        dialogueData.Add(message);

        var payload = new
        {
            model = "meta-llama/Llama-3.3-70B-Instruct",
            messages = dialogueData,
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
            Debug.Log(responseJson);    
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
            // 提取JSON部分
            int jsonStart = responseJson.IndexOf('{');
            int jsonEnd = responseJson.LastIndexOf('}') + 1;

            if (jsonStart == -1 || jsonEnd == 0)
            {
                Debug.LogError($"未找到有效的JSON数据，原始响应:\n{responseJson}");
                return;
            }

            string jsonPart = responseJson.Substring(jsonStart, jsonEnd - jsonStart);
            Debug.Log($"提取的JSON: {jsonPart}");

            JObject response = JObject.Parse(jsonPart);

            string[] mingshas = new string[4];
            for (int i = 0; i < 4; i++)
            {
                mingshas[i] = response["mingshas"][i]["type"].ToString();
            }

            player.SetMingsha(mingshas[0], mingshas[1], mingshas[2], mingshas[3]);

            var attrs = response["attributes"];
            player.ModifyDaode(attrs["daode"].Value<int>());
            player.ModifyChushi(attrs["chushi"].Value<int>());
            player.ModifyRongmao(attrs["rongmao"].Value<int>());
            player.ModifyWencai(attrs["wencai"].Value<int>());
            player.ModifyTipo(attrs["tipo"].Value<int>());
            player.ModifyMingqi(attrs["mingqi"].Value<int>());

            Debug.Log($"命煞: {string.Join(", ", mingshas)}");
            Debug.Log($"属性值已更新");
        }
        catch (Exception e)
        {
            Debug.LogError($"解析响应时出错: {e.Message}\n原始响应:\n{responseJson}");
        }
    }

    private int ParseVerticalNumber(string verticalText)
    {
        if (string.IsNullOrEmpty(verticalText))
            return 0;
        string cleanText = verticalText.Replace("\n", "");
        return int.TryParse(cleanText, out int result) ? result : 0;
    }
}
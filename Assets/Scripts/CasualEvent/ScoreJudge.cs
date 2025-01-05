using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TMPro.Examples;

public class ScoreJudge : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Player player;
    [SerializeField] private Text promptText;
    [SerializeField] private InputField responseInput;

    private Image buttonImage;
    private List<Dictionary<string, string>> dialogueData = new();
    private const string url = "https://api.siliconflow.cn/v1/chat/completions";
    private const string apiKey = "sk-cjktrxbohzgcvvcgkeppefasertnysxdmerrowgadqkciews";
    private bool isProcessing = false;

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


    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        buttonImage = GetComponent<Image>();
    }

    private void OnMouseDown()
    {
        Debug.Log("按下了");
        if (!isProcessing && gameObject.activeInHierarchy)
        {
            StartCoroutine(JudgeResponse());
        }
    }

    public void HandleEvents()
    {
        StartCoroutine(JudgeResponse());
    }

    IEnumerator JudgeResponse()
    {
        Debug.Log("已按下");
        if (isProcessing || string.IsNullOrEmpty(responseInput.text)) yield break;

        string inputText = responseInput.text;
        responseInput.text = "";
        isProcessing = true;
        yield return StartCoroutine(SendToSiliconCoroutine());

        gameControl.SetObjectVisible(targetObject);
    }

    private IEnumerator SendToSiliconCoroutine()
    {
        Debug.Log("开始生成评价");
        dialogueData.Clear();

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "system"},
            {"content", "你是一个专业的修养评估师，需要根据提问和回答评估一个人的各项属性变化"}
        });

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "user"},
            {"content", $@"基于以下对话评估玩家修养属性的变化：
                问题：{promptText.text}
                回答：{responseInput.text}
                
                请根据回答的内容判断以下六个属性应该如何变化，变化范围为-10到10，变化幅度尽量大一些，谢谢您了：
                - 道德 (反映道德品质)
                - 出世 (反映处世能力)
                - 容貌 (反映外在形象)
                - 文才 (反映文学才能)
                - 体魄 (反映身体素质)
                - 名气 (反映运势变化)

                请返回json的格式:
                {{
                    ""attributes"": {{
                        ""daode"": 数值(-10到10),
                        ""chushi"": 数值(-10到10),
                        ""rongmao"": 数值(-10到10),
                        ""wencai"": 数值(-10到10),
                        ""tipo"": 数值(-10到10),
                        ""mingqi"": 数值(-10到10)
                    }},
                }}" }
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
        responseInput.text = "";

        // 等待API请求完成
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

        isProcessing = false;
    }

    private void ProcessResponse(string responseJson)
    {
        try
        {
            JObject response = JObject.Parse(responseJson);
            Debug.Log($"Player set Attribute{response}");
            player.AttributeResponse = response;

            //在attribute界面修改
            //var attrs = response["attributes"];

            // 修改玩家属性
            //player.ModifyDaodeChange(attrs["daode"].Value<int>());
            //player.ModifyChushiChange(attrs["chushi"].Value<int>());
            //player.ModifyRongmaoChange(attrs["rongmao"].Value<int>());
            //player.ModifyWencaiChange(attrs["wencai"].Value<int>());
            //player.ModifyTipoChange(attrs["tipo"].Value<int>());
            //player.ModifyMingqiChange(attrs["mingqi"].Value<int>());

            Debug.Log($"生成完毕");

        }
        catch (Exception e)
        {
            Debug.LogError($"解析响应时出错: {e.Message}\n原始响应:\n{responseJson}");
        }
    }
}

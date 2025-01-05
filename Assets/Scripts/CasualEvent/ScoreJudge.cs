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
        Debug.Log("������");
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
        Debug.Log("�Ѱ���");
        if (isProcessing || string.IsNullOrEmpty(responseInput.text)) yield break;

        string inputText = responseInput.text;
        responseInput.text = "";
        isProcessing = true;
        yield return StartCoroutine(SendToSiliconCoroutine());

        gameControl.SetObjectVisible(targetObject);
    }

    private IEnumerator SendToSiliconCoroutine()
    {
        Debug.Log("��ʼ��������");
        dialogueData.Clear();

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "system"},
            {"content", "����һ��רҵ����������ʦ����Ҫ�������ʺͻش�����һ���˵ĸ������Ա仯"}
        });

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "user"},
            {"content", $@"�������¶Ի���������������Եı仯��
                ���⣺{promptText.text}
                �ش�{responseInput.text}
                
                ����ݻش�������ж�������������Ӧ����α仯���仯��ΧΪ-10��10���仯���Ⱦ�����һЩ��лл���ˣ�
                - ���� (��ӳ����Ʒ��)
                - ���� (��ӳ��������)
                - ��ò (��ӳ��������)
                - �Ĳ� (��ӳ��ѧ����)
                - ���� (��ӳ��������)
                - ���� (��ӳ���Ʊ仯)

                �뷵��json�ĸ�ʽ:
                {{
                    ""attributes"": {{
                        ""daode"": ��ֵ(-10��10),
                        ""chushi"": ��ֵ(-10��10),
                        ""rongmao"": ��ֵ(-10��10),
                        ""wencai"": ��ֵ(-10��10),
                        ""tipo"": ��ֵ(-10��10),
                        ""mingqi"": ��ֵ(-10��10)
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

        // �ȴ�API�������
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(uwr.downloadHandler.text);
            string responseJson = apiResponse.choices[0].message.content;
            ProcessResponse(responseJson);
        }
        else
        {
            Debug.LogError($"API����ʧ��: {uwr.error}");
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

            //��attribute�����޸�
            //var attrs = response["attributes"];

            // �޸��������
            //player.ModifyDaodeChange(attrs["daode"].Value<int>());
            //player.ModifyChushiChange(attrs["chushi"].Value<int>());
            //player.ModifyRongmaoChange(attrs["rongmao"].Value<int>());
            //player.ModifyWencaiChange(attrs["wencai"].Value<int>());
            //player.ModifyTipoChange(attrs["tipo"].Value<int>());
            //player.ModifyMingqiChange(attrs["mingqi"].Value<int>());

            Debug.Log($"�������");

        }
        catch (Exception e)
        {
            Debug.LogError($"������Ӧʱ����: {e.Message}\nԭʼ��Ӧ:\n{responseJson}");
        }
    }
}

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
                Debug.LogWarning("����������");
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
        Debug.Log($"�Ѹ��������Ϣ��{player.PlayerName}, ����ʱ�䣺{player.BirthDateTime}");
    }

    private IEnumerator CallAPIAndSwitchScene()
    {
        yield return StartCoroutine(SendToSiliconCoroutine());

        if (targetObject != null && gameControl != null)
        {
            gameControl.SetObjectVisible(targetObject);
            Debug.Log($"�л�������: {targetObject.name}");
        }
        else
        {
            Debug.LogError($"�����л�ʧ�� - Target: {targetObject != null}, GameControl: {gameControl != null}");
        }

        apiCallInProgress = false;
    }

    private IEnumerator SendToSiliconCoroutine()
    {
        dialogueData.Clear();
        var message = new Dictionary<string, string>
       {
           {"role", "user"},
           {"content", $@"��ֱ�ӷ������¸�ʽ��JSON���ݣ���Ҫ����κζ���˵����
               {{
                   ""mingshas"": [
                       {{""type"": ""��ɷ����""}},
                       {{""type"": ""��ɷ����""}},
                       {{""type"": ""��ɷ����""}},
                       {{""type"": ""��ɷ����""}}
                   ],
                   ""attributes"": {{
                       ""daode"": ��ֵ(40-70),
                       ""chushi"": ��ֵ(40-70),
                       ""rongmao"": ��ֵ(40-70),
                       ""wencai"": ��ֵ(40-70),
                       ""tipo"": ��ֵ(40-70),
                       ""mingqi"": ��ֵ(40-70)
                   }}
               }}
               
               �����������Ϣ�������ݣ�
               ������֣�{player.PlayerName}
               ����ʱ�䣺{player.BirthDateTime}
               ��ѡ����ɷ���������ҹ��ˡ���¹��ˡ��µ¹��ˡ��Ĳ����ˡ�̫�����ˡ�������ˡ�������ˡ�������ˡ����ǹ��ˡ���ӡ���ˡ�ѧ�á���ҽ�����ǡ����ߡ�ʮ���ܡ����ޡ��������һ�ɷ���³������ޡ���ɷ������ɥ�š����͡����顢��ɷ�����ɷ������ɷ���ų�ͯ��ɷ�����С�Ѫ�С���ϼ�����ǡ�������𽡣"}
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
            Debug.LogError($"API����ʧ��: {uwr.error}");
        }
    }

    private void ProcessResponse(string responseJson)
    {
        try
        {
            // ��ȡJSON����
            int jsonStart = responseJson.IndexOf('{');
            int jsonEnd = responseJson.LastIndexOf('}') + 1;

            if (jsonStart == -1 || jsonEnd == 0)
            {
                Debug.LogError($"δ�ҵ���Ч��JSON���ݣ�ԭʼ��Ӧ:\n{responseJson}");
                return;
            }

            string jsonPart = responseJson.Substring(jsonStart, jsonEnd - jsonStart);
            Debug.Log($"��ȡ��JSON: {jsonPart}");

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

            Debug.Log($"��ɷ: {string.Join(", ", mingshas)}");
            Debug.Log($"����ֵ�Ѹ���");
        }
        catch (Exception e)
        {
            Debug.LogError($"������Ӧʱ����: {e.Message}\nԭʼ��Ӧ:\n{responseJson}");
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
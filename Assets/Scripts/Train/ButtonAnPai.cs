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

            ////ʱ������
            ////TimeControl();
            //gameControl.SetObjectVisible(targetObject);
            //Debug.Log($"�Ѿ��л�Ϊ: {targetObject.activeSelf}");
            StartCoroutine(TrainChange());
        }
        else
        {
            Debug.LogWarning("ʧ��");
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
    /// ����ı�circle��Ȼ��ѡ����볪�����򣬲�Ҫ yield return StartCoroutine(GenerateEvent());
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
                curMingsha = "�������䷶Χ";
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
            {"content", "����һ����ͨ���׵Ĵ�ʦ"}
        });

        dialogueData.Add(new Dictionary<string, string>
        {
            {"role", "user"},
            {"content", $@"����������Ϣ����һ������¼���Ҫ�����£�
                1. �¼�������Ͻ�ɫ�������ɷ�����ǹ����в�Ҫ�ἰ�������ɷ����
                2. �¼�����������ȫʹ�����ģ���Ҫ�������ֵȷ���
                3. �벻Ҫ�����κ�������Ż�Ӣ��
                4. ����������120-150��֮��
                5. ��������������й���������ʱ������
                6. ���Ե�һ�˳ơ��ҡ����ӽ�����������£���һ��10�����ң��Ѿ���ʦѧϰ���磬Ŭ����Ϊ���������ĺ���
        
                �����Ϣ��
                - ���֣�{player.PlayerName}
                - ��ɷ��{curMingsha}
                - �¼��ķ���ʱ�䣺{player.BirthDateYear}
        
                �밴�����¸�ʽ�ظ���
                {{
                    ""event"": {{
                        ""description"": ""<��������д�����������¼�>""
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
            Debug.LogError($"API����ʧ��: {uwr.error}");
        }
    }

    private void ProcessResponse(string responseJson)
    {
        try
        {
            responseJson = CleanJsonResponse(responseJson);
            //Debug.Log($"������ JSON: {responseJson}");

            JObject response = JObject.Parse(responseJson);

            string description = response["event"]["description"]?.ToString();
            if (string.IsNullOrEmpty(description))
            {
                Debug.LogError("�޷���ȡ��Ч���¼�����");
                return;
            }

            //description = CleanDescription(description);
            //Debug.Log($"����������: {description}");

            if (player != null)
            {
                player.SetCurMingshaEvent(description);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"������Ӧʱ����: {e.Message}\n�쳣��ջ: {e.StackTrace}\nԭʼ��Ӧ:\n{responseJson}");
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
            Debug.LogError($"���� JSON ʱ����: {ex.Message}");
            return json;
        }
    }

    private string CleanDescription(string description)
    {
        try
        {
            description = Regex.Replace(description, @"[\(\)_\[\]{}]", " ");
            description = Regex.Replace(description, @"[^\u4e00-\u9fa5��������������""'']+", " ");
            description = Regex.Replace(description, @"\s+", " ").Trim();

            return description;
        }
        catch (Exception ex)
        {
            Debug.LogError($"��������ʱ����: {ex.Message}");
            return "�����д���";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using Unity.Mathematics;


public class AttributeDisplay : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Text displayText;

    [Header("显示选项")]
    [SerializeField] private bool Name;
    [SerializeField] private bool BirthDate;
    [SerializeField] private bool Daode;
    [SerializeField] private bool Chushi;
    [SerializeField] private bool Rongmao;
    [SerializeField] private bool Wencai;
    [SerializeField] private bool Tipo;
    [SerializeField] private bool Mingqi;
    [SerializeField] private bool Shensha;

    public bool autoRefresh = true;
    private float smoothSpeed = 1f;

    private void Update()
    {
        //if (autoRefresh)
        //{
        //    UpdateDisplay();
        //}
    }

    public void UpdateDisplay()
    {
        if (player == null || displayText == null) return;

        string displayContent = "";

        if (Name)
            displayContent += $"{player.PlayerName} {player.Age}岁\n";

        if (BirthDate)
            displayContent += $"生辰：{player.BirthDateTime}\n";

        if (Daode)
            displayContent += $"{player.Daode}\n";

        if (Chushi)
            displayContent += $"{player.Chushi}\n";

        if (Rongmao)
            displayContent += $"{player.Rongmao}\n";

        if (Wencai)
            displayContent += $"{player.Wencai}\n";

        if (Tipo)
            displayContent += $"{player.Tipo}\n";

        if (Mingqi)
            displayContent += $"{player.Mingqi}\n";

        if (Shensha)
            displayContent += $"{player.Mingsha_0}、{player.Mingsha_1}、{player.Mingsha_2}、{player.Mingsha_3}、{player.Mingsha_4}\n";

        if (displayContent.EndsWith("\n"))
            displayContent = displayContent.TrimEnd('\n');

        displayText.text = displayContent;
    }

    public void ToggleName(bool value) { Name = value; UpdateDisplay(); }
    public void ToggleBirthDate(bool value) { BirthDate = value; UpdateDisplay(); }
    public void ToggleDaode(bool value) { Daode = value; UpdateDisplay(); }
    public void ToggleChushi(bool value) { Chushi = value; UpdateDisplay(); }
    public void ToggleRongmao(bool value) { Rongmao = value; UpdateDisplay(); }
    public void ToggleWencai(bool value) { Wencai = value; UpdateDisplay(); }
    public void ToggleTipo(bool value) { Tipo = value; UpdateDisplay(); }
    public void ToggleMingqi(bool value) { Mingqi = value; UpdateDisplay(); }
    public void ToggleShensha(bool value) { Shensha = value; UpdateDisplay(); }



    /// <summary>
    /// 随机时间后的增减
    /// </summary>
    /// <param name="AttributeResponse"></param>
    public void ChangeAttribute(JObject AttributeResponse )
    {

        var attrs = AttributeResponse["attributes"];

        //道德
        if (Daode) StartCoroutine(changeNum(attrs["daode"].Value<int>(), player.Daode));
        //处世
        if (Chushi) StartCoroutine(changeNum(attrs["chushi"].Value<int>(), player.Chushi));
        //容貌
        if (Rongmao) StartCoroutine(changeNum(attrs["rongmao"].Value<int>(), player.Rongmao));
        //文采
        if (Wencai) StartCoroutine(changeNum(attrs["wencai"].Value<int>(), player.Wencai ));
        //体魄
        if (Tipo) StartCoroutine(changeNum(attrs["tipo"].Value<int>(), player.Tipo));
        //名气
        if (Mingqi) StartCoroutine(changeNum(attrs["mingqi"].Value<int>(), player.Mingqi));
    }

    IEnumerator changeNum(int changeAmount,int oldValue)
    {
        int curValue = oldValue;
        float temp;
        while (!Mathf.Approximately(curValue, oldValue + changeAmount)){
            temp = Mathf.Lerp(curValue, oldValue + changeAmount, Time.deltaTime * smoothSpeed);
            curValue = (int)temp;
            displayText.text = curValue.ToString();
            yield return null;
        }
        player.ModifyDaode(changeAmount);
    }
}

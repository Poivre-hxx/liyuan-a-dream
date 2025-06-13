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

    private JObject response = new();

    private void OnEnable()
    {
        response = player.AttributeResponse;
        //player.AttributeResponse = null; 返回时清空
        //只有随机事件后会产生
        if (response != null)
        {
            //Debug.Log("show Coroutine");
            StartCoroutine(ChangeAttribute(response));
        }
        else
        {
            //Debug.Log("normal show");
            UpdateDisplay();
        }
    }

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
    IEnumerator ChangeAttribute(JObject AttributeResponse )
    {
        UpdateDisplay();
        var attrs = AttributeResponse["attributes"];
        //Debug.Log($"attrs:{attrs}");
        //道德
        if (Daode)
        {
            yield return changeNum(attrs["daode"].Value<int>(), player.Daode);
            player.ModifyDaode(attrs["daode"].Value<int>());
        }

       
        //处世
        if (Chushi)
        {
            yield return changeNum(attrs["chushi"].Value<int>(), player.Chushi);
            player.ModifyChushi(attrs["chushi"].Value<int>());
        }
   
        //容貌
        if (Rongmao)
        {
            yield return changeNum(attrs["rongmao"].Value<int>(), player.Rongmao);
            player.ModifyRongmao(attrs["rongmao"].Value<int>());
        }
            
        
        //文采
        if (Wencai)
        {
            yield return changeNum(attrs["wencai"].Value<int>(), player.Wencai);
            player.ModifyWencai(attrs["wencai"].Value<int>());
        }
            
       
        //体魄
        if (Tipo)
        {
            yield return changeNum(attrs["tipo"].Value<int>(), player.Tipo);
            player.ModifyTipo(attrs["tipo"].Value<int>());
        }

        //名气
        if (Mingqi)
        {
            yield return changeNum(attrs["mingqi"].Value<int>(), player.Mingqi);
            player.ModifyMingqi(attrs["mingqi"].Value<int>());
        }
    }

    IEnumerator changeNum(int changeAmount,int oldValue)
    {
        int curValue = oldValue;
        //Debug.Log("Start attribute change Coroutine");
        while (curValue <= oldValue + changeAmount){
            //Debug.Log(curValue);
            curValue++;
            displayText.text = curValue.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        curValue = oldValue + changeAmount;
        displayText.text = curValue.ToString();
    }
}

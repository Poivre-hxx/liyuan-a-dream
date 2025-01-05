using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

[Serializable]
public class PlayerInfo
{
    public string name;
    public int age;
    public string teacher;
    public int birY;
    public int birM;
    public int birD;
    public int birH;
}

[Serializable]
public class MingshaInfo
{
    public string mingsha_0;
    public string mingsha_1;
    public string mingsha_2;
    public string mingsha_3;
    public string mingsha_4;
}

[Serializable]
public class CurMingshaEvent
{
    public string mingsha_Event;
}


[Serializable]
public class AttributeInfo
{
    public int daode;
    public int chushi;
    public int rongmao;
    public int wencai;
    public int tipo;
    public int mingqi;
}

[Serializable]
public class AttributeChange
{
    public int daode_change;
    public int chushi_change;
    public int rongmao_change;
    public int wencai_change;
    public int tipo_change;
    public int mingqi_change;
}

[Serializable]
public class SkillInfo
{
    public int four_Sing;
    public int four_Chant;
    public int four_Do;
    public int four_Fight;
    public int five_Hand;
    public int five_Eye;
    public int five_Body;
    public int five_Magic;
    public int five_Foot;
}

public class Player : MonoBehaviour
{
    [Header("基本信息")]
    [SerializeField] private PlayerInfo playerInfo;

    [Header("命煞")]
    [SerializeField] private MingshaInfo mingshaInfo;

    [Header("当前的命煞事件")]
    [SerializeField] private CurMingshaEvent curMingshaEvent;

    [Header("属性值")]
    [SerializeField] private AttributeInfo attributeInfo;

    [Header("属性值的变化值")]
    [SerializeField] private AttributeChange attributeChange;

    [Header("技能值")]
    [SerializeField] private SkillInfo skillInfo;

    [Header("时间")]
    public int Year;
    public Seasons season;

    [Header("随机事件变化值")]
    public JObject AttributeResponse = null;


    // 访问基本信息的属性
    public string PlayerName => playerInfo.name;
    public string Teacher => playerInfo.teacher;
    public int Age => playerInfo.age;
    public string BirthDateTime => $"{playerInfo.birY}年{playerInfo.birM}月{playerInfo.birD}日";

    // 访问命煞
    public string Mingsha_0 => mingshaInfo.mingsha_0;
    public string Mingsha_1 => mingshaInfo.mingsha_1;
    public string Mingsha_2 => mingshaInfo.mingsha_2;
    public string Mingsha_3 => mingshaInfo.mingsha_3;
    public string Mingsha_4 => mingshaInfo.mingsha_4;

    // 访问当前的随机事件
    public string Mingsha_event => curMingshaEvent.mingsha_Event;

    // 访问属性信息的属性
    public int Daode => attributeInfo.daode;
    public int Chushi => attributeInfo.chushi;
    public int Rongmao => attributeInfo.rongmao;
    public int Wencai => attributeInfo.wencai;
    public int Tipo => attributeInfo.tipo;
    public int Mingqi => attributeInfo.mingqi;

    // 访问属性变化
    public int DaodeChange => attributeChange.daode_change;
    public int ChushiChange => attributeChange.chushi_change;
    public int RongmaoChange => attributeChange.rongmao_change;
    public int WencaiChange => attributeChange.wencai_change;
    public int TipoChange => attributeChange.tipo_change;
    public int MingqiChange => attributeChange.mingqi_change;

    // 访问技能信息的属性
    public int FourSing => skillInfo.four_Sing;
    public int FourChant => skillInfo.four_Chant;
    public int FourDo => skillInfo.four_Do;
    public int FourFight => skillInfo.four_Fight;
    public int FiveHand => skillInfo.five_Hand;
    public int FiveEye => skillInfo.five_Eye;
    public int FiveBody => skillInfo.five_Body;
    public int FiveMagic => skillInfo.five_Magic;
    public int FiveFoot => skillInfo.five_Foot;

    //访问年份
    public int curYear => Year;
    public Seasons curseason => season;

    public void SetPlayerName(string newName, string teacherName)
    {
        playerInfo.name = newName;
        playerInfo.teacher = teacherName;
    }

    public void AddAge()
    {
        playerInfo.age = playerInfo.age <= 10 ? playerInfo.age + 1 : playerInfo.age;
    }

    public void SetBirthTime(int year, int month, int day, int hour)
    {
        playerInfo.birY = year;
        playerInfo.birM = month;
        playerInfo.birD = day;
        playerInfo.birH = hour;
    }
    public void SetMingsha(string mingsha_0, string mingsha_1, string mingsha_2, string mingsha_3, string mingsha_4)
    {
        mingshaInfo.mingsha_0 = mingsha_0;
        mingshaInfo.mingsha_1 = mingsha_1;
        mingshaInfo.mingsha_2 = mingsha_2;
        mingshaInfo.mingsha_3 = mingsha_3;
        mingshaInfo.mingsha_4 = mingsha_4;
    }

    public void SetCurMingshaEvent(string curEvent)
    {
        curMingshaEvent.mingsha_Event = curEvent;
    }

    public void ModifyDaode(int amount) => attributeInfo.daode = Mathf.Clamp(attributeInfo.daode + amount, 0, 100);
    public void ModifyChushi(int amount) => attributeInfo.chushi = Mathf.Clamp(attributeInfo.chushi + amount, 0, 100);
    public void ModifyRongmao(int amount) => attributeInfo.rongmao = Mathf.Clamp(attributeInfo.rongmao + amount, 0, 100);
    public void ModifyWencai(int amount) => attributeInfo.wencai = Mathf.Clamp(attributeInfo.wencai + amount, 0, 100);
    public void ModifyTipo(int amount) => attributeInfo.tipo = Mathf.Clamp(attributeInfo.tipo + amount, 0, 100);
    public void ModifyMingqi(int amount) => attributeInfo.mingqi = Mathf.Clamp(attributeInfo.mingqi + amount, 0, 100);

    public void ModifyDaodeChange(int amount) => attributeChange.daode_change = amount;
    public void ModifyChushiChange(int amount) => attributeChange.chushi_change = amount;
    public void ModifyRongmaoChange(int amount) => attributeChange.rongmao_change = amount;
    public void ModifyWencaiChange(int amount) => attributeChange.wencai_change = amount;
    public void ModifyTipoChange(int amount) => attributeChange.tipo_change = amount;
    public void ModifyMingqiChange(int amount) => attributeChange.mingqi_change = amount;

    public void ModifyFourSing(int amount) => skillInfo.four_Sing += amount;
    public void ModifyFourChant(int amount) => skillInfo.four_Chant += amount;
    public void ModifyFourDo(int amount) => skillInfo.four_Do += amount;
    public void ModifyFourFight(int amount) => skillInfo.four_Fight += amount;
    public void ModifyFiveHand(int amount) => skillInfo.five_Hand += amount;
    public void ModifyFiveEye(int amount) => skillInfo.five_Eye += amount;
    public void ModifyFiveBody(int amount) => skillInfo.five_Body += amount;
    public void ModifyFiveMagic(int amount) => skillInfo.five_Magic += amount;
    public void ModifyFiveFoot(int amount) => skillInfo.five_Foot += amount;

    private void Awake()
    {
        playerInfo = new PlayerInfo();
        mingshaInfo = new MingshaInfo();
        curMingshaEvent = new CurMingshaEvent();
        attributeInfo = new AttributeInfo();
        attributeChange = new AttributeChange();
        skillInfo = new SkillInfo();

        ResetPlayerAge();
        ResetCurEvent();
        ResetAttributes();
        ResetSkills();
        Year = 1;
        season = Seasons.spring;
    }

    public void ResetPlayerAge()
    {
        playerInfo.age = 7;
    }

    public void ResetCurEvent()
    {
        curMingshaEvent.mingsha_Event = "";
    }

    public void ResetAttributes()
    {
        attributeInfo.daode = 0;
        attributeInfo.chushi = 0;
        attributeInfo.rongmao = 0;
        attributeInfo.wencai = 0;
        attributeInfo.tipo = 0;
        attributeInfo.mingqi = 0;
    }

    public void ResetSkills()
    {
        skillInfo.four_Sing = 0;
        skillInfo.four_Chant = 0;
        skillInfo.four_Do = 0;
        skillInfo.four_Fight = 0;
        skillInfo.five_Hand = 0;
        skillInfo.five_Eye = 0;
        skillInfo.five_Body = 0;
        skillInfo.five_Magic = 0;
        skillInfo.five_Foot = 0;
    }
}

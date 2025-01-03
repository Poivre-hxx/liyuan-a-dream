using UnityEngine;
using System;

[Serializable]
public class PlayerInfo
{
    public string name;
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
public class SkillInfo
{
    public int four_Sing;
    public int four_Chant;
    public int four_Do;
    public int four_Fight;
    public int five_Hand;
    public int five_Eye;
    public int five_Body;
    public int Five_Magic;
    public int Five_Foot;
}

public class Player : MonoBehaviour
{
    [Header("������Ϣ")]
    [SerializeField] private PlayerInfo playerInfo;

    [Header("��ɷ")]
    [SerializeField] private MingshaInfo mingshaInfo;

    [Header("����ֵ")]
    [SerializeField] private AttributeInfo attributeInfo;

    [Header("����ֵ")]
    [SerializeField] private SkillInfo skillInfo;

    // ���ʻ�����Ϣ������
    public string PlayerName => playerInfo.name;
    public string BirthDateTime => $"{playerInfo.birY}��{playerInfo.birM}��{playerInfo.birD}��{playerInfo.birH}ʱ";

    // ������ɷ
    public string Mingsha_0 => mingshaInfo.mingsha_0;
    public string Mingsha_1 => mingshaInfo.mingsha_1;
    public string Mingsha_2 => mingshaInfo.mingsha_2;
    public string Mingsha_3 => mingshaInfo.mingsha_3;

    // ����������Ϣ������
    public int Daode => attributeInfo.daode;
    public int Chushi => attributeInfo.chushi;
    public int Rongmao => attributeInfo.rongmao;
    public int Wencai => attributeInfo.wencai;
    public int Tipo => attributeInfo.tipo;
    public int Mingqi => attributeInfo.mingqi;

    // ���ʼ�����Ϣ������
    public int FourSing => skillInfo.four_Sing;
    public int FourChant => skillInfo.four_Chant;
    public int FourDo => skillInfo.four_Do;
    public int FourFight => skillInfo.four_Fight;
    public int FiveHand => skillInfo.five_Hand;
    public int FiveEye => skillInfo.five_Eye;
    public int FiveBody => skillInfo.five_Body;
    public int FiveMagic => skillInfo.Five_Magic;
    public int FiveFoot => skillInfo.Five_Foot;

    public void SetPlayerName(string newName, string teacherName)
    {
        playerInfo.name = newName;
        playerInfo.teacher = teacherName;
    }

    public void SetBirthTime(int year, int month, int day, int hour)
    {
        playerInfo.birY = year;
        playerInfo.birM = month;
        playerInfo.birD = day;
        playerInfo.birH = hour;
    }
    public void SetMingsha(string mingsha_0, string mingsha_1, string mingsha_2, string mingsha_3)
    {
        mingshaInfo.mingsha_0 = mingsha_0;
        mingshaInfo.mingsha_1 = mingsha_1;
        mingshaInfo.mingsha_2 = mingsha_2;
        mingshaInfo.mingsha_3 = mingsha_3;
    }

    public void ModifyDaode(int amount) => attributeInfo.daode += amount;
    public void ModifyChushi(int amount) => attributeInfo.chushi += amount;
    public void ModifyRongmao(int amount) => attributeInfo.rongmao += amount;
    public void ModifyWencai(int amount) => attributeInfo.wencai += amount;
    public void ModifyTipo(int amount) => attributeInfo.tipo += amount;
    public void ModifyMingqi(int amount) => attributeInfo.mingqi += amount;

    public void ModifyFourSing(int amount) => skillInfo.four_Sing += amount;
    public void ModifyFourChant(int amount) => skillInfo.four_Chant += amount;
    public void ModifyFourDo(int amount) => skillInfo.four_Do += amount;
    public void ModifyFourFight(int amount) => skillInfo.four_Fight += amount;
    public void ModifyFiveHand(int amount) => skillInfo.five_Hand += amount;
    public void ModifyFiveEye(int amount) => skillInfo.five_Eye += amount;
    public void ModifyFiveBody(int amount) => skillInfo.five_Body += amount;
    public void ModifyFiveMagic(int amount) => skillInfo.Five_Magic += amount;
    public void ModifyFiveFoot(int amount) => skillInfo.Five_Foot += amount;

    private void Awake()
    {
        ResetAttributes();
        ResetSkills();
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
        skillInfo.Five_Magic = 0;
        skillInfo.Five_Foot = 0;
    }
}

using UnityEngine;

public enum XianMuNames
{
    four_Sing,
    four_Chant,
    four_Do,
    four_Fight,
    five_Hand,
    five_Eye,
    five_Body,
    five_Magic,
    five_Foot
}

public enum Seasons
{
    spring,
    summer,
    autumn,
    winter
}

public class Settings
{
    public const int SkillAmount = 10;
}

[System.Serializable]
public class cirles
{
    public XianMuNames xianmu;
    public CircleProcess circleProcess;
    public float curFill;
    public float targetFill;
}



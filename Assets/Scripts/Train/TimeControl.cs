using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private Player player;
    public Text YearText;
    public Text SeasonText;

    private void OnEnable()
    {
        UpdateText();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        YearText.text = player.curYear.ToString();
        SeasonText.text = SeasonToChinese(player.curseason);
    }

    private string SeasonToChinese(Seasons season)
    {
        switch (season)
        {
            case Seasons.spring:
                return "´º";
            case Seasons.summer:
                return "ÏÄ";
            case Seasons.autumn:
                return "Çï";
            case Seasons.winter:
                return "¶¬";
            default:
                return "";
        }
    }
}

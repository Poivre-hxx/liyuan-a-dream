using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public GameObject triggerObject;
    public Player player;
    public Text endingText;

    private RectTransform textRectTransform;
    private float textHeight;
    private bool isScrollingComplete = false;
    private bool hasStartedScrolling = false;

    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
        textHeight = textRectTransform.rect.height;

        ResetTextPosition();
    }

    void Update()
    {
        if (triggerObject != null && triggerObject.activeSelf && !hasStartedScrolling)
        {
            hasStartedScrolling = true;
            refreshText();
        }

        if (hasStartedScrolling && !isScrollingComplete)
        {
            Vector2 position = textRectTransform.anchoredPosition;
            position.y += scrollSpeed * Time.deltaTime;
            textRectTransform.anchoredPosition = position;

            if (position.y > textHeight)
            {
                isScrollingComplete = true;
            }
        }
    }

    private void ResetTextPosition()
    {
        Vector2 startPos = textRectTransform.anchoredPosition;
        startPos.y = -textHeight - 300;
        textRectTransform.anchoredPosition = startPos;
        isScrollingComplete = false;
        hasStartedScrolling = false;
    }

    void refreshText()
    {
        int curYear = player.BirthDateYear + player.Year;
        endingText.text = curYear + "年，年仅 11 岁的" + player.PlayerName + "已然在京剧武生行当崭露头角。\n" +
            "舞台之上，他身姿矫健，一招一式尽显功力，\n" +
            "高难度的动作如鹞子翻身、旋子飞脚都做得干脆利落，\n" +
            "台下观众掌声雷动，喝彩声不绝于耳。\n" +
            player.Teacher + "站在幕后，看着爱徒的精彩表演，眼中满是欣慰与自豪。\n" +
            "他深知" + player.PlayerName + "这四年付出了多少汗水，\n" +
            "无数个日夜，当别人还在睡梦中，" + player.PlayerName + "就已在练功房压腿、下腰、练身段；\n" +
            "夏日酷暑，汗水湿透衣衫，他未曾停下；\n" +
            "寒冬腊月，手脚冻得红肿，依旧咬牙坚持。\n" +
            "而" + player.PlayerName + "，望着台下如痴如醉的观众，心中满是对京剧艺术的热爱与敬畏。\n" +
            "此刻，他不仅是在展示自己的技艺，更是在传承" + player.Teacher + "所授的京剧精魂。\n" +
            "他暗暗发誓，这一生都要扎根于京剧舞台，\n" +
            "将武生这一行当发扬光大，让更多人领略京剧的独特魅力，\n" +
            "让这门传统艺术在时代的洪流中永绽光芒，无论前路多少艰难险阻，他都将矢志不渝。\n" +
            "此后，" + player.PlayerName + " 带着这份决心，继续书写着他与京剧的传奇篇章，\n" +
            "成为了当地百姓口中津津乐道的 “京剧神童”。\n" +
            "\n\n\n" + "全剧终";
    }
}
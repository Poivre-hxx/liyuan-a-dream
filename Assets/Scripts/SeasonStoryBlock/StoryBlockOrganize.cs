using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBlockOrganize : MonoBehaviour
{
    public GameObject StoryBlock;

    [Header("»¬¶¯¿ò")]
    public Transform content;
    public ScrollRect scrollRect;
    public float Padding;
    private RectTransform contentRect;
    private float LowHight = 0;

    private void Start()
    {
        contentRect = gameObject.GetComponent<RectTransform>();
    }

    public void CreatStoryBlock(string TextContent)
    {
        GameObject block = Instantiate(StoryBlock,content);

        TextBackAlign textBackAlign = block.GetComponent<TextBackAlign>();
        textBackAlign.StoryText.text = TextContent;
        textBackAlign.AlignBack();

        RectTransform rect = block.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -1 * LowHight);
        LowHight += textBackAlign.BackTitlePadding + textBackAlign.TextBackPadding * 2 + textBackAlign.StoryText.preferredHeight+Padding;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, LowHight);
        scrollRect.verticalNormalizedPosition = 0f;
    }
}

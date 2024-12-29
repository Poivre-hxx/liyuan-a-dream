using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBackAlign : MonoBehaviour
{
    public Image StoryBack;
    public Text StoryText;

    public Image BackGround;

    public RectTransform StoryBackRect;
    public RectTransform BackRect;

    private float MaxWidth;

    [Header("边界数值调整")]
    public float TextBackPadding;
    public float BackTitlePadding;
    

    private void Start()
    {
        //StoryBackRect = StoryBack.GetComponent<RectTransform>();
        //BackRect = BackGround.GetComponent<RectTransform>();
        MaxWidth = StoryText.rectTransform.rect.width;
        AlignBack();
    }

    private void Update()
    {
        //AlignBack();
    }

    public void AlignBack()
    {
        float curWidth = StoryText.preferredWidth;
        float curHeight = StoryText.preferredHeight;

        if (curWidth < MaxWidth)
        {
            StoryBackRect.sizeDelta = new Vector2(curWidth + TextBackPadding*2, curHeight+ TextBackPadding * 2);
            BackRect.sizeDelta = new Vector2 (curWidth+TextBackPadding * 2, curHeight + TextBackPadding * 2+ BackTitlePadding); 
        }
        else
        {
            StoryBackRect.sizeDelta = new Vector2(MaxWidth + TextBackPadding * 2, curHeight + TextBackPadding * 2);
            BackRect.sizeDelta = new Vector2(MaxWidth + TextBackPadding * 2, curHeight + TextBackPadding * 2 + BackTitlePadding);
        }
    }
}

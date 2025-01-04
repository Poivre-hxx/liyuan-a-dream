using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnPaiControl : MonoBehaviour
{
    public List<GameObject> KuangObjects = new();
    public List<Sprite> LableImage = new();
    [SerializeField]private List<SpriteRenderer> renderers = new();    

    [Header("需要显示的标签")]
    [SerializeField] private Sprite four_Sing;
    [SerializeField] private Sprite four_Chant;
    [SerializeField] private Sprite four_Do;
    [SerializeField] private Sprite four_Fight;
    [SerializeField] private Sprite five_Hand;
    [SerializeField] private Sprite five_Eye;
    [SerializeField] private Sprite five_Body;
    [SerializeField] private Sprite five_Magic;
    [SerializeField] private Sprite five_Foot;

    private void Start()
    {
        for(int i = 0; i < KuangObjects.Count; i++)
        {
            SpriteRenderer cur = KuangObjects[i].GetComponentInChildren<SpriteRenderer>();
            cur.sprite = null;
            renderers.Add(cur);
        }
    }

    public void AddXianMu(XianMuNames name)
    {
        if (LableImage.Count < 8)
        {
            if (name == XianMuNames.five_Hand)
            {
                LableImage.Add(five_Hand);
            }
            else if (name == XianMuNames.five_Foot)
            {
                LableImage.Add(five_Foot);
            }
            else if (name == XianMuNames.four_Fight)
            {
                LableImage.Add(four_Fight);
            }
            else if (name == XianMuNames.four_Sing)
            {
                LableImage.Add(four_Sing);
            }
            else if (name == XianMuNames.four_Chant)
            {
                LableImage.Add(four_Chant);
            }
            else if (name == XianMuNames.four_Do)
            {
                LableImage.Add(four_Do);
            }
            else if (name == XianMuNames.five_Body)
            {
                LableImage.Add(five_Body);
            }
            else if (name == XianMuNames.five_Eye)
            { 
                LableImage.Add(five_Eye);
            }
            else if (name == XianMuNames.five_Magic)
            {
                LableImage.Add(five_Magic);
            }
            UpdateLayer();
        }
    }

    public void UpdateLayer()
    {
        for(int i = 0; i < LableImage.Count; i++)
        {
            if(LableImage[i] != null)
            {
                renderers[i].sprite = LableImage[i];
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KebanSelect : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    [SerializeField] private AnPaiControl anPaiControl;

    [Header("选择要显示的属性")]
    [SerializeField] private bool four_Sing;
    [SerializeField] private bool four_Chant;
    [SerializeField] private bool four_Do;
    [SerializeField] private bool four_Fight;
    [SerializeField] private bool five_Hand;
    [SerializeField] private bool five_Eye;
    [SerializeField] private bool five_Body;
    [SerializeField] private bool Five_Magic;
    [SerializeField] private bool Five_Foot;

    void Start()
    {
        SetupCollider();
    }

    private void SetupCollider()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCollider2D.isTrigger = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        }
    }

    private void OnMouseDown()
    {
        if(anPaiControl != null && anPaiControl.LableImage.Count < 8)
        {
            if (four_Sing)
            {
                anPaiControl.AddXianMu(XianMuNames.four_Sing);
            }
            else if (four_Chant)
            {
                anPaiControl.AddXianMu(XianMuNames.four_Chant);
            }
            else if (four_Do)
            {
                anPaiControl.AddXianMu(XianMuNames.four_Do);
            }
            else if (four_Fight)
            {
                anPaiControl.AddXianMu(XianMuNames.four_Fight);
            }
            else if (five_Hand)
            {
                anPaiControl.AddXianMu(XianMuNames.five_Hand);
            }
            else if (five_Eye)
            {
                anPaiControl.AddXianMu(XianMuNames.five_Eye);
            }
            else if (five_Body)
            {
                anPaiControl.AddXianMu(XianMuNames.five_Body);
            }
            else if (Five_Magic)
            {
                anPaiControl.AddXianMu(XianMuNames.five_Magic);
            }
            else if (Five_Foot)
            {
                Debug.Log("choose five_Foot");
                anPaiControl.AddXianMu(XianMuNames.five_Foot);
            }
        }
        else
        {
            Debug.Log("当月安排已满");
        }

    }

}

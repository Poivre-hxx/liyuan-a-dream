using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnPai : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Player player;

    private BoxCollider2D boxCollider2D;
    

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
        if (targetObject != null)
        {
            //ʱ������
            TimeControl();
            gameControl.SetObjectVisible(targetObject);
            Debug.Log($"�Ѿ��л�Ϊ: {targetObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning("ʧ��");
        }
    }

    public void TimeControl()
    {
        if(player.season == Seasons.winter)
        {
            player.Year++;
            player.season = Seasons.spring;
        }
        else
        {
            player.season++;
        }
    }
}

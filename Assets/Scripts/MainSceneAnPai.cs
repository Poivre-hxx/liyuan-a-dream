using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneAnPai : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private GameObject targetScene;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameControl gameControl;

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
        if(player.curYear < 6)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(!targetObject.activeSelf);
                Debug.Log($"ÒÑ¾­ÇÐ»»Îª: {targetObject.activeSelf}");
            }
            else
            {
                Debug.LogWarning("Ê§°Ü");
            }
        }
        else
        {
            gameControl.SetObjectVisible(targetScene);
        }
       
    }
}

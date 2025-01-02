using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class SceneTrans : MonoBehaviour
{
    private GameControl gameControl;
    [SerializeField]
    private GameObject targetObject;

    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        gameControl = FindObjectOfType<GameControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        }
    }

    private void OnMouseDown()
    {

        if (targetObject != null && gameControl != null)
        {
            gameControl.SetObjectVisible(targetObject);
            Debug.Log($"Setting {targetObject.name} visible");
        }
        else
        {
            Debug.Log($"Click failed - TargetObject: {targetObject != null}, GameControl: {gameControl != null}");
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log($"Mouse entered {gameObject.name}");
    }
}

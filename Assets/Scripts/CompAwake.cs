using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompAwake : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

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
            targetObject.SetActive(!targetObject.activeSelf);
            //Debug.Log($"ÒÑ¾­ÇÐ»»Îª: {targetObject.activeSelf}");
        }
        else
        {
            //Debug.LogWarning("Ê§°Ü");
        }
    }
}

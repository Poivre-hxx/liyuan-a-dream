using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    private GameControl gameControl;
    [SerializeField]
    private GameObject targetObject;
    [SerializeField] private Player player;

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
            if(player.AttributeResponse != null)
            {
                JObject response = player.AttributeResponse;
                player.AttributeResponse = null;
                var attrs = response["attributes"];

                //–ﬁ∏ƒÕÊº“ Ù–‘
                //player.ModifyDaode(attrs["daode"].Value<int>());
                //player.ModifyChushi(attrs["chushi"].Value<int>());
                //player.ModifyRongmao(attrs["rongmao"].Value<int>());
                //player.ModifyWencai(attrs["wencai"].Value<int>());
                //player.ModifyTipo(attrs["tipo"].Value<int>());
                //player.ModifyMingqi(attrs["mingqi"].Value<int>());
                player.AttributeResponse = null;
            }
            
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

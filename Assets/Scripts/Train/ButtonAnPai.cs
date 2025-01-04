using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnPai : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Player player;

    //public Dictionary<float, float> cirleChange = new();
    public List<cirles> changecirles = new();

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

            ////时间增加
            ////TimeControl();
            //gameControl.SetObjectVisible(targetObject);
            //Debug.Log($"已经切换为: {targetObject.activeSelf}");
            StartCoroutine(TrainChange());
        }
        else
        {
            Debug.LogWarning("失败");
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

    IEnumerator TrainChange()
    {
        foreach(var cirle in changecirles)
        {
            yield return cirle.circleProcess.StartProgress(cirle.curFill, cirle.targetFill);
        }

        yield return new WaitForSeconds(0.5f);

        gameControl.SetObjectVisible(targetObject);

        TimeControl();

        foreach(var cirle in changecirles)
        {
            cirle.curFill = cirle.targetFill;   
        }

        //生成随机事件
    }
}

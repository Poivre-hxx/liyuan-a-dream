using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBorder : MonoBehaviour
{
    void Start()
    {
        // ���Outline���
        Outline outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(4, 4);
    }
}

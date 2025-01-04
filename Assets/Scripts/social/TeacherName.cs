using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherName : MonoBehaviour
{
    [SerializeField] private Player player;
    public Text NameText;

    private void Start()
    {
        NameText.text = player.Teacher;
    }

}

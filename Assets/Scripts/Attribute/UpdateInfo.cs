using UnityEngine;
using UnityEngine.UI;

public class UpdateInfo : MonoBehaviour
{
    public Text Infotext;
    public Player player;

    public void Update()
    {
        int curAge = player.curYear + 6;
        Infotext.text = player.PlayerName + " " + curAge + "Ëê";
    }
}
using UnityEngine;
using UnityEngine.UI;

public class GetStory : MonoBehaviour
{
    public Text storyText;
    public Player player;
    private string curMingsha;
    private void UpdateCurrentMingsha()
    {
        int playerAge = player.curYear;

        switch (playerAge)
        {
            case 1:
                curMingsha = player.Mingsha_0;
                break;
            case 2:
                curMingsha = player.Mingsha_1;
                break;
            case 3:
                curMingsha = player.Mingsha_2;
                break;
            case 4:
                curMingsha = player.Mingsha_3;
                break;
            case 5:
                curMingsha = player.Mingsha_4;
                break;
            default:
                curMingsha = "超过年龄范围";
                break;
        }
    }

    public void Update()
    {
        UpdateCurrentMingsha();
        Debug.Log(curMingsha);
        if (storyText != null && player != null)
        {
            storyText.text = "爱徒今年的命煞为：" + curMingsha + '\n';
            storyText.text += player.Mingsha_event;
        }
    }
}

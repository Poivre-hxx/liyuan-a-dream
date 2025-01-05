using UnityEngine;
using UnityEngine.UI;

public class GetStory : MonoBehaviour
{
    public Text storyText;
    public Player player;

    public void UpdateStoryText()
    {
        if (storyText != null && player != null)
        {
            storyText.text = player.Mingsha_event;
        }
    }
}

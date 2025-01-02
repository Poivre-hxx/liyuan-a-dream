using UnityEngine;
using UnityEngine.UI;

public class SetAttribute : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField teacherInput;
    [SerializeField] private InputField yearInput;
    [SerializeField] private InputField monthInput;
    [SerializeField] private InputField dayInput;
    [SerializeField] private InputField hourInput;

    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        }
        boxCollider2D.isTrigger = true;
    }

    private void OnMouseDown()
    {
        if (player != null)
        {
            if (nameInput != null && !string.IsNullOrEmpty(nameInput.text))
            {
                string newName = nameInput.text.Replace("\n", "");
                string teacherName = teacherInput.text.Replace("\n", "");
                player.SetPlayerName(newName, teacherName);
            }

            int year = ParseVerticalNumber(yearInput.text);
            int month = ParseVerticalNumber(monthInput.text);
            int day = ParseVerticalNumber(dayInput.text);
            int hour = ParseVerticalNumber(hourInput.text);

            player.SetBirthTime(year, month, day, hour);

            Debug.Log($"已更新玩家信息：{player.PlayerName}, 出生时间：{player.BirthDateTime}");
        }
    }

    private int ParseVerticalNumber(string verticalText)
    {
        if (string.IsNullOrEmpty(verticalText))
            return 0;

        string cleanText = verticalText.Replace("\n", "");

        int result;
        if (int.TryParse(cleanText, out result))
        {
            return result;
        }
        return 0;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputVertical : MonoBehaviour
{
    private InputField inputField;
    private Text inputText;
    private Text placeholderText;
    private string rawInput = ""; 

    void Start()
    {
        inputField = GetComponent<InputField>();

        if (inputField != null)
        {
            inputText = inputField.textComponent;
            placeholderText = transform.Find("Placeholder")?.GetComponent<Text>();

            if (inputText != null)
            {
                inputText.alignment = TextAnchor.MiddleCenter;
            }

            if (placeholderText != null)
            {
                placeholderText.alignment = TextAnchor.MiddleCenter;
            }

            inputField.onEndEdit.AddListener(OnEndEdit);
        }
    }

    private void OnEndEdit(string text)
    {
        if (text != rawInput)
        {
            rawInput = text.Replace("\n", "");

            MakeVertical();
        }
    }

    private void MakeVertical()
    {
        if (string.IsNullOrEmpty(rawInput))
        {
            inputField.text = "";
            return;
        }

        string verticalText = "";
        for (int i = 0; i < rawInput.Length; i++)
        {
            verticalText += rawInput[i];
            if (i < rawInput.Length - 1)
            {
                verticalText += "\n";
            }
        }

        inputField.text = verticalText;
    }

    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnEndEdit);
        }
    }
}

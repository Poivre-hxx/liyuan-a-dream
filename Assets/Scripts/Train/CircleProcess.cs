using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
public class CircleProcess : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("Progress Bar Settings")]
    [SerializeField] private Image progressImage;
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("选择要显示的属性")]
    [SerializeField] private bool show_four_Sing;
    [SerializeField] private bool show_four_Chant;
    [SerializeField] private bool show_four_Do;
    [SerializeField] private bool show_four_Fight;
    [SerializeField] private bool show_five_Hand;
    [SerializeField] private bool show_five_Eye;
    [SerializeField] private bool show_five_Body;
    [SerializeField] private bool show_Five_Magic;
    [SerializeField] private bool show_Five_Foot;

    private float currentFillAmount = 0f;
    private float targetFillAmount = 0f;

    private void Start()
    {
        if (progressImage != null)
        {
            progressImage.type = Image.Type.Filled;
            progressImage.fillMethod = Image.FillMethod.Radial360;
            progressImage.fillOrigin = (int)Image.Origin360.Top;
            progressImage.fillClockwise = true;
            progressImage.fillAmount = 0f; 
        }

    }

    private void Update()
    {
        UpdateProgress();
        if (progressImage != null && Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            progressImage.fillAmount = currentFillAmount;

            Debug.Log($"Current Fill Amount: {currentFillAmount}, Target Fill Amount: {targetFillAmount}");
        }
    }

    public void UpdateProgress()
    {
        if (player == null || progressImage == null) return;

        float totalValue = 0f;
        int selectedCount = 0;

        if (show_four_Sing)
        {
            totalValue += player.FourSing;
            selectedCount++;
            Debug.Log($"Four Sing: {player.FourSing}");
        }
        if (show_four_Chant)
        {
            totalValue += player.FourChant;
            selectedCount++;
            Debug.Log($"Four Chant: {player.FourChant}");
        }
        if (show_four_Do)
        {
            totalValue += player.FourDo;
            selectedCount++;
            Debug.Log($"Four Do: {player.FourDo}");
        }
        if (show_four_Fight)
        {
            totalValue += player.FourFight;
            selectedCount++;
            Debug.Log($"Four Fight: {player.FourFight}");
        }
        if (show_five_Hand)
        {
            totalValue += player.FiveHand;
            selectedCount++;
            Debug.Log($"Five Hand: {player.FiveHand}");
        }
        if (show_five_Eye)
        {
            totalValue += player.FiveEye;
            selectedCount++;
            Debug.Log($"Five Eye: {player.FiveEye}");
        }
        if (show_five_Body)
        {
            totalValue += player.FiveBody;
            selectedCount++;
            Debug.Log($"Five Body: {player.FiveBody}");
        }
        if (show_Five_Magic)
        {
            totalValue += player.FiveMagic;
            selectedCount++;
            Debug.Log($"Five Magic: {player.FiveMagic}");
        }
        if (show_Five_Foot)
        {
            totalValue += player.FiveFoot;
            selectedCount++;
            Debug.Log($"Five Foot: {player.FiveFoot}");
        }

        if (selectedCount > 0)
        {
            float averageValue = totalValue / selectedCount;
            targetFillAmount = averageValue / maxValue;
            targetFillAmount = Mathf.Clamp01(targetFillAmount);

            Debug.Log($"Total Value: {totalValue}, Count: {selectedCount}, Average: {averageValue}, Target Fill: {targetFillAmount}");
        }
        else
        {
            targetFillAmount = 0f;
            Debug.Log("No attributes selected, setting target fill to 0");
        }

        if (targetFillAmount == 0f)
        {
            currentFillAmount = 0f;
            progressImage.fillAmount = 0f;
        }
    }
}

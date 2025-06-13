using System.Collections;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
public class CircleProcess : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ButtonAnPai buttonAnPai;

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

    //private float currentFillAmount = 0f;
    private float targetFillAmount = 0f;

    private void Awake()
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

    private void OnEnable()
    {
        SetFillAmount();
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// 第二回合之后的数据历史记录
    /// </summary>
    public void SetFillAmount()
    {
        foreach (var cirle in buttonAnPai.changecirles)
        {
            switch (cirle.xianmu)
            {
                case XianMuNames.four_Sing:
                    if (show_four_Sing) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.four_Chant:
                    if (show_four_Chant) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.four_Do:
                    if (show_four_Do) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.four_Fight:
                    if (show_four_Fight) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.five_Magic:
                    if (show_Five_Magic) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.five_Hand:
                    if (show_five_Hand) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.five_Eye:
                    if (show_five_Eye) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.five_Foot:
                    if (show_Five_Foot) progressImage.fillAmount = cirle.curFill;
                    return;
                case XianMuNames.five_Body:
                    if (show_five_Body) progressImage.fillAmount = cirle.curFill;
                    return;
                default:
                    return;


            }
        }
    }

    private void Update()
    {

    }

    public void Progress()
    {
        if (player == null || progressImage == null) return;

        if (show_four_Sing)
        {
            player.ModifyFourSing(Settings.SkillAmount);
            //Debug.Log($"Four Sing: {player.FourSing}");
            targetFillAmount = player.FourSing / maxValue;
            ModifycirlesList(XianMuNames.four_Sing, targetFillAmount);
        }
        else if (show_four_Chant)
        {
            player.ModifyFourChant(Settings.SkillAmount);
            //Debug.Log($"Four Chant: {player.FourChant}");
            targetFillAmount = player.FourChant / maxValue;
            ModifycirlesList(XianMuNames.four_Chant, targetFillAmount);
        }
        else if (show_four_Do)
        {
            player.ModifyFourDo(Settings.SkillAmount);
            //Debug.Log($"Four Do: {player.FourDo}");
            targetFillAmount = player.FourDo / maxValue;
            ModifycirlesList(XianMuNames.four_Do, targetFillAmount);
        }
        else if (show_four_Fight)
        {
            player.ModifyFourFight(Settings.SkillAmount);
            //Debug.Log($"Four Fight: {player.FourFight}");
            targetFillAmount = player.FourFight / maxValue;
            ModifycirlesList(XianMuNames.four_Fight, targetFillAmount);
        }
        else if (show_five_Hand)
        {
            player.ModifyFiveHand(Settings.SkillAmount);
            //Debug.Log($"Five Hand: {player.FiveHand}");
            targetFillAmount = player.FiveHand / maxValue;
            ModifycirlesList(XianMuNames.five_Hand, targetFillAmount);
        }
        else if (show_five_Eye)
        {
            player.ModifyFiveEye(Settings.SkillAmount);
            //Debug.Log($"Five Eye: {player.FiveEye}");
            targetFillAmount = player.FiveEye / maxValue;
            ModifycirlesList(XianMuNames.five_Eye, targetFillAmount);
        }
        else if (show_five_Body)
        {
            player.ModifyFiveBody(Settings.SkillAmount);
            //Debug.Log($"Five Body: {player.FiveBody}");
            targetFillAmount = player.FiveBody / maxValue;
            ModifycirlesList(XianMuNames.five_Body, targetFillAmount);
        }
        else if (show_Five_Magic)
        {
            player.ModifyFiveMagic(Settings.SkillAmount);
            //Debug.Log($"Five Magic: {player.FiveMagic}");
            targetFillAmount = player.FiveMagic / maxValue;
            ModifycirlesList(XianMuNames.five_Magic, targetFillAmount);
        }
        else if (show_Five_Foot)
        {
            player.ModifyFiveFoot(Settings.SkillAmount);
            //Debug.Log($"Five Foot: {player.FiveFoot}");
            targetFillAmount = player.FiveFoot / maxValue;
            ModifycirlesList(XianMuNames.five_Foot, targetFillAmount);
        }
        
    }

    private void ModifycirlesList(XianMuNames xianmu,float tar)
    {
        foreach (var cirle in buttonAnPai.changecirles)
        {
            if (cirle.xianmu == xianmu)
            {
                cirle.targetFill = tar;
                break; 
            }
        }
    }



    public IEnumerator StartProgress(float cur,float tar)
    {
        while(progressImage != null && Mathf.Abs(cur - tar) > 0.001f)
        {
            cur = Mathf.Lerp(cur, tar, Time.deltaTime * smoothSpeed);
            progressImage.fillAmount = cur;
            yield return null;
            //Debug.Log($"Current Fill Amount: {cur}, Target Fill Amount: {tar}");
        }
        progressImage.fillAmount = tar;
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
            //Debug.Log($"Four Sing: {player.FourSing}");
        }
        if (show_four_Chant)
        {
            totalValue += player.FourChant;
            selectedCount++;
            //Debug.Log($"Four Chant: {player.FourChant}");
        }
        if (show_four_Do)
        {
            totalValue += player.FourDo;
            selectedCount++;
            //Debug.Log($"Four Do: {player.FourDo}");
        }
        if (show_four_Fight)
        {
            totalValue += player.FourFight;
            selectedCount++;
            //Debug.Log($"Four Fight: {player.FourFight}");
        }
        if (show_five_Hand)
        {
            totalValue += player.FiveHand;
            selectedCount++;
            //Debug.Log($"Five Hand: {player.FiveHand}");
        }
        if (show_five_Eye)
        {
            totalValue += player.FiveEye;
            selectedCount++;
            //Debug.Log($"Five Eye: {player.FiveEye}");
        }
        if (show_five_Body)
        {
            totalValue += player.FiveBody;
            selectedCount++;
            //Debug.Log($"Five Body: {player.FiveBody}");
        }
        if (show_Five_Magic)
        {
            totalValue += player.FiveMagic;
            selectedCount++;
            //Debug.Log($"Five Magic: {player.FiveMagic}");
        }
        if (show_Five_Foot)
        {
            totalValue += player.FiveFoot;
            selectedCount++;
            //Debug.Log($"Five Foot: {player.FiveFoot}");
        }

        if (selectedCount > 0)
        {
            float averageValue = totalValue / selectedCount;
            targetFillAmount = averageValue / maxValue;
            targetFillAmount = Mathf.Clamp01(targetFillAmount);

            //Debug.Log($"Total Value: {totalValue}, Count: {selectedCount}, Average: {averageValue}, Target Fill: {targetFillAmount}");
        }
        else
        {
            targetFillAmount = 0f;
            //Debug.Log("No attributes selected, setting target fill to 0");
        }

        if (targetFillAmount == 0f)
        {
            //currentFillAmount = 0f;
            progressImage.fillAmount = 0f;
        }
    }
}

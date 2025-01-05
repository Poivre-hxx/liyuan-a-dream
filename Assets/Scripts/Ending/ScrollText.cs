using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public GameObject triggerObject;
    public Player player;
    public Text endingText;

    private RectTransform textRectTransform;
    private float textHeight;
    private bool isScrollingComplete = false;
    private bool hasStartedScrolling = false;

    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
        textHeight = textRectTransform.rect.height;

        ResetTextPosition();
    }

    void Update()
    {
        if (triggerObject != null && triggerObject.activeSelf && !hasStartedScrolling)
        {
            hasStartedScrolling = true;
            refreshText();
        }

        if (hasStartedScrolling && !isScrollingComplete)
        {
            Vector2 position = textRectTransform.anchoredPosition;
            position.y += scrollSpeed * Time.deltaTime;
            textRectTransform.anchoredPosition = position;

            if (position.y > textHeight)
            {
                isScrollingComplete = true;
            }
        }
    }

    private void ResetTextPosition()
    {
        Vector2 startPos = textRectTransform.anchoredPosition;
        startPos.y = -textHeight - 300;
        textRectTransform.anchoredPosition = startPos;
        isScrollingComplete = false;
        hasStartedScrolling = false;
    }

    void refreshText()
    {
        int curYear = player.BirthDateYear + player.Year;
        endingText.text = curYear + "�꣬��� 11 ���" + player.PlayerName + "��Ȼ�ھ��������е�ո¶ͷ�ǡ�\n" +
            "��̨֮�ϣ������˽ý���һ��һʽ���Թ�����\n" +
            "���ѶȵĶ��������ӷ������ӷɽŶ����øɴ����䣬\n" +
            "̨�¹��������׶����Ȳ��������ڶ���\n" +
            player.Teacher + "վ��Ļ�󣬿��Ű�ͽ�ľ��ʱ��ݣ�����������ο���Ժ���\n" +
            "����֪" + player.PlayerName + "�����긶���˶��ٺ�ˮ��\n" +
            "��������ҹ�������˻���˯���У�" + player.PlayerName + "������������ѹ�ȡ�����������Σ�\n" +
            "���տ����ˮʪ͸��������δ��ͣ�£�\n" +
            "�������£��ֽŶ��ú��ף�����ҧ����֡�\n" +
            "��" + player.PlayerName + "������̨���������Ĺ��ڣ��������ǶԾ����������Ȱ��뾴η��\n" +
            "�˿̣�����������չʾ�Լ��ļ��գ������ڴ���" + player.Teacher + "���ڵľ��羫�ꡣ\n" +
            "���������ģ���һ����Ҫ�����ھ�����̨��\n" +
            "��������һ�е��������ø��������Ծ���Ķ���������\n" +
            "�����Ŵ�ͳ������ʱ���ĺ�����������â������ǰ·���ټ������裬������ʸ־���塣\n" +
            "�˺�" + player.PlayerName + " ������ݾ��ģ�������д�����뾩��Ĵ���ƪ�£�\n" +
            "��Ϊ�˵��ذ��տ��н���ֵ��� ��������ͯ����\n" +
            "\n\n\n" + "ȫ����";
    }
}
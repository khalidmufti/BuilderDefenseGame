using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }   

    [SerializeField] private RectTransform m_canvasRectTransform;
    private TextMeshProUGUI m_TextMeshPro;
    private RectTransform m_RectTransform;
    private RectTransform m_BackgroundRectTransform;
    private TooltipTimer m_Timer;

    private void Awake()
    {
        Instance = this;     
        m_RectTransform = GetComponent<RectTransform>();
        m_TextMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        m_BackgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        HandleFollowMouse();

        if (m_Timer != null)
        {
            m_Timer.Timer -= Time.deltaTime;
            if (m_Timer.Timer <= 0)
            {
                Hide();
            }
        }

    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / m_canvasRectTransform.localScale.x;  //can use y or z as uniform

        if (anchoredPosition.x + m_BackgroundRectTransform.rect.width > m_canvasRectTransform.rect.width)
        {
            anchoredPosition.x = m_canvasRectTransform.rect.width - m_BackgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + m_BackgroundRectTransform.rect.height > m_canvasRectTransform.rect.height)
        {
            anchoredPosition.y = m_canvasRectTransform.rect.height - m_BackgroundRectTransform.rect.height;
        }


        m_RectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        m_TextMeshPro.SetText(tooltipText);
        m_TextMeshPro.ForceMeshUpdate();

        Vector2 textSize = m_TextMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        m_BackgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show (string tooltipExt, TooltipTimer timer = null)
    {
        m_Timer = timer;
        gameObject.SetActive(true);
        SetText(tooltipExt);
        HandleFollowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float Timer;
    }

}

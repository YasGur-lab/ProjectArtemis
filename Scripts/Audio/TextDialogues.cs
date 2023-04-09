using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDialogues : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMesh;
    [SerializeField] string m_Text = "";

    [SerializeField] TextMeshProUGUI m_OptionTextMesh;
    [SerializeField] string m_OptionText = "";

    private float m_MaxTimerForOptionText = 2.0f;
    private float m_TimerForOptionText;
    private bool m_IsOptionTextOn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_TextMesh.text = m_Text;

        m_OptionTextMesh.text = m_OptionText;

        if (m_IsOptionTextOn)
        {
            m_TimerForOptionText += Time.deltaTime;

            if (m_TimerForOptionText > m_MaxTimerForOptionText)
            {
                m_OptionText = "";
                m_TimerForOptionText = 0.0f;
                m_IsOptionTextOn = false;
            }
        }
    }

    public void SetText(string text)
    {
        m_Text = text;
    }

    public void SetOptionText(string text)
    {
        m_IsOptionTextOn = true;
        m_OptionText = text;
    }
}

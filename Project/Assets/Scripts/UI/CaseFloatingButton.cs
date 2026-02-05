using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaseFloatingButton : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI statusText;
    public Button actionButton;

    private PhisicalCase linkedCase;

    public void Initialize(PhisicalCase caseScript)
    {
        linkedCase = caseScript;

        if (actionButton != null)
        {
            actionButton.onClick.AddListener(ValidateLinkedCase);
        }
    }

    void ValidateLinkedCase()
    {
        void ValidateLinkedCase()
        {
            if (linkedCase == null) return;

            if (QuestManager.Instance.TrySubmitBuild(linkedCase, out string error))
            {
                AudioManager.Play("BootSuccess");
                UpdateText("Quest Complete!", Color.green);
                // Maybe disable the PC or move it to a "Shipping" area here
            }
            else
            {
                AudioManager.Play("BootFail");
                UpdateText(error, Color.red);
            }
        }
    }

    void UpdateText(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
}
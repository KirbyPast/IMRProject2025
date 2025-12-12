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
        if (linkedCase == null)
        {
            UpdateText("Error: Disconnected", Color.red);
            return;
        }

        if (linkedCase.CheckCompleteness(out string missingPart))
        {
            UpdateText("Success! Booting...", Color.green);
            Debug.Log("Build Complete for " + linkedCase.name);

            // Optional: Hide button after success?
            // gameObject.SetActive(false); 
        }
        else
        {
            UpdateText(missingPart, Color.red);
            Debug.Log("Incomplete: " + missingPart);
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
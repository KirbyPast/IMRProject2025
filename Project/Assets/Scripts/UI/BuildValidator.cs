using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class BuildValidator : MonoBehaviour
{
    [Header("Setup")]
    public PhisicalCase targetCase; 
    public TextMeshProUGUI feedbackText; 

    private Button myButton;

    private void Start()
    {
        myButton = GetComponent<Button>();
        
        if (myButton != null)
        {
            myButton.onClick.AddListener(ValidatePc);
        }
    }

    public void ValidatePc()
    {
        if (targetCase == null)
        {
            targetCase = FindObjectOfType<PhisicalCase>();
        }

        if (targetCase == null)
        {
            UpdateFeedback("No PC found!", Color.red);
            return;
        }


        if (targetCase.CheckCompleteness(out string missingPart))
        {
            // SUCCES
            Debug.Log("Build Complete!");
            UpdateFeedback("Success! Booting...", Color.green);
        }
        else
        {
            // EROARE
            Debug.Log("Build Incomplete: " + missingPart);
            UpdateFeedback(missingPart, Color.red);
        }
    }

    private void UpdateFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }
}
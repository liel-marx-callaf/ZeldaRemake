using UnityEngine;
using TMPro;

public class JournalController : MonoBehaviour
{
    [SerializeField] private TMP_InputField journalInputField;

    private string _savedText = ""; // Keep the text persistent between opens

    private void OnEnable()
    {
        // If you want uppercase conversion on-the-fly:
        journalInputField.onValueChanged.AddListener(ForceUppercase);
    }

    private void OnDisable()
    {
        journalInputField.onValueChanged.RemoveListener(ForceUppercase);
    }

    /// <summary>
    /// Called by GameManager when the journal is opened.
    /// </summary>
    public void OnOpenJournal()
    {
        // Restore the text from memory
        journalInputField.text = _savedText;

        // Optionally, place the cursor at the end:
        journalInputField.caretPosition = journalInputField.text.Length;
        
        // Focus the input field so player can type immediately
        journalInputField.Select();
        journalInputField.ActivateInputField();
    }

    /// <summary>
    /// Called by GameManager when the journal is closed.
    /// </summary>
    public void OnCloseJournal()
    {
        // Store whatever the user typed
        _savedText = journalInputField.text;
    }

    private void ForceUppercase(string currentText)
    {
        // Overwrite input with uppercase
        string upper = currentText.ToUpper();
        if (upper != currentText)
        {
            // This prevents the text field from losing caret position 
            // if we forcibly replace the entire string each time.
            int oldPos = journalInputField.caretPosition;
            journalInputField.SetTextWithoutNotify(upper);
            journalInputField.caretPosition = oldPos;
        }
    }
}
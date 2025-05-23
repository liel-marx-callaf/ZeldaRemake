using System.Collections;
using Audio;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea]
    [SerializeField] private string enterMessage;
    [TextArea]
    [SerializeField] private string[] reEnterMessages;
    [SerializeField] private float typingSpeed = 0.05f;

    private bool _isTyping;
    private static bool _hasEntered;

    public static void ResetHasEntered()
    {
        _hasEntered = false;
    }
    private void Start()
    {
        dialogueText.text = "";
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        yield return new WaitForSeconds(1);
        // Freeze player
        MyEvents.TogglePlayerFreeze?.Invoke();
        if (!_hasEntered)
        {
            foreach (char c in enterMessage)
            {
                dialogueText.text += c;
                AudioManager.Instance.PlaySound(transform.position, "LOZ_Text", 0.5f);
                yield return new WaitForSeconds(typingSpeed);
            }

            // Wait for user press (Space) or short delay
            // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // Unfreeze
            // MyEvents.TogglePlayerFreeze?.Invoke();
            _hasEntered = true;
        }
        else
        {
            string randomMessage = reEnterMessages[Random.Range(0, reEnterMessages.Length)];
            foreach (char c in randomMessage)
            {
                dialogueText.text += c;
                AudioManager.Instance.PlaySound(transform.position, "LOZ_Text", 0.5f);
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        MyEvents.TogglePlayerFreeze?.Invoke();
        // dialogueText.text = "";
    }
}
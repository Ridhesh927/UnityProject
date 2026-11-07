
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button restorePowerButton;
    public Button escapeButton;

    void Start()
    {
        dialogueText.text =
            "The emergency signal is coming from an abandoned research facility. " +
            "The power is failing. What should I do?";

        restorePowerButton.onClick.AddListener(RestorePower);
        escapeButton.onClick.AddListener(EscapeNow);
    }

    void RestorePower()
    {
        dialogueText.text =
            "You restore the backup generator. The lights return, " +
            "and the trapped scientists are rescued. GOOD ENDING!";

        restorePowerButton.gameObject.SetActive(false);
        escapeButton.gameObject.SetActive(false);
    }

    void EscapeNow()
    {
        dialogueText.text =
            "You escape safely, but the research facility goes dark. " +
            "The distress signal disappears. ALTERNATE ENDING!";

        restorePowerButton.gameObject.SetActive(false);
        escapeButton.gameObject.SetActive(false);
    }
}

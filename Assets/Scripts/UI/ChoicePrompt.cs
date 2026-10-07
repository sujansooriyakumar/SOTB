using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Yes/no box on the Modal layer. Put this on the prompt's root panel, under UIController.
public class ChoicePrompt : UIModal<bool>
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text yesLabel;
    [SerializeField] private TMP_Text noLabel;

    private string yesText;
    private string noText;
    private bool defaultYes;

    // Shows the prompt and calls onChosen with true for yes, false for no or cancel.
    // Returns false if a prompt is already showing.
    public static bool Show(Action<bool> onChosen, string yesText = "Yes", string noText = "No", bool defaultYes = true)
    {
        ChoicePrompt prompt = UIController.Instance.Get<ChoicePrompt>();
        if (prompt == null) return false;

        if (prompt.IsOpen)
        {
            Debug.LogWarning("ChoicePrompt is already showing.", prompt);
            return false;
        }

        UIController.Instance.Open<ChoicePrompt>(p =>
        {
            p.yesText = yesText;
            p.noText = noText;
            p.defaultYes = defaultYes;
            p.SetCallbacks(onChosen, null);
        });
        return true;
    }

    // Coroutine version, for use inside sequences:
    //   bool answer = false;
    //   yield return ChoicePrompt.Ask(result => answer = result);
    public static IEnumerator Ask(Action<bool> onChosen, string yesText = "Yes", string noText = "No", bool defaultYes = true)
    {
        bool done = false;
        bool shown = Show(result =>
        {
            done = true;
            onChosen?.Invoke(result);
        }, yesText, noText, defaultYes);

        if (shown) yield return new WaitUntil(() => done);
    }

    private void Awake()
    {
        yesButton.onClick.AddListener(() => Submit(true));
        noButton.onClick.AddListener(() => Submit(false));

        // Explicit navigation keeps the cursor on these two buttons.
        SetNavigation(yesButton, noButton);
        SetNavigation(noButton, yesButton);
    }

    protected override void OnOpen()
    {
        yesLabel.text = yesText;
        noLabel.text = noText;
    }

    protected override GameObject GetDefaultSelection() =>
        defaultYes ? yesButton.gameObject : noButton.gameObject;

    // Cancel counts as "no" rather than dismissing without an answer.
    public override bool HandleCancel()
    {
        Submit(false);
        return true;
    }

    private static void SetNavigation(Button button, Button other)
    {
        button.navigation = new Navigation
        {
            mode = Navigation.Mode.Explicit,
            selectOnLeft = other,
            selectOnRight = other,
            selectOnUp = other,
            selectOnDown = other
        };
    }
}
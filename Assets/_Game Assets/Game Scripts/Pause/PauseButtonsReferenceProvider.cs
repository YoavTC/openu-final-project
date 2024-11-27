using UnityEngine;
using UnityEngine.UI;

public class PauseButtonsReferenceProvider : MonoBehaviour
{
    private void Start()
    {
        PauseManager.Instance.AddButtonListeners(resumeButton, quitButton);
    }

    public Button resumeButton;
    public Button quitButton;
}

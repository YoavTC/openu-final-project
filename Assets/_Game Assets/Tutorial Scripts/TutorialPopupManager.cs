using UnityEngine;

public class TutorialPopupManager : MonoBehaviour
{
    private TutorialPopup currentPopup;
    
    public void OnPopupTriggered(TutorialPopup popup)
    {
        currentPopup = popup;
    }

    public void OnPopupStopped(TutorialPopup popup)
    {
        Debug.Log($"Popup {popup} stopped");
        currentPopup = null;
    }

    public void StopPopup()
    {
        currentPopup.Stop();
    }

    public void TriggerPopup(TutorialPopup popup)
    {
        popup.Trigger();
    }
}

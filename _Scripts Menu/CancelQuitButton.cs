using UnityEngine;
using UnityEngine.UI;

public class CancelQuitButton : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject quitCanvas;

    public void CancelButtonClick()
    {
        quitCanvas.SetActive(false);
        foreach (var button in menuCanvas.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    
    [SerializeField] private GameObject quitCanvas;
    [SerializeField] private GameObject menuCanvas;


    public void ExitButtonClick()
    {
        quitCanvas.SetActive(true);
        foreach (var button in menuCanvas.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }
}

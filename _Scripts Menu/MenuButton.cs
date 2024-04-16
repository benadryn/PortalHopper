using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject levelCanvas;


    public void MenuButtonClick()
    {
        levelCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}

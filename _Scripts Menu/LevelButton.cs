using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject levelSelectCanvas;

    public void LevelSelectButton()
    {
        menuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
    }

}

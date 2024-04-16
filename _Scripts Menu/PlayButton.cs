using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene(1);
    }
}

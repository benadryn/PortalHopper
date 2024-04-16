using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private SplashManager _splashManager;
    
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        _splashManager = SplashManager.Instance;
    }

    public void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = currentSceneIndex + 1;
        _splashManager.SetNextSceneIndex(nextSceneIndex);
        SceneManager.LoadScene("SplashScreen");
    }

    public void LoadSpecificLevel(int level)
    {
        _splashManager.SetNextSceneIndex(level);
        SceneManager.LoadScene("SplashScreen");

    }
    
}

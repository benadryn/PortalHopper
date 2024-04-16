using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public static SplashManager Instance;
    private bool _isInvoked;
    [SerializeField] private Sprite[] splashScreenImages;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private static int _nextSceneIndex = 2;


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

    public void SetNextSceneIndex(int sceneIndex)
    {
        _nextSceneIndex = sceneIndex;
    }
    
    void Start()
    {
        if (_nextSceneIndex - 2 < splashScreenImages.Length)
        {
            spriteRenderer.sprite = splashScreenImages[_nextSceneIndex - 2];
        }
        else
        {
            spriteRenderer.sprite = splashScreenImages[0];
            Debug.Log("splash screen image not found");
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            spriteRenderer.enabled = true;
            Invoke(nameof(LoadNextLevel), 3f);
        }
    }

    private void LoadNextLevel()
    {
        if (_nextSceneIndex >= 0 && _nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(_nextSceneIndex);
        }
        else
        {
            Debug.Log("Invalid Next Scene Index: " + _nextSceneIndex);
        }
    }
    
}

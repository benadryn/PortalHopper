using UnityEngine;
using UnityEngine.Playables;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;
    
    [SerializeField] private PlayableDirector losePlayableDirector;
    private bool _gameOver;    
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

    public void LoseGame()
    {
        if (!_gameOver)
        {
            losePlayableDirector.Play();
            _gameOver = true;
        }
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }
    
}
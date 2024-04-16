using UnityEngine;

public class FrogMunch : MonoBehaviour
{
    private FrogSpawnManager _frogSpawnManager;
    private BaseFrog _baseFrog;
    private EndGameManager _endGameManager;

    private void Start()
    {
        _frogSpawnManager = FrogSpawnManager.Instance;
        _endGameManager = EndGameManager.Instance;
        _baseFrog = GetComponent<BaseFrog>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CookieHealthManager.Instance.DamagePlayer(_baseFrog.CheckDamageToPlayer());
            ReturnFrogToPool();
            if (CookieHealthManager.Instance.CheckHealth() <= 0)
            {
               _endGameManager.LoseGame();
            }
        }

        if (other.CompareTag("Cookie") && !_baseFrog.CheckHidden())
        {
            var cookieBehaviour = other.gameObject.GetComponent<CookieBehaviour>();
            cookieBehaviour.ResetCookieThrown();
            cookieBehaviour.ReturnCookieToPool(other.gameObject);

            if (_baseFrog.Damaged(1) == 0)
            {
               ReturnFrogToPool();
                ScoreManager.Instance.AddScore(_baseFrog.GetScoreToAdd());
            }
        }
    }

    private void ReturnFrogToPool()
    {
        _frogSpawnManager.ReturnFrogToPool(gameObject);
        _baseFrog.ResetFrog();
    }
}
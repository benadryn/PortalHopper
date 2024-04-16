using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    
    public delegate void NewCookieSpawned(GameObject cookie);
    public event NewCookieSpawned OnCookieSpawned;

    public delegate void SendDistanceOfCookie(float distance);

    public event SendDistanceOfCookie OnDistanceSent;

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

    public void TriggerCookieSpawned(GameObject cookie)
    {
        OnCookieSpawned?.Invoke(cookie);
    }

    public void TriggerDistanceSent(float distance)
    {
        OnDistanceSent?.Invoke(distance);
    }
}

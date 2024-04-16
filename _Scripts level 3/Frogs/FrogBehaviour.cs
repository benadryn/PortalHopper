using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FrogBehaviour : MonoBehaviour
{
    [SerializeField] private float frogShowTime = 1.5f;
    private const float ShownPosZ = 0.03f;
    private const float HiddenPosZ = 0.12f;
    private int _bossFrogHealth = 5;
    [SerializeField] private float bossSpeed = 0.5f;

    private float originalFrogShowTime;
    private bool _isHidden;

    private float _randomTimeToResurface;

    private EndGameManager _endGameManager;

    private void Start()
    {
        _endGameManager = EndGameManager.Instance;
        _randomTimeToResurface = Random.Range(1f, 6f);
        originalFrogShowTime = frogShowTime;
    }

    void Update()
    {
        if (gameObject.CompareTag("Frog"))
        {
            FrogShowHide();
        }

        if (gameObject.CompareTag("BossFrog"))
        {
            SetTransform(ShownPosZ);
            BossFrogChase();
        }
    }

    private void BossFrogChase()
    {
        var pos = transform.position;
        pos += Vector3.down * (bossSpeed * Time.deltaTime);
        transform.position = pos;
    }
    
    private void FrogShowHide()
    {
        _isHidden = true;
        if (_isHidden)
        {
            _randomTimeToResurface -= Time.deltaTime;
        }
        if (_randomTimeToResurface <= 0)
        {
            SetTransform(ShownPosZ);
            frogShowTime -= Time.deltaTime;
            _isHidden = false;
        }
        if (frogShowTime <= 0)
        {
            SetTransform(HiddenPosZ);
            _randomTimeToResurface = Random.Range(1f, 6f);
            frogShowTime = originalFrogShowTime;
            _isHidden = true;
        }
    }

    public bool CheckHidden()
    {
        return _isHidden;
    }

    public int BossDamaged()
    {
        _bossFrogHealth--;
        return _bossFrogHealth;
    }
    private void SetTransform(float zVal)
    {
        var transform1 = transform.position;
        transform.position = new Vector3(transform1.x, transform1.y, zVal);
    }
}
using UnityEngine;

public abstract class BaseFrog : MonoBehaviour
{
    protected int Health = 1;
    protected int StartHealth;
    protected bool IsHidden;
    protected float FrogShowTime;
    protected float OriginalFrogShowTime;
    protected float TimeToResurfaceMin;
    protected float TimeToResurfaceMax;
    protected int ScoreAmount = 1;
    protected float DamageToPlayer = 1;
    
    private float _timeToResurface;
    private const float HiddenPosZ = 0.12f;
    private const float ShownPosZ = 0f; 
    private ParticleSystem _waterRippleParticleSystem;
    private bool _particlePlaying;
    

    protected void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _waterRippleParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected void FrogShowHide()
    {
        if (_timeToResurface == 0)
        {
            _timeToResurface = Random.Range(TimeToResurfaceMin, TimeToResurfaceMax);
        }
        IsHidden = true;
        if (IsHidden)
        {
            _timeToResurface -= Time.deltaTime;
        }
        if (_timeToResurface <= 0)
        {
            SetTransform(ShownPosZ);
            FrogShowTime -= Time.deltaTime;
            IsHidden = false;
        }

        if (FrogShowTime <= 1f && !_particlePlaying)
        {
            _waterRippleParticleSystem.Play();
            _particlePlaying = true;
        }
        if (FrogShowTime <= 0)
        {
            SetTransform(HiddenPosZ);
            _particlePlaying = false;
            _timeToResurface = Random.Range(TimeToResurfaceMin, TimeToResurfaceMax);
            FrogShowTime = OriginalFrogShowTime;
            IsHidden = true;
        }
    }

    protected void MoveTowardsPlayer(float speed)
    {
        var pos = transform.position;
        pos += Vector3.down * (speed * Time.deltaTime);
        transform.position = pos;
    }

    private void SetTransform(float zVal)
    {
        var transform1 = transform.position;
        transform.position = new Vector3(transform1.x, transform1.y, zVal);
    }


    public void ResetFrog()
    {
        FrogShowTime = OriginalFrogShowTime;
        IsHidden = true;
        _timeToResurface = Random.Range(TimeToResurfaceMin, TimeToResurfaceMax);
        Health = StartHealth;
    }
    public bool CheckHidden()
    {
        return IsHidden;
    }

    public int Damaged(int value)
    {
        return Health -= value;
    }

    public float CheckDamageToPlayer()
    {
        return DamageToPlayer;
    }

    public int GetScoreToAdd()
    {
        return ScoreAmount;
    }
}
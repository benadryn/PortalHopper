using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleDamage : MonoBehaviour
{
    public static HandleDamage Instance;

    [SerializeField] private FloatVariable healthPoints;
    private float _maxHealth;

    [SerializeField] private string projectileLayerName = "Projectile";
    [SerializeField] private string poisonGasLayerName = "Poison";
    [SerializeField] private string fireAoeLayerName = "Fire";
    [SerializeField] private Color damagedColor = Color.black;
    [SerializeField] private Color poisonDamageColor = Color.green;

    [SerializeField] private float fireDamage = 3.2f;

    [SerializeField] private float resetTimeAfterDeath = 3.0f;

    private int _projectileLayer;
    private int _poisonGasLayer;
    private int _fireLayer;
    
    private PlayerAnimator _playerAnimator;
    private ProjectileManager _projectileManager;
    private Material _playerColorMaterialGradient;

    private float _poisonDamage = 0.2f;
    private float _poisonDamageTime = 2.5f;
    private bool _takePoisonDamage;
    private float _poisonTimer;
    
    private static readonly int GradHeight = Shader.PropertyToID("GradHeight");
    private static readonly int GradTint = Shader.PropertyToID("_GradTint");
    public event Action OnProjectileReturnedToPool;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        _projectileManager = ProjectileManager.Instance;
        _projectileLayer = LayerMask.NameToLayer(projectileLayerName);
        _poisonGasLayer = LayerMask.NameToLayer(poisonGasLayerName);
        _fireLayer = LayerMask.NameToLayer(fireAoeLayerName);
        _playerColorMaterialGradient = GetComponent<Renderer>().material;
        _maxHealth = healthPoints.Value;
        
        _playerColorMaterialGradient.SetColor(GradTint, damagedColor);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer ==  _projectileLayer)
        {
            ProjectileDamageTaken(other);
        }

        if (other.gameObject.layer == _poisonGasLayer)
        {
            PoisonDamageTaken();
        }

        if (other.gameObject.layer == _fireLayer)
        {
            FireDamageTaken();
        }

        if (other.CompareTag("Damage"))
        {
            var enemy = other.gameObject.GetComponentInParent<Enemy>();
            var damage = enemy.GetDamageAmount();
            SmackDamageTaken(damage);
        }
    }

    private void ProjectileDamageTaken(Collider2D other)
    {
        var projectile = other.gameObject.GetComponent<ProjectileBehaviour>();
        if (!projectile.IsBlocked())
        {
            _playerAnimator.PlayDamageAnimation();
            healthPoints.Value -= projectile.GetProjectileDamageFromRange();
            UpdateSliderValue();
            _projectileManager.ReturnProjectileToPool(other.gameObject);
            OnProjectileReturnedToPool?.Invoke();
            CheckIfDead();
                
        }
    }

    private void PoisonDamageTaken()
    {
        _playerAnimator.PlayDamageAnimation();
        _takePoisonDamage = true;
        _poisonTimer = _poisonDamageTime;

    }

    private void FireDamageTaken()
    {
        _playerAnimator.PlayDamageAnimation();
        healthPoints.Value -= fireDamage;
        UpdateSliderValue();
        CheckIfDead();
    }

    private void SmackDamageTaken(float damage)
    {
        _playerAnimator.PlayDamageAnimation();
        healthPoints.Value -= damage;
        UpdateSliderValue();
        CheckIfDead();
    }

    private void Update()
    {
        if (_takePoisonDamage)
        {
            _poisonTimer -= Time.deltaTime;
            if (_poisonTimer <= 0f)
            {
                _takePoisonDamage = false;
                _playerColorMaterialGradient.SetColor(GradTint, damagedColor);
                CheckIfDead();
            }
            else
            {
                TakeDamageOverTime(_poisonDamage);
            }
        }
    }

    private void TakeDamageOverTime(float damage)
    {
        healthPoints.Value -= damage * Time.deltaTime;
        UpdateSliderValue();
        _playerColorMaterialGradient.SetColor(GradTint, poisonDamageColor);
    }
    
    public void UpdateSliderValue()
    {
        var healthPercentage = Mathf.Clamp01(healthPoints.Value / _maxHealth);
        var sliderValue = Mathf.Lerp(0.3f, -0.3f, healthPercentage);
        _playerColorMaterialGradient.SetFloat(GradHeight, sliderValue);
    }

    private void CheckIfDead()
    {
        if (healthPoints.Value <= 0.0f)
        {
            _playerAnimator.PlayDeathAnimation();
            StartCoroutine(nameof(HandleDeath));
        }
    }

    private IEnumerator HandleDeath()
    {
        PlayerController.Instance.enabled = false;
        yield return new WaitForSeconds(resetTimeAfterDeath);
        healthPoints.Value = _maxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

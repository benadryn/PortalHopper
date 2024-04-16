using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class CookieBehaviour : MonoBehaviour
{
    [SerializeField] private ObjectPool _cookieSpawner;
    [SerializeField] private GameObject distanceCheckGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float baseSpeed = 5;
    [SerializeField] private float minRotAngle = -60f;
    [SerializeField] private float maxRotAngle = 60f;
    [SerializeField] private float rotationSpeed;

    [Header("Throwing Cookie")] [SerializeField]
    private float thrownMaxSize = 0.75f;

    private float _originalSizeChangeRate;
    [SerializeField] private float maxForceDuration = 2.0f;
    [SerializeField] private float thrownMinSize = 0.6f;
    [SerializeField] private float sizeChangeRate = 0.3f;
    [SerializeField] private ParticleSystem waterRipple;
    private float _originalSize;
    private float _currentSize;
    private bool _isIncreasingSize;
    private float forceDuration;


    private Camera _cameraMain;
    private Rigidbody2D _rb;
    private EventManager _eventManager;
    private float _power;
    private float _rotationDirection;
    private bool _rotateClockwise;
    private bool _cookieThrown;

    private const int MaxRotationClamp = 1870;
    private const int MinRotationClamp = 1740;
    private const int RotationMulti = 20;

    private void Start()
    {
        
        
        _rb = GetComponent<Rigidbody2D>();
        _eventManager = EventManager.Instance;
        _originalSize = transform.localScale.x;
        _currentSize = _originalSize;
        _cameraMain = Camera.main;
        _originalSizeChangeRate = sizeChangeRate;
    }

    private void Update()
    {
        if (_cookieThrown)
        {
            BounceOnWater();
        }

        if (_cookieThrown) return;

        GetPressStrength();
    }

    private void GetPressStrength()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _power += Time.deltaTime;
            if (_power > slider.maxValue)
            {
                _power = slider.maxValue;
            }

            var distance = _power * baseSpeed * maxForceDuration;
            _eventManager.TriggerDistanceSent(distance);
        }

        RotateCookie();
        slider.value = _power;

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_power > 0.3f)
            {
                forceDuration = _power * maxForceDuration;

                StartCoroutine(ApplyForceWithDelay(forceDuration));
                _cookieThrown = true;
                var newCookie = _cookieSpawner.GetPooledObject(transform.position);
                _eventManager.TriggerCookieSpawned(newCookie);
            }

            _power = 0f;
            slider.value = _power;
        }
    }

    private IEnumerator ApplyForceWithDelay(float duration)
    {
        var force = transform.up * baseSpeed;
        _rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        _rb.velocity = Vector2.zero;
        InstantiateWaterRipple();
        if (gameObject)
        {
            ResetCookieThrown();
            _cookieSpawner.ReturnObjectToPool(gameObject);
        }
    }
    private void BounceOnWater()
    {
        if (_isIncreasingSize)
        {
            _currentSize += sizeChangeRate * Time.deltaTime;
            transform.localScale = new Vector3(_currentSize, _currentSize, _currentSize);
            if (_currentSize >= thrownMaxSize)
            {
                _currentSize = thrownMaxSize;
                _isIncreasingSize = false;
                sizeChangeRate += 0.3f;
            }
        }
        else
        {
            _currentSize -= sizeChangeRate * Time.deltaTime;
            transform.localScale = new Vector3(_currentSize, _currentSize, _currentSize);
            if (_currentSize <= thrownMinSize)
            {
                InstantiateWaterRipple();
                _currentSize = thrownMinSize;
                _isIncreasingSize = true;
            }
        }
    }
    
    private void RotateCookie()
    {
        // var mouseMove = Input.mousePosition;
        // Debug.Log(mouseMove);
        // // var mousePos = _cameraMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
        // //     _cameraMain.nearClipPlane));
        // // mousePos.z = 0;
        // mouseMove.z = 0;
        // var direction = (mouseMove - transform.position).normalized;
        // if (direction == Vector3.zero) return;
        // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * RotationMulti;
        // transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(angle, MinRotationClamp, MaxRotationClamp));
        var mousePos = _cameraMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            _cameraMain.nearClipPlane));

        var distance = gameObject.transform.position.x - mousePos.x;
        distance = Mathf.Clamp(distance, -0.3f, 0.3f);
        var rotationAngle = Mathf.Lerp(minRotAngle, maxRotAngle, distance * rotationSpeed);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

    }

    public void ResetCookieThrown()
    {
        transform.position = _cookieSpawner.transform.position;
        _cookieThrown = false;
        _isIncreasingSize = false;
        _currentSize = _originalSize;
        sizeChangeRate = _originalSizeChangeRate;
        transform.localScale = new Vector3(_originalSize, _originalSize, _originalSize);
    }

    public void ReturnCookieToPool(GameObject cookie)
    {
        ResetCookieThrown();
        _cookieSpawner.ReturnObjectToPool(cookie);
    }

    private void InstantiateWaterRipple()
    {
        var pos = transform.position;
        Instantiate(waterRipple, new Vector3(pos.x, pos.y, waterRipple.transform.position.z), Quaternion.identity);
    }
}
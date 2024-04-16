using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CookieHealthManager : MonoBehaviour
{
    public static CookieHealthManager Instance;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthColorImage;
    [SerializeField] private float maxHealth;
    [SerializeField]private float currentHealth;
    private bool _sliderCoroutineRunning;
    private bool _lowHealthCoroutineRunning;
    private float startAlpha;
    private float targetAlpha = 0;


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
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        
        startAlpha = healthColorImage.color.a;

    }

    private void Update()
    {
        if (currentHealth <= maxHealth/3 && !_lowHealthCoroutineRunning)
        {
            StartCoroutine(nameof(LowHealthBarFlash), 0.5f);
        }
    }

    public void DamagePlayer(float value)
    {
        currentHealth -= value;
        UpdateSliderColor();
        if (!_sliderCoroutineRunning)
        {
            StartCoroutine(nameof(UpdateHealthSlider), 0.5f);
        }
    }

    private void UpdateSliderColor()
    {
        if (currentHealth <= maxHealth / 2)
        {
            healthColorImage.color = Color.red;
        }
    }

    private IEnumerator UpdateHealthSlider(float duration)
    {
        _sliderCoroutineRunning = true;
        var elapsedTime = 0f;
        var startValue = healthSlider.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startValue, currentHealth, elapsedTime / duration);
            yield return null;
        }

        healthSlider.value = currentHealth;
        _sliderCoroutineRunning = false;
    }

    private IEnumerator LowHealthBarFlash(float duration)
    {
        _lowHealthCoroutineRunning = true;
        var elapsedTime = 0f;
        Color newColor = healthColorImage.color;
        
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / duration;
            var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            newColor = new Color(healthColorImage.color.r, healthColorImage.color.g, healthColorImage.color.b,
                newAlpha);
            yield return null;
        }

        healthColorImage.color = newColor;
        (startAlpha, targetAlpha) = (targetAlpha, startAlpha);
        _lowHealthCoroutineRunning = false;
    }

    public float CheckHealth()
    {
        return currentHealth;
    }
}
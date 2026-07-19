using DigitalRuby.RainMaker;
using UnityEngine;

public class RainController : MonoBehaviour
{
    RainScript2D rainScript2D => GetComponent<RainScript2D>();

    [Range(0.0f, 1f)]
    [SerializeField] private float intensity;
    [SerializeField] private float targetIntensity;

    [SerializeField] private float changeRate;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    [SerializeField] private float chanceToRain;
    [SerializeField] private float currentChanceToRain;
    [SerializeField] private float chanceToRainCoolDown;
    [SerializeField] private float chanceToRainCoolDownTimer;

    [SerializeField] private float changeIntensityCoolDown;
    [SerializeField] private float changeIntensityCoolDownTimer;

    private bool canChangeIntensity;

    private void Start()
    {
        canChangeIntensity = true;
        targetIntensity = maxValue;
        chanceToRain = 50;
        changeIntensityCoolDownTimer = changeIntensityCoolDown;
        chanceToRainCoolDownTimer = chanceToRainCoolDown;
    }

    private void Update()
    {
        if (canChangeIntensity && currentChanceToRain > chanceToRain) 
            ChangeIntensity();
        if (!canChangeIntensity) CoolDownIntensity();
        SetCurrentChanceToRain();
        rainScript2D.RainIntensity = intensity;
    }

    private void SetCurrentChanceToRain()
    {
        chanceToRainCoolDownTimer -= Time.deltaTime;
        if(chanceToRainCoolDownTimer < 0)
        {
            chanceToRainCoolDownTimer = 1.5f;
            currentChanceToRain = Random.Range(0, 100);
        }
    }
    private void CoolDownIntensity()
    {
        changeIntensityCoolDownTimer -= Time.deltaTime;
        if (changeIntensityCoolDownTimer <= 0) canChangeIntensity = true;
    }
    private void ChangeIntensity()
    {
        if (intensity < targetIntensity) IncreaseIntensity();
        if (intensity > targetIntensity) DecreaseIntensity();
    }
    private void DecreaseIntensity()
    {
        intensity -= changeRate * Time.deltaTime;
        if (intensity <= targetIntensity) SetTargetIntensity(minValue);
    }
    private void IncreaseIntensity()
    {
        intensity += changeRate * Time.deltaTime;
        if (intensity >= targetIntensity) SetTargetIntensity(maxValue);
    }
    private void SetTargetIntensity(float value)
    {
        targetIntensity = value;
        canChangeIntensity = false;
        changeIntensityCoolDownTimer = changeIntensityCoolDown;
    }
}

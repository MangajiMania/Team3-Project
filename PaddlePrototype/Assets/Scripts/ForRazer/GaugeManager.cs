using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GaugeManager : MonoBehaviour
{
    
    
    [SerializeField] private ChargingRazerManager charging;

    [Header("Gauge")]
    public int filledGaugeSegments = 0;
    private int currentGaugeValue = 0;
    private int gaugePerSegment = 10;
    [SerializeField] private int maxGaugeSegments = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI gaugeSegmentText;
    [SerializeField] private TextMeshProUGUI gaugeValueText;

    public int CurrentGaugeValue => currentGaugeValue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateGaugeUI();
    }

    public void AddGauge()
    {
        
        if (currentGaugeValue >= filledGaugeSegments * gaugePerSegment)
        {
            currentGaugeValue = maxGaugeSegments * gaugePerSegment;
        }
        else
        {
            currentGaugeValue++;
        }
        
        if (currentGaugeValue %gaugePerSegment ==0)
        {
            ChangeGaugeLevel(currentGaugeValue /gaugePerSegment);
        }
        UpdateGaugeUI();
    }
    
    
    public void ChangeGaugeLevel(int level)
    {

        if (level <= 0)
        {
            filledGaugeSegments--;
        }
        else
        {
            filledGaugeSegments = level; 
        }
        
        filledGaugeSegments = Mathf.Clamp(filledGaugeSegments, 0, maxGaugeSegments);
        
        Debug.Log(filledGaugeSegments + "차징 게이지");
        
        UpdateGaugeUI();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateGaugeUI()
    {
        if (gaugeSegmentText != null)
        {
            gaugeSegmentText.text = $"Gauge Segments: {filledGaugeSegments} / {maxGaugeSegments}";
        }

        if (gaugeValueText != null)
        {
            gaugeValueText.text = $"Gauge Value: {currentGaugeValue}";
        }
    }
}

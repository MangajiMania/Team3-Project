using UnityEngine;

public class ChargingRazerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GaugeManager gauge; 
    private bool charging = false;
    
    private int bounceCount = 0;
    private int chargeCount = 0;
    [SerializeField]private int PerBounceCount = 10;
    
    
    
    
    public void CheckBounceCount()
    {
        bounceCount++;
        Debug.Log("팅");
        if (bounceCount % PerBounceCount == 0 && gauge.filledGaugeSegments!=0)
        {
            ChargingbyBounce(bounceCount/PerBounceCount);
            
        }
    }

    
    private void ChargingbyBounce(int bounceUnit)
    {
        charging = true;
        gauge.ChangeGaugeLevel(-1);

        chargeCount = bounceUnit;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ShootRazer();
        }
    }

    private void ShootRazer()
    {
        if (chargeCount > 0)
        {
            Debug.Log("차징 "+chargeCount+"슛");
            
            Reset();
        }
    }
    
    
    public void Reset()
    {
        
        bounceCount = 0;
        chargeCount = 0;
    }
    
}

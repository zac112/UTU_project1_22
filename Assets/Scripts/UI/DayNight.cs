using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNight : MonoBehaviour
{ 
    [SerializeField] public Image dayNight;
    [SerializeField] public TMP_Text DayValue;
    [SerializeField] public Gradient dayNightColor;
    [SerializeField] int daylength = 20000;
    private int days;
    private int dayPhase;
    
    void Start()
    {
        dayNight = GameObject.Find("DayNightFilter").GetComponent<Image>();
        GameEvents.current.Tick += Timer;
    }

    void Timer(int ticks)
    {   
        dayPhase++;
        // Switch DayNightFilter transparency
        dayNight.color = dayNightColor.Evaluate((0f+ticks%daylength)/daylength);
        if (dayPhase >= daylength)
        {            
            days++;
            IntializeUI();
            GameEvents.current.OnDayChange(days);
            dayPhase = 0;
        }
    }

    public void IntializeUI()
    {
        DayValue.text = $"Day: {days}";
    }

    public int getDayLength(){ return daylength; }
}

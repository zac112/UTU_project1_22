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
    [SerializeField] int daylength = 200;
    [SerializeField] float filterPercentage = 1.0f;    
    private int days;
    
    void Start()
    {
        dayNight = GameObject.Find("DayNightFilter").GetComponent<Image>();
        GameEvents.current.Tick += Timer;
    }

    void Timer(int ticks)
    {   
        // Switch DayNightFilter transparency
        dayNight.color = dayNightColor.Evaluate((filterPercentage*ticks%daylength)/daylength);
        if ((ticks%daylength)+1 >= daylength)
        {            
            days++;
            IntializeUI();
            GameEvents.current.OnDayChange(days);
        }
    }

    public void IntializeUI()
    {
        DayValue.text = $"Day: {days}";
    }
}

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
        ticks += (int)Time.deltaTime;

        // Switch DayNightFilter transparency
        dayNight.color = dayNightColor.Evaluate((filterPercentage*ticks%daylength)/daylength);
        double dayMax = dayNight.color[3];

        if (dayMax == 0)
        {
            days++;
        }
    }


    void Update()
    {
        intializeUI();
    }


    public void intializeUI()
    {
        DayValue.text = $"Day: {days}";
    }
}

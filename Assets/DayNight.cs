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

    private int days;
    private bool active = true;


    
    void Start()
    {
        dayNight = GameObject.Find("DayNightFilter").GetComponent<Image>();
        GameEvents.current.Tick += Timer;
    }

    void Timer(int ticks)
    {     
        /*// Day length --> start again if over
        if (ticks > 240)
        {
            ticks = 0;
        }

        // Increase day (Time when day switches --> lowest transparency)
        if (ticks == 120)
        {
            days++;
        }

        ticks += (int)Time.deltaTime;*/

        // Switch DayNightFilter transparency (24min = 0.00001f ???);

        Debug.Log(dayNight.color = dayNightColor.Evaluate(ticks *0.0002f));
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

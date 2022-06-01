using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    public static UIManager current;
    void Awake() => current = this;


    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text gold;
    [SerializeField] PlayerStats stats;

    public void UpdateHealthText(int amount) => health.text = $"Health: {amount}";

    public void UpdateGoldText(int amount) => gold.text = $"Gold: {amount}";

}

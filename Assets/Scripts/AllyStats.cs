using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStats : MonoBehaviour
{

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField] HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        //what happens when ally dies
        if(currentHealth<=0){
            Destroy(gameObject);
        }
    }
}

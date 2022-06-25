using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Stats : NetworkBehaviour
{
    protected NetworkVariable<int> networkMaxHealth = new NetworkVariable<int>();
    protected NetworkVariable<int> networkHealth = new NetworkVariable<int>();

    protected int currentHealth => networkHealth.Value;
    protected int maxHealth => networkMaxHealth.Value;

    [SerializeField] protected int attack;
    [SerializeField] protected int startingHealth = 10;
    [SerializeField] protected HealthBar healthBar;
     
    private void Start()
    {
        if (NetworkManager.IsHost) {
            networkMaxHealth.Value = startingHealth;
            networkHealth.Value = startingHealth;
        }
        
        SetHealthServerRpc(startingHealth);
        SetMaxHealthServerRpc(startingHealth);
        healthBar.SetMaxHealth(startingHealth);
        
        PostStart();
    }

    abstract protected void PostStart();

    public int GetAttack() { return attack; }
    public int GetHP() { return currentHealth; }

    public void ReceiveDamage(int amount)
    {
        healthBar.gameObject.SetActive(true);
        SetHealthServerRpc (currentHealth - amount);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            ActOnDeath();
        }
        StartCoroutine(HideHealthBar());
    }

    private IEnumerator HideHealthBar() {
        yield return new WaitForSeconds(5);
        healthBar.gameObject.SetActive(false); 
    }

    protected abstract void ActOnDeath();

    [ServerRpc(RequireOwnership = false)]
    protected void SetHealthServerRpc(int health)
    {
        networkHealth.Value = health;
    }

    [ServerRpc(RequireOwnership = false)]
    protected void SetMaxHealthServerRpc(int amount)
    {
        networkMaxHealth.Value = amount;
    }
}

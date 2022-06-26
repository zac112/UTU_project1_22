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

    [SerializeField] protected int attack = 2;
    [SerializeField] protected int startingHealth = 10;
    [SerializeField] protected HealthBar healthBar;

    float sinceLastAttacked = 0;
    private void Start()
    {
        GameObject hb = Instantiate<GameObject>(Resources.Load<GameObject>("Healthbar"), transform);
        healthBar = hb.transform.GetComponentInChildren<HealthBar>();
        healthBar.gameObject.SetActive(false);

        if (NetworkManager.IsHost) {
            networkMaxHealth.Value = startingHealth;
            networkHealth.Value = startingHealth;
        }
        
        SetHealthServerRpc(startingHealth);
        SetMaxHealthServerRpc(startingHealth);
        healthBar.SetMaxHealth(startingHealth);

        StartCoroutine(HideHealthBar());

        PostStart();
    }

    abstract protected void PostStart();

    public int GetAttack() { return attack; }
    public int GetHP() { return currentHealth; }
    
    protected void SetHP(int amount) {
        healthBar.gameObject.SetActive(true);
        SetHealthServerRpc (amount);
        sinceLastAttacked = 0;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            ActOnDeath();
            if (NetworkManager.IsHost)
            {
                NetworkObject.Despawn();
                Destroy(gameObject,1);
                gameObject.SetActive(false);
            }
        }
    }
    public void ReceiveDamage(int amount)
    {
        SetHP(currentHealth-amount);
    }

    private IEnumerator HideHealthBar() {
        while (true)
        {
            if (sinceLastAttacked > 5)
            {
                healthBar.gameObject.SetActive(false);                
                yield return new WaitUntil(()=>sinceLastAttacked < 1);
            }
            sinceLastAttacked += Time.deltaTime;
            yield return null;
        }
    }

    /*
     * Do what the GameOhject needs to do on death. 
     * Despawning and destroying is handled in the base class.
     */
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

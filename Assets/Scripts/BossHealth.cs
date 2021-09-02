using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private displayBossHP DisplayBossHP;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private Slime1 slime1;
    public int maxHealth = 3;
    int currentHealth;
    BossNavMesh bossNavMesh;


    //float cooldownTime;
    private float nextTime = 0;
    public bool BossHP = true;

    // Start is called before the first frame update
    void Start()
    {
        bossNavMesh = GetComponent<BossNavMesh>();
        //cooldownTime = 10.0f;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        
        if(Time.time >= nextTime)
        {
            currentHealth -= damage;
            nextTime = Time.time + 4.0f;
            slime1.perdeuHP = true;
            DisplayBossHP.SetHealth(currentHealth.ToString());
            if(currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        gameOverScreen.MainMenuButton();
        Destroy(gameObject);
    }

}

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;
    bool invulnerable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!invulnerable)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth); //current health can never go below 0 or above max
            if (currentHealth <= 0) //if the enemy's health reaches 0 it gets destroyed
            {
                killEnemy();
            }
        }
    }
    public void killEnemy()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //on collision with an entity with the "Player" tag
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            player.PlayerTakeDamage(contactDamage); //makes player take contact damage
        }
    }
}

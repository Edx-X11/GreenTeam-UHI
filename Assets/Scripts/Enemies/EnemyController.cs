using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int currentHealth = 0;
    int maxHealth;
    int contactDamage;
    bool invulnerable = false;
    public bool onScreen = false;
    public int points;

    public GameManager manager;

    public void SetContactDamage(int contact)
    {
        this.contactDamage = contact;
    }
    public void SetMaxHealth(int Health)
    {
        this.maxHealth = Health;
        currentHealth = maxHealth;
    }
    public void SetPoints(int score)
    {
        this.points = score;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameVisible() //functions to determine if the enemy is on screen or not
    {
        onScreen = true;
    }
    private void OnBecameInvisible()
    {
        onScreen = false;
    }
    public void TakeDamage(int damage)
    {
        if (!invulnerable)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //current health can never go below 0 or above max
            if (currentHealth <= 0) //if the enemy's health reaches 0 it gets destroyed
            {
                GameManager.Instance.AddScore(points);
                KillEnemy();
            }
        }
    }
    public void KillEnemy()
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

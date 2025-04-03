using System.Linq;
using UnityEngine;

public class SlugController : MonoBehaviour
{
    public GameObject Player;
    Rigidbody2D rb2d;
    EnemyController enemy;

    int contactDamage = 2;
    int maxHealth = 5;
    int currentHealth;
    float moveSpeed = 0.3f;
    int points = 1000;
    Vector2 position;
    float positionDifference;
    int direction;
    int yRotation;
    bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("PlayerCharacter");
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyController>();
        enemy.SetContactDamage(contactDamage);
        enemy.SetMaxHealth(maxHealth);
        enemy.SetPoints(points);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) //checks if the enemy is on the ground
        {
            isGrounded = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        if(enemy.onScreen && Player != null) //if enemy is on screen and player is not null enemy will act
        {
            direction = (this.transform.position.x < Player.transform.position.x) ? 1 : -1; //sets direction to 1 or -1 depending on if player is to the left or right of enemy
            if (direction == 1) //if player is to the right
            {
                yRotation = 180;
            }
            else //otherwise player is to the left
            {
                yRotation = 0;
            }
            if (isGrounded) //moves towards player as long as enemy is on the ground
            {
                position.x = position.x + ((0.1f * moveSpeed) * direction);
                transform.position = position;
            }
            positionDifference = this.transform.position.x - Player.transform.position.x; //subtracts player x position from enemy x position
            if (positionDifference <= 1.5f && rb2d.gravityScale == -1) //if enemy is close enough to player the enemy will drop onto the player
            {
                rb2d.gravityScale = 1;
                isGrounded = false;
            }
            if(rb2d.gravityScale != -1)
            {
                rb2d.transform.rotation = Quaternion.Euler(180, yRotation, 0); //rotates the sprite towards the ground
            }
        }
    }
}

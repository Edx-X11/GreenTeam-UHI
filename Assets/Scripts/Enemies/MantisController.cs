using UnityEngine;
using UnityEngine.UIElements;


public class MantisController : MonoBehaviour
{
    public GameObject Player;
    private float moveSpeed = 0.8f;
    private int contactDamage = 3;
    private int maxHealth = 5;
    private int points = 750;

    public BoxCollider2D hitbox;
    bool playerHit = false;
    float pathAway = 1.5f;
    Vector2 position;
    int direction;

    PlayerControl playerControl;
    Rigidbody2D rb2d;
    EnemyController enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("PlayerCharacter");
        playerControl = gameObject.GetComponent<PlayerControl>();
        enemy = GetComponent<EnemyController>();
        rb2d = GetComponent<Rigidbody2D>();
        enemy.SetContactDamage(contactDamage); //sets contact damage, health and points in the enemy controller
        enemy.SetMaxHealth(maxHealth);
        enemy.SetPoints(points);
        direction = (this.transform.position.x < Player.transform.position.x) ? 1 : -1; //determines if player is to the left or right of entity
    }
    // Update is called once per frame
    void Update()
    {
        position = transform.position; //sets position variable as current position
        if (enemy.onScreen && Player != null)
        {
            if (direction == 1) //if player is to the right
            {
                rb2d.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else //otherwise player is to the left
            {
                rb2d.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (playerHit) //when player gets hit paths the entity away from the player
            {
                direction = 1;
                position.x = position.x + ((0.1f * moveSpeed)) * direction;
                transform.position = position;
                pathAway -= Time.deltaTime;
                if (pathAway < 0) //moves in one direction for 1.5 seconds
                {
                    playerHit = false;
                    pathAway = 1.5f;
                }
            }
            else //enemy moves towards the player
            {
                position.x = position.x + ((0.1f * moveSpeed) * direction);
                transform.position = position;
                direction = (this.transform.position.x < Player.transform.position.x) ? 1 : -1;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) //when enemy collides with the player sets boolean to true
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHit = true;
        }
    }
}

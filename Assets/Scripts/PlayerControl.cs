using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.EventSystems.EventTrigger;
public class PlayerControl : MonoBehaviour
{
    public InputAction LeftAction;
    public InputAction RightAction;
    public Rigidbody2D rb2d;
    public InputAction ShootAction;
    public bool isGrounded;
    public bool facingLeft;
    public bool invulnerable;

    public GameObject bulletPrefab;
    public Transform BulletShootpos;
    public int bulletDamage = 1;
    public float bulletSpeed = 10f;

    public int currentHealth;
    public int maxHealth = 15;
    public bool isTakingDamage = false;
    float lastHit = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        rb2d = GetComponent<Rigidbody2D>();
        facingLeft = true;

        LeftAction.Enable();
        RightAction.Enable();
        ShootAction.Enable();

        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true; //checks for collision with another object for all purposes here, checks for ground collision
    }
    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        PlayerShootInput();
        
        if (isTakingDamage) //checks when last hit to determine if player can be hit again based off damage cooldown
        {
            float damageCooldown = 1.5f;
            lastHit += Time.deltaTime;
            if (lastHit > damageCooldown)
            {
                invulnerable = false;
                lastHit = 0f;
            }
        }
        Debug.Log(currentHealth);
    }
    void Run()
    {
        Vector2 position = transform.position; //stores player asset position in a vector variable
        float horizontal = 0.0f;
        if (LeftAction.IsPressed())
        {
            horizontal = -1f; //move left
            rb2d.transform.rotation = Quaternion.Euler(0, 180, 0);
            facingLeft = false;
        }
        else if (RightAction.IsPressed())
        {
            horizontal = 1f; //move right
            rb2d.transform.rotation = Quaternion.Euler(0, 0, 0);
            facingLeft = true;
        }
        position.x = position.x + (0.1f * horizontal); //change position to horizontal * 0.1
        transform.position = position; //set position of asset to new position value
    }

    void Jump()
    {
        float jumpSpeed = 350f;
        if (Input.GetKeyDown(KeyCode.W) && isGrounded) //checks if w key was pressed down and if ground collision is true then fires once
        {
            rb2d.AddForce(Vector2.up * jumpSpeed); //adds velocity to y and causes player character to jump
            isGrounded = false; //sets boolean value for ground collision to false
        }

    }
    void PlayerShootInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerShoot();
        }
    }
    void playerShoot() 
    {
        GameObject bullet = Instantiate(bulletPrefab, BulletShootpos.transform.position, Quaternion.identity); //creates bullet prefab at bulletshootpos gizmo on playercharacter
        bullet.name = bulletPrefab.name; //changes the name of the created prefab
        bullet.GetComponent<BulletScript>().SetBulletDamage(bulletDamage); //sets damage from this script
        bullet.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed); //sets speed from this script
        bullet.GetComponent<BulletScript>().SetBulletDirection((facingLeft) ? Vector2.left : Vector2.right); //shoots left or right depending on the way the player is facing
        bullet.GetComponent<BulletScript>().Shoot();
    }

    public void PlayerTakeDamage(int damage)
    {
        
        if (!invulnerable)
        {
            currentHealth -= damage;
            isTakingDamage = true;
            invulnerable = true;
            Mathf.Clamp(currentHealth, 0, maxHealth); //current health can never go below 0 or above max
            if (currentHealth <= 0)
            {
                killPlayer();
            }
            
        }
    }
    public void killPlayer()
    {
        Destroy(gameObject);
    }
}

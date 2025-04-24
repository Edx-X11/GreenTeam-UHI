using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
public class PlayerControl : MonoBehaviour
{
    public InputAction LeftAction;
    public InputAction RightAction;
    public Rigidbody2D rb2dPlayer;
    public InputAction ShootAction;
    public InputAction JumpAction;
    public bool isGrounded;
    public bool facingLeft;
    public bool invulnerable;

    public int currentHealth;
    public int maxHealth = 15;
    public bool isTakingDamage = false;
    float lastHit = 0f;
    float invulnPeriod = 0.5f;
    public bool weapon1;
    public bool weapon2 = false;
    public Transform BulletShootpos;
    public GameObject BulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        
        rb2dPlayer = GetComponent<Rigidbody2D>();
        facingLeft = true;

        LeftAction.Enable();
        RightAction.Enable();
        ShootAction.Enable();
        JumpAction.Enable();
        

        currentHealth = maxHealth;
        weapon1 = true;
        weapon2 = false;
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
            RightAction.Disable();
            JumpAction.Disable();
            LeftAction.Disable();
            ShootAction.Disable();

            float damageCooldown = 1.5f;
            lastHit += Time.deltaTime;
            if(invulnPeriod > 0)
            {
                Vector2 position = transform.position;
                position.x -= 0.1f;
                transform.position = position;
            }
            if (lastHit > damageCooldown)
            {
                RightAction.Enable();
                LeftAction.Enable();
                ShootAction.Enable();
                JumpAction.Enable();
                invulnerable = false;
                lastHit = 0f;
                invulnPeriod = 0.5f;
                isTakingDamage = false;
            }
        }

        
        //Debug.Log(currentHealth);
    }
    void Run()
    {
        Vector2 position = transform.position; //stores player asset position in a vector variable
        float horizontal = 0.0f;
        if (LeftAction.IsPressed())
        {
            horizontal = -1f; //move left
            rb2dPlayer.transform.rotation = Quaternion.Euler(0, 180, 0);
            facingLeft = false;
        }
        else if (RightAction.IsPressed())
        {
            horizontal = 1f; //move right
            rb2dPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
            facingLeft = true;
        }
        position.x = position.x + (0.1f * horizontal); //change position to horizontal * 0.1
        transform.position = position; //set position of asset to new position value
    }

    void Jump()
    {
        float jumpSpeed = 350f;
        if (JumpAction.IsPressed() && isGrounded) //checks if w key was pressed down and if ground collision is true then fires once
        {
            rb2dPlayer.AddForce(Vector2.up * jumpSpeed); //adds velocity to y and causes player character to jump
            isGrounded = false; //sets boolean value for ground collision to false
        }

    }
    public void PlayerShootInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) //if fire key is pressed checks which weapon is currently enabled
        {
            CheckWeapon();
        }
    }
    void CheckWeapon() //uses weapon booleans to call appropriate shoot function
    {
        if (weapon1)
        {
            GetComponent<PistolScript>().Shoot(facingLeft);
        }
        else if(weapon2)
        {

        }
    }
    public void PlayerTakeDamage(int damage) //calls whenever the player should take damage and decrements health
    {
        
        if (!invulnerable) //if player isn't invulnerable code continues
        {
            currentHealth -= damage;
            isTakingDamage = true;
            invulnerable = true;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //current health can never go below 0 or above max
            if (currentHealth <= 0)
            {
                KillPlayer(); //if health reaches 0 destroys player object
            }
        }
    }
    public void KillPlayer()
    {
        GameManager.Instance.PlayerDied();
        Destroy(gameObject);
    }
}

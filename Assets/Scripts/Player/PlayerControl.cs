using System;
using System.Runtime.CompilerServices;
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
    public bool canJump;
    float jumpDelay = 0.3f;
    public bool facingLeft;
    public bool invulnerable;
    float actionableCooldown = 1.5f;
    float invulnPeriod = 4f;

    public int currentHealth;
    public int maxHealth = 15;
    public bool isTakingDamage = false;
    public bool tookDamage = false;
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
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true; //checks for collision with another object for all purposes here, checks for ground collision
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(invulnPeriod);

        Run();
        Jump();
        PlayerShootInput();
        if(tookDamage)
        {
            invulnPeriod -= Time.deltaTime;
            actionableCooldown -= Time.deltaTime;
            if(invulnPeriod < 0)
            {
                invulnerable = false;
                invulnPeriod = 4f;
                tookDamage = false;
            }
            if (actionableCooldown < 0)
            {
                LeftAction.Enable();
                RightAction.Enable();
                JumpAction.Enable();
                ShootAction.Enable();
                actionableCooldown = 1.5f;
            }
        }

        if(isGrounded && !canJump)
        {
            jumpDelay -= Time.deltaTime;
            if (jumpDelay < 0)
            {
                canJump = true;
                jumpDelay = 0.3f;
            }
        }
    }
    void Run()
    {
        float horizontal = 0.0f;
        Vector2 position = transform.position; //stores player asset position in a vector variable
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
        position.x = position.x + (0.13f * horizontal); //change position to horizontal * 0.1
        transform.position = position; //set position of asset to new position value
    }

    void Jump()
    {
        float jumpSpeed = 400f;
        if (JumpAction.IsPressed() && isGrounded && canJump) //checks if w key was pressed down and if ground collision is true then fires once
        {
            rb2dPlayer.AddForce(Vector2.up * jumpSpeed); //adds velocity to y and causes player character to jump
            isGrounded = false; //sets boolean value for ground collision to false
            canJump = false;
        }

    }
    public void PlayerShootInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && ShootAction.enabled) //if fire key is pressed checks which weapon is currently enabled
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
            LeftAction.Disable();
            RightAction.Disable();
            JumpAction.Disable();
            ShootAction.Disable();
            tookDamage = true;
            invulnerable = true;
            isTakingDamage = true;
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

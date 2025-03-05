using System.Threading;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.UIElements;

public class GrasshopperController : MonoBehaviour
{
    GameObject Player;
    private float timer = 0.0f;
    private float waittime = 1.0f;
    private bool isGrounded;
    private float jumpSpeed = 6.5f;
    private float moveSpeed = 4f;

    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0; //framerate stuff for movement speed
        Application.targetFrameRate = 60;

        Player = GameObject.Find("PlayerCharacter");
        rb2d = GetComponent<Rigidbody2D>();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            isGrounded = true;
    }

    void Update()
    {
        int direction = (this.transform.position.x < Player.transform.position.x) ? 1 : -1; //checks direction of player character then stores 1 for to the right and -1 for to the left
        if (isGrounded) //checks if grounded
        {
            timer += Time.deltaTime; //starts a timer that updates with framerate
            if (timer > waittime) //if timer is higher than wait time
            {
                if (direction == 1) //checks direction then rotates the model to face player
                {
                    rb2d.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    rb2d.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                isGrounded = false;
                rb2d.AddForce(new Vector2(moveSpeed * direction, jumpSpeed), ForceMode2D.Impulse); //moves towards the player
            }
        }
        else
        {
            timer = 0.0f; //resets timer
        }
    }
}

using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rb2d;

    public int damage = 1;
    public float bulletSpeed = 5f;
    public Vector2 bulletDirection;
    float destroyTime;
    float bulletLife = 3f;
    public float destroyDelay;
    public GameObject bulletPrefab;
    public Transform BulletShootpos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime += Time.deltaTime;
        if (destroyTime > bulletLife) //destroys the bullet once its been instantiated after a certain period
        {
            Destroy(gameObject);
        }
    }
    public void SetBulletSpeed (float speed)
    {
        this.bulletSpeed = speed;
    }
    public void SetBulletDirection (Vector2 direction)
    {
        this.bulletDirection = direction;
    }
    public void SetBulletDamage (int damage)
    {
        this.damage = damage;
    }
    public void SetDestroyDelay(float delay)
    {
        this.destroyDelay = delay;
    }
    public void Shoot()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = (bulletDirection * -1) * bulletSpeed; //bullet velocity is direction multiplied by -1 multiplied then by speed to determine direction
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //on collision with something with the tag "Enemy"
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null) //takes damage if enemy is not null
            {
                enemy.TakeDamage(this.damage);
            }
            Destroy(bulletPrefab, 0.01f); //destroys bullet after one hundredth of a second
        }
    }
}

using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int bulletDamage;
    public PistolScript pistol;
    public PlayerControl player;
    public Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pistol = GetComponent<PistolScript>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBulletDamage(int damage)
    {
        this.bulletDamage = damage;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
        }
        Destroy(this.gameObject, 0.05f);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject, 0.1f);
    }
}

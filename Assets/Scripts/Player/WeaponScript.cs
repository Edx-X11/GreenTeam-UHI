using JetBrains.Annotations;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class PistolScript : MonoBehaviour, IShoot //Inherits from the interface with the abstract function Shoot() and the class Weapon
{
    public Weapon gun;
    public Transform BulletShootpos;
    public GameObject BulletPrefab;
    public Rigidbody2D rb2dBullet;
    public PlayerControl player;
    public bool weapon1 = false;
    public bool weapon2 = false;
    void Start()
    {
        gun = new Weapon(1, 17, 0.5f, false, 0f); //sets parameters of pistol
    }
    
    private void Update()
    {
        if (gun.isFiring) //if the weapon has fired begins counting
        {
            gun.lastFired += Time.deltaTime;
        }
        if (gun.lastFired > gun.fireDelay) //if the last time fired is greater than the cooldown the weapon can fire again
        {
            gun.isFiring = false;
            gun.lastFired = 0f;
        }
    }
    public void Shoot(bool facingLeft) //instantiates bullet and adds rigidbody and velocity
    {
        rb2dBullet = GetComponent<Rigidbody2D>();
        if (!gun.isFiring) //if gun isn't already firing and the cooldown is reset, fire gun
        {
            gun.isFiring = true; //disable ability to shoot
            GameObject bullet = Instantiate(BulletPrefab, BulletShootpos.transform.position, Quaternion.identity);
            bullet.name = BulletPrefab.name; //changes the name of the created prefab 
            Vector2 direction = facingLeft ? Vector2.left : Vector2.right; //checks direction
            bullet.GetComponent<Rigidbody2D>().linearVelocity = (direction * -1) * gun.bulletSpeed; //adds velocity based on the way the player is facing
            bullet.GetComponent<BulletScript>().SetBulletDamage(gun.bulletDamage); //sets bullet damage in BulletScript
        }
    } 
}
interface IShoot //abstract function with "facingLeft" parameter sent from playerControl
{
    void Shoot(bool facingLeft);
}
public class Weapon
{
    public int bulletDamage;
    public int bulletSpeed;
    public float fireDelay;
    public bool isFiring = false;
    public float lastFired;
    public Weapon(int damage, int speed, float delay, bool firing, float last) //constructor for shooting parameters
    {
        bulletDamage = damage;
        bulletSpeed = speed;
        fireDelay = delay;
        isFiring = firing;
        lastFired = last;
    }
}
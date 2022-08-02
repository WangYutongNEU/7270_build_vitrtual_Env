using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    
    public AudioClip hitEnemyClip;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 1f);
        
    }
    
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.AudioPlay(hitEnemyClip);
            Destroy(other.gameObject);
        }
    
        Destroy(gameObject);
    }
}

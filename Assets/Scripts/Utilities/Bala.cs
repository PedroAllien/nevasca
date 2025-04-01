using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    GameObject target;
    public float Speed;
    Rigidbody2D bulletRb;
    Collision2D collision;
    CircleCollider2D circle;
    public GameObject Explosao;
    public AudioClip Explosion;

    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * Speed;
        bulletRb.linearVelocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 1f);
    }
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Instantiate(Explosao, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            if (!SoundManager.audioSrc.isPlaying)
            {
                SoundManager.PlaySound(Explosion);
            }
            Destroy(this.gameObject, 0.1f);
        }
        if (collision.gameObject.layer == 7)
        {
            Instantiate(Explosao, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            if (!SoundManager.audioSrc.isPlaying)
            {
                SoundManager.PlaySound(Explosion);
            }
            Destroy(this.gameObject, 0.01f);
        }
        if (collision.gameObject.layer == 9)
        {
            Instantiate(Explosao, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            if (!SoundManager.audioSrc.isPlaying)
            {
                SoundManager.PlaySound(Explosion);
            }
            Destroy(this.gameObject, 0.1f);
        }
    }
}

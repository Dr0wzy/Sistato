using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float death_timer;
    [SerializeField] int damage;

    [SerializeField] private GameObject hit_effect_bullet;

    public enum Type
    {
        Player,
        Enemy
    }
    [SerializeField] Type type;

    void Start()
    {
        Invoke(nameof(Death), death_timer);
    }

    void Update() 
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && type == Type.Player)
        {
            Instantiate(hit_effect_bullet, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
            Death();
        } if (collision.gameObject.tag == "Wall")
        {
            Death();
        } if (collision.gameObject.tag == "Player" && type == Type.Enemy)
        {
            Instantiate(hit_effect_bullet, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Player>().Damage(damage);
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}

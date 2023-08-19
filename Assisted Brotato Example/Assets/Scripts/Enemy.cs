using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sr;
    protected Player player;

    [SerializeField] private GameObject hit_effect;

    [SerializeField] private int health;
    [SerializeField] private float stop_distance;
    [SerializeField] private float distance_to_run_out;
    [SerializeField] private float speed;
    public bool is_dead = false;

    private bool can_attack = false;

    Vector3 addRandPosToGo;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = Player.instance;

        StartCoroutine(nameof(SetRandomPos));
        EnemyOrder.Instance.Add(sr);
    }

    private void OnDestroy()
    {
        EnemyOrder.Instance.Dell(sr);
    }

    public virtual void Update()
    {
        if (is_dead || !player) return;
        if (player == null) return;
        Scale(player.transform.position);
    }

    private void FixedUpdate()
    {
        if (is_dead) return;
        if (player == null) return;
        if (player && Vector2.Distance(transform.position, player.transform.position) > stop_distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, speed * Time.fixedDeltaTime);
            anim.SetBool("Run", true);
            can_attack = false;
        } else if (player &&  Vector2.Distance(transform.position, player.transform.position) < distance_to_run_out)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, -speed * Time.fixedDeltaTime);
            anim.SetBool("Run", true);
            can_attack = false;
        } else
        {
            anim.SetBool("Run", false);
            can_attack = true;
        }
    }

    public void Damage(int damage)
    {
        if (is_dead) return;
        health -= damage;

        Instantiate(hit_effect, transform.position, Quaternion.identity);


        if (health <= 0)
        {
            Death();
        }
    }

    private void Scale(Vector3 pos)
    {
        if (pos.x >= transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        } else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    protected void Death()
    {
        is_dead = true;
        GetComponent<CircleCollider2D>().enabled = false;
        anim.SetTrigger("Death");
    }

    public virtual bool CheckIfCanAttack()
    {
        return can_attack && !is_dead;
    }

    public IEnumerator DestroyObj()
    {
        while (sr.color.a > 0)
        {
            float p = sr.color.a;
            sr.color = new Color(255,255,255,p - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    public IEnumerator SetRandomPos()
    {
        addRandPosToGo = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(nameof(SetRandomPos));
    }
}

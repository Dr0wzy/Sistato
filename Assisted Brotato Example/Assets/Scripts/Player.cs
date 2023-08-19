using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    [SerializeField] Sprite[] sprites_muzzle_flash;
    [SerializeField] SpriteRenderer muzzle_flash_sr;

    [SerializeField] Slider health_slider;
    [SerializeField] Slider dash_slider;

    [SerializeField] float dash_force;
    [SerializeField] float time_between_dash;
    [SerializeField] float dash_time;

    float dash_timer;
    bool is_dashing = false;
    Vector2 move_input;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shoot_pos;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject player;

    [SerializeField] private float speed;

    [SerializeField] private int health;
    private int max_health;

    [SerializeField] private float time_btw_shoot;
    private float shoot_timer;
    private Vector2 move_velocity;

    public static Player instance;


    void Awake()
    {
        instance = this;  
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        shoot_timer = time_btw_shoot;
        dash_timer = time_between_dash;

        max_health = health;
        UpdateHealthUI();
    }

    void FixedUpdate()
    {
        Move();
        if (is_dashing)
        {
            Dash();
        }
    }

    void Update()
    {
        shoot_timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && shoot_timer >= time_btw_shoot)
        {
            Shoot();
            shoot_timer = 0;
        }

        dash_timer += Time.deltaTime;
        dash_slider.value = dash_timer / time_between_dash;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dash_timer >= time_between_dash)
            {
                dash_timer = 0;
                ActivateDash();
            }
        }
    }

    private void Move()
    {
        move_input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move_input != Vector2.zero)
        {
            anim.SetBool("Run", true);
        } else
        {
            anim.SetBool("Run", false);
        }
        move_velocity = move_input.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move_velocity);
        ScalePlayer(move_input.x);

    }

    private void Shoot()
    {
        Instantiate(bullet, shoot_pos.position, shoot_pos.rotation);
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    private void ScalePlayer(float x)
    {
        if (x==1)
        {
            sr.flipX = false;
        } else if (x==-1)
        {
            sr.flipX = true;
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        CameraHandler.instance.ShakeCamera();

        UpdateHealthUI();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Dash()
    {
        rb.AddForce(move_input * Time.fixedDeltaTime * dash_force * 100);
    }

    private void ActivateDash()
    {
        is_dashing = true;
        Invoke(nameof(DeactivateDash), dash_time);
    }

    private void DeactivateDash()
    {
        is_dashing = false;
    }

    private void UpdateHealthUI()
    {
        health_slider.value = (float)health / max_health;
    }

    IEnumerator SetMuzzleFlash()
    {
        muzzle_flash_sr.sprite = sprites_muzzle_flash[Random.Range(0, sprites_muzzle_flash.Length)];
        muzzle_flash_sr.enabled = true;
        yield return new WaitForSeconds(0.05f);

        muzzle_flash_sr.enabled = false;
    }
}

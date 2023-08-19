using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    float timer;
    [SerializeField] float time_between_attack;
    [SerializeField] Transform shotPos;
    Transform player_t;
    [SerializeField] float min_recoil, max_recoil;
    [SerializeField] GameObject bullet;

    public override void Start()
    {
        base.Start();

        timer = time_between_attack;
        player_t = Player.instance.transform;
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (CheckIfCanAttack() && player)
        {
            if (timer >= time_between_attack)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 direction = player_t.position - shotPos.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotation.z += Random.Range(min_recoil, max_recoil);
        shotPos.rotation = rotation;

        Instantiate(bullet, shotPos.position, shotPos.rotation);
    }
}

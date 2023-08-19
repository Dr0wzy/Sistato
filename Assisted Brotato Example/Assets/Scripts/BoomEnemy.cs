using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemy : Enemy
{

    [SerializeField] float attack_radius;
    [SerializeField] LayerMask is_player;
    [SerializeField] int damage;
    [SerializeField] GameObject boom_effect;

    public override void Update()
    {
        base.Update();

        if (CheckIfCanAttack())
        {
            BoomAttack();
        }
    }

    void BoomAttack()
    {
        Collider2D[] detected_objects = Physics2D.OverlapCircleAll(transform.position, attack_radius, is_player);
        foreach (Collider2D item in detected_objects)
        {
            item?.GetComponent<Player>()?.Damage(damage);
        }
        Instantiate(boom_effect, transform.position, Quaternion.identity);
        Death();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attack_radius);
    }
}

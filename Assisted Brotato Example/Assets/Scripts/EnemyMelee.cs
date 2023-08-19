using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : Enemy
{

    private float timer;
    [SerializeField] private float time_between_attack;

    [SerializeField] private int damage;
    [SerializeField] private float attack_speed;

    [SerializeField] private ParticleSystem ps;

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (CheckIfCanAttack() && player)
        {
            if (timer >= time_between_attack )
            {
                timer = 0;
                StartCoroutine(nameof(Attack));
            }
        }
    }

    IEnumerator Attack()
    {
        
        Player.instance.Damage(damage);
        Vector2 player_pos = Player.instance.transform.position;
        Vector2 enemy_pos = transform.position;
        float counter = 0f;

        Instantiate(ps, Player.instance.transform.position, Quaternion.identity);

        while (counter <= 1)
        {
            counter += Time.deltaTime * attack_speed;
            float interpolation = (-Mathf.Pow(counter, 2) + counter) * 4;
            transform.position = Vector2.Lerp(enemy_pos, player_pos, interpolation);
            
            yield return null;
        }
    }
}

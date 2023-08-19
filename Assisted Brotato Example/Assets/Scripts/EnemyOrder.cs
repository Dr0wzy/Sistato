using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyOrder : MonoBehaviour
{

    public static EnemyOrder Instance;

    List<SpriteRenderer> enemy_sr = new List<SpriteRenderer>();

    public void Add(SpriteRenderer spp) { enemy_sr.Add(spp); }
    public void Dell(SpriteRenderer spp) { enemy_sr.Remove(spp); }

    float[] posYs;
    SpriteRenderer[] sprite_rends;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(nameof(Check));
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(1);
        int n = enemy_sr.Count;
        posYs = new float[n];
        sprite_rends = new SpriteRenderer[n];
        for (int i = 0; i < n; i++)
        {
            posYs[i] = enemy_sr[i].transform.position.y;
            sprite_rends[i] = enemy_sr[i];
        }
        Array.Sort(posYs, sprite_rends);
        for (int i = 0; i < sprite_rends.Length; i++)
        {
            sprite_rends[i].sortingOrder = i;
        }
        StartCoroutine(nameof(Check));
    }
}

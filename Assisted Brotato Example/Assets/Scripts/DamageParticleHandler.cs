using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticleHandler : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}

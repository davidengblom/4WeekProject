using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    public GameObject target;
    public bool playAnim;
    private void Update()
    {
        EngageTarget();
        Explode();
    }

    private void Awake()
    {
        target = GetComponentInParent<ProjectileAttack>().target;
    }
    private void EngageTarget()
    {
        //fly towards target
        playAnim = true;
    }
    private void Explode()
    {
        //explode on contact with target
    }
}

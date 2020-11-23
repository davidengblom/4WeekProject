using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingBehaviour : MonoBehaviour
{
    public float lifeTime = 5;
    public float ejectThrust;
    private Rigidbody rb;
    public GameObject casingExit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(casingExit.transform.forward * ejectThrust);
        StartCoroutine(Death(lifeTime));
    }
    IEnumerator Death(float life)
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}

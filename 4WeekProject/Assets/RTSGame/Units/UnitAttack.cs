using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSGame.Units;
using System;

public class UnitAttack : UnitInfo
{
    public AnimScript animScript;
    public Collider[] objectsInRange; // this array checks for all objects around this game object
    new public List<GameObject> targetList;
    public GameObject[] targets; // this array will contain targets that will be attacked
    [SerializeField]
    private string otherTeam;
    public GameObject target;
    private Vector3 relativePos;
    private void Update()
    {
        FindTargets();
        SetTarget();
        LookTowardsTarget();
        AttackTarget();
    }
    private void FindTargets()
    {
        if (Physics.OverlapSphere(transform.position, range).Length >= 0) //this actually 
        {
            objectsInRange = Physics.OverlapSphere(transform.position, range);
            foreach (var item in objectsInRange)
            {
                if (item.gameObject.CompareTag(otherTeam) && targetList.Contains(item.gameObject) == false) //checks if items inside of objectsinrange have the correct tag
                {
                    targetList.Add(item.gameObject);
                }
            }
        }
    }
    private void SetTarget()
    {
        target = targetList[targetList.Count - targetList.Count];
        if (target == null)
        {
            targetList.Remove(target);
        }
    }
    private void LookTowardsTarget()
    {
        if (target != null)
        {
            relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
        else
        {
            target = null;
        }
    }
    private void AttackTarget()
    {
        animScript.PlayAnimation();
    }
}

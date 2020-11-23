using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public Collider[] objectsInRange; // this array checks for all objects around this game object
    public List<GameObject> targetList;
    [SerializeField]
    private string otherTeam;
    public GameObject target;
    private Vector3 relativePos;
    public bool playAnim;

    public GameObject rocketPrefab;
    public GameObject rocketExit;


    public int range;
    public float attackSpeed;
    public int damage;
    private float startShootCD;
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
        if (Time.time >= startShootCD + attackSpeed)
        {
            startShootCD = Time.time;
            StartCoroutine(TriggerAnim());
            if (target != null)
            {
                Instantiate(rocketPrefab, rocketExit.transform);
            }
            else return;
        }
    }

    IEnumerator TriggerAnim()
    {
        playAnim = true;
        yield return new WaitForSeconds(0.5f);
        playAnim = false;
    }
}

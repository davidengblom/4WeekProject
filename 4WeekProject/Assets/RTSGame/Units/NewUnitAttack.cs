using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSGame.Units;

public class NewUnitAttack : MonoBehaviour
{
    public Collider[] objectsInRange; // this array checks for all objects around this game object
    public List<GameObject> targetList;
    [SerializeField]
    private string otherTeam;
    public GameObject target;
    private Vector3 relativePos;
    public bool playAnim;


    public int range;
    public float attackSpeed;
    public int damage;
    public GameObject casingPref;
    public GameObject casingExit;

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
                target.GetComponent<UnitInfo>().TakeDamage(damage);
            }
            else return;
        }
    }

    IEnumerator TriggerAnim()
    {
        playAnim = true;
        Instantiate(casingPref, new Vector3(casingExit.transform.position.x, casingExit.transform.position.y, casingExit.transform.position.z), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), null);
        casingPref.GetComponent<CasingBehaviour>().casingExit = casingExit;
        yield return new WaitForSeconds(0.5f);
        playAnim = false;
    }
}

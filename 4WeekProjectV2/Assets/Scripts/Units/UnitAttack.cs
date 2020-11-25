using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitAttack : MonoBehaviour
    {
        private UnitInfo _info;
        
        private List<GameObject> _targetList = new List<GameObject>();

        public GameObject target;
        public float range;
        private Quaternion _initialRot;
        public int damage;
        public float cd;
        
        private void Start()
        {
            this._info = GetComponent<UnitInfo>();
            InvokeRepeating(nameof(UpdateTarget), 0, 1);
        }

        private void Awake()
        {
            this._initialRot = this.transform.rotation;
        }

        private void UpdateTarget()
        {
            var hitColliders = Physics.OverlapSphere(this.transform.position, this.range, LayerMask.GetMask("Unit"));

            foreach (var obj in hitColliders)
            {
                if (obj.GetComponent<UnitInfo>().team != this._info.team)
                {
                    this.target = obj.gameObject;
                    InvokeRepeating(nameof(FocusTarget), 0, 0.01f);
                    InvokeRepeating(nameof(AttackTarget), 0, this.cd);
                }
            }
        }

        private void FocusTarget()
        {
            if (this.target)
            {
                var relativePos = this.target.transform.position - this.transform.position;
                var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                this.transform.rotation = rotation;
            }
            else
            {
                this.transform.rotation = this._initialRot;
                CancelInvoke(nameof(FocusTarget));
            }
        }

        private void AttackTarget()
        {
            if (this.target)
            {
                this.target.GetComponent<UnitInfo>().TakeDamage(this.damage);
                Debug.Log($"Dealt {this.damage} damage!");
            }
        }

        public void ResetTarget()
        {
            var nextTarget = this._targetList[this._targetList.Count - this._targetList.Count];
            this.target = nextTarget;
        }
    }
}

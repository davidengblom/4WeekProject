using System;
using UnityEngine;

namespace RTSGame.Units
{
    public enum UnitState { Selected, Unselected }
    public class UnitInfo : MonoBehaviour
    {
        public UnitState state;
        
        [Header("Unit Stats")]
        public int maxHealth = 100;
        public int _currentHealth;
        
        public int damage;
        
        public float attackSpeed;
        
        public float range;
        public float cost;

        internal Camera Cam;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }
        private void Start()
        {
            Cam = Camera.main;
        }
        public void TakeDamage(int dmg)
        {
            _currentHealth -= dmg;
            if(_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

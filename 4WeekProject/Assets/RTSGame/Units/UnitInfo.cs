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
        private int _currentHealth;
        
        public int damage;
        
        public float moveSpeed;
        public float attackSpeed;
        
        public float range;
        public float cost;

        internal Camera Cam;

        private void Start()
        {
            Cam = Camera.main;
        }

        public void TakeDamage(int dmg)
        {
            switch (_currentHealth - dmg <= 0)
            {
                case true:
                    //die
                    break;
                case false:
                    _currentHealth -= dmg;
                    break;
            }
        }
    }
}

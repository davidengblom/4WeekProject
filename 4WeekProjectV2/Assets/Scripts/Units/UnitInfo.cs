using UnityEngine;

namespace Units
{
    public enum UnitState { Selected, Unselected }
    public enum UnitTeam { Team1, Team2 };
    public class UnitInfo : MonoBehaviour
    {
        public UnitState state;
        public UnitTeam team;
        
        [Header("Unit Stats")]
        public int maxHealth = 100;
        public int currentHealth;
        
        public int damage;
        
        public float attackSpeed;
        
        public float range;
        public float cost;

        internal Camera Cam;

        private void Awake()
        {
            this.currentHealth = this.maxHealth;
        }
        private void Start()
        {
            this.Cam = Camera.main;
        }
        
        public void TakeDamage(int dmg)
        {
            switch (this.currentHealth - dmg <= 0)
            {
                case true:
                    Destroy(this.gameObject);
                    break;
                case false:
                    this.currentHealth -= dmg;
                    break;
            }
        }
    }
}

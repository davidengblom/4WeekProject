using System.Linq;
using Misc;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : MonoBehaviour
    {
        private UnitInfo _info;

        private SelectableBehaviour _selectableScript;
        private SelectionManager _manager;

        private Vector3 _startVector;
        private Vector3 _center;

        private bool GetSelectStatus()
        {
            return SelectionManager.Selectables.Contains(this._selectableScript);
        }
        
        private NavMeshAgent _agent;
        private int _ground;

        private void Awake()
        {
            this._manager = GameObject.Find("Camera").GetComponent<SelectionManager>();
            this._selectableScript = GetComponent<SelectableBehaviour>();
            this._info = GetComponent<UnitInfo>();
        }

        private void Start()
        {
            this._agent = GetComponent<NavMeshAgent>();
            this._ground = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            //Make unit faster if further away from target destination **

            if (Input.GetMouseButton(1) && this._info.state == UnitState.Selected)
            {
                this._startVector = this.transform.position - GetCenter();
                SetDestination();
            }
        }

        private void SetDestination()
        {
            var ray = this._info.Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, this._info.Cam.farClipPlane, this._ground) && GetSelectStatus())
            {
                this._agent.destination = hit.point + this._startVector;
            }
        }

        private Vector3 GetCenter()
        {
            var total = new Vector3();

            total = this._manager.Selected.Aggregate(total, (current, unit) => current + unit.transform.position);

            return total / this._manager.Selected.Count;
        }
    }
}

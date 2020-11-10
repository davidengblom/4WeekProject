using System;
using System.Linq;
using RTSGame.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace RTSGame.Units
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
            return SelectionManager.Selectables.Contains(_selectableScript);
        }
        
        private NavMeshAgent _agent;
        private int _ground;

        private void Awake()
        {
            _manager = GameObject.Find("Camera").GetComponent<SelectionManager>();
            _selectableScript = GetComponent<SelectableBehaviour>();
            _info = GetComponent<UnitInfo>();
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _ground = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            //Make unit faster if further away from target destination **

            if (Input.GetMouseButton(2) && _info.state == UnitState.Selected)
            {
                _startVector = transform.position - GetCenter();
                SetDestination();
            }
        }

        private void SetDestination()
        {
            var ray = _info.Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, _info.Cam.farClipPlane, _ground) && GetSelectStatus())
            {
                _agent.destination = hit.point + _startVector;
            }
        }

        private Vector3 GetCenter()
        {
            var total = new Vector3();

            total = _manager.Selected.Aggregate(total, (current, unit) => current + unit.transform.position);

            return total / _manager.Selected.Count;
        }
    }
}

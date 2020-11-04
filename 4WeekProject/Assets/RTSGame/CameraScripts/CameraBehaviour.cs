using System;
using UnityEngine;

namespace RTSGame.CameraScripts
{
    public class CameraBehaviour : MonoBehaviour
    {
        //To do list:
        //-Be able to move camera with WASD
        //-Be able to move camera with edges of the screen
        //-Be able to rotate camera with either right click or middle mouse button

        public float moveSpeed = 15f;
        public float zoomSpeed = 25f;
        public float rotateSpeed = 15f;

        public float minZoomDistance = 10f;
        public float maxZoomDistance = 50f;

        private Camera _camera;
        public Transform cameraAnchor;

        private void Awake()
        {
            _camera = Camera.main;
            if (!(_camera is null))
                cameraAnchor = _camera.transform.Find("Anchor");
        }

        private void Update()
        {
            Move();
            Zoom();
            Rotate();
            //Reset();
        }

        private void Move()
        {
            var xInput = Input.GetAxis("Horizontal");
            var zInput = Input.GetAxis("Vertical");
            var direction = cameraAnchor.transform.forward * zInput + _camera.transform.right * xInput;

            _camera.transform.position += direction * (moveSpeed * Time.deltaTime);
        }

        private void Zoom()
        {
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            var distance = Vector3.Distance(transform.position, _camera.transform.position);

            if (distance < minZoomDistance && scrollInput > 0.0f)
                return;
            if (distance > maxZoomDistance && scrollInput < 0.0f)
                return;

            _camera.transform.position += _camera.transform.forward * (scrollInput * zoomSpeed);
        }

        private void Rotate()
        {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                _camera.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
            }
            else if (!Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        // private void Reset()
        // {
        //     if (Input.GetMouseButtonDown(2))
        //     {
        //         transform.position = new Vector3(0, 0, 0);
        //         _camera.transform.position = new Vector3(0, 20, -17);
        //         _camera.transform.rotation = Quaternion.Euler(50, 0, 0);
        //         cameraAnchor.position = new Vector3(0, 0, 0);
        //         cameraAnchor.rotation = Quaternion.Euler(-45, 0, 0);
        //     }
        //     
        //     //Last Session Here
        // }
    }
}

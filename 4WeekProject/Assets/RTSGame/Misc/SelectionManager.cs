using System;
using System.Collections.Generic;
using System.Linq;
using RTSGame.Units;
using UnityEngine;

namespace RTSGame.Misc
{
    public class SelectionManager : MonoBehaviour
    {
        public UnitInfo info;
        
        public Texture topLeftBorder, bottomLeftBorder, topRightBorder, bottomRightBorder;

        private Texture2D _borderTexture;
        private Camera _cam;
        private Texture2D BorderTexture
        {
            get
            {
                if (_borderTexture != null) return _borderTexture;
                _borderTexture = new Texture2D(1, 1);
                _borderTexture.SetPixel(0, 0, Color.white);
                _borderTexture.Apply();

                return _borderTexture;
            }
        }

        private bool _selectionStarted = false;
        private Vector3 _mousePosition1;
        
        public static List<SelectableBehaviour> Selectables = new List<SelectableBehaviour>();
        public List<int> _selectedObjects = new List<int>();
        public List<GameObject> Selected = new List<GameObject>();
        
        private void Awake()
        {
            _cam = Camera.main;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _selectionStarted = true;
                _mousePosition1 = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectionStarted = false;
            }

            if (_selectionStarted)
            {
                //Unoptimized
                foreach (var t in Selectables.Where(t => t.GetComponent<UnitInfo>() != null))
                {
                    t.GetComponent<UnitInfo>().state = UnitState.Unselected;
                }
                _selectedObjects.Clear();
                Selected.Clear();
                for (var i = 0; i < Selectables.Count; i++)
                {
                    var viewportBounds = GetViewportBounds(_cam, _mousePosition1, Input.mousePosition);
                    if (viewportBounds.Contains(_cam.WorldToViewportPoint(Selectables[i].transform.position)))
                    {
                        _selectedObjects.Add(i);
                        Selected.Add(Selectables[i].gameObject);
                        //Unoptimized
                        Selectables[i].GetComponent<UnitInfo>().state = UnitState.Selected;
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (_selectionStarted)
            {
                var rect = GetScreenRect(_mousePosition1, Input.mousePosition);
                DrawScreenRectBorder(rect, 2, Color.cyan);
            }

            if (_selectedObjects.Count > 0)
            {
                foreach (var obj in _selectedObjects)
                {
                    DrawSelectionIndicator(_cam, Selectables[obj].GetObjectBounds());
                }
            }
        }

        private void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            //Top
            DrawBorderRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            //Left
            DrawBorderRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            //Right
            DrawBorderRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            //Left
            DrawBorderRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        private void DrawBorderRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, BorderTexture);
            GUI.color = Color.white;
        }

        private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;

            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);

            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        private Bounds GetViewportBounds(Camera cam, Vector3 screenPosition1, Vector3 screenPosition2)
        {
            var v1 = _cam.ScreenToViewportPoint(screenPosition1);
            var v2 = _cam.ScreenToViewportPoint(screenPosition2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = _cam.nearClipPlane;
            max.z = _cam.farClipPlane;
            
            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }

        private void DrawSelectionIndicator(Camera cam, Bounds bounds)
        {
            var boundPoint1 = bounds.min;
            var boundPoint2 = bounds.max;
            var boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
            var boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
            var boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
            var boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
            var boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
            var boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);
            
            var screenPoints = new Vector2[8];
            screenPoints[0] = _cam.WorldToScreenPoint(boundPoint1);
            screenPoints[1] = _cam.WorldToScreenPoint(boundPoint2);
            screenPoints[2] = _cam.WorldToScreenPoint(boundPoint3);
            screenPoints[3] = _cam.WorldToScreenPoint(boundPoint4);
            screenPoints[4] = _cam.WorldToScreenPoint(boundPoint5);
            screenPoints[5] = _cam.WorldToScreenPoint(boundPoint6);
            screenPoints[6] = _cam.WorldToScreenPoint(boundPoint7);
            screenPoints[7] = _cam.WorldToScreenPoint(boundPoint8);
            
            var topLeftPosition = Vector2.zero;
            var topRightPosition = Vector2.zero;
            var bottomLeftPosition = Vector2.zero;
            var bottomRightPosition = Vector2.zero;

            for (var a = 0; a < screenPoints.Length; a++)
            {
                //Top Left
                if (topLeftPosition.x == 0 || topLeftPosition.x > screenPoints[a].x)
                {
                    topLeftPosition.x = screenPoints[a].x;
                }
                if (topLeftPosition.y == 0 || topLeftPosition.y > Screen.height - screenPoints[a].y)
                {
                    topLeftPosition.y = Screen.height - screenPoints[a].y;
                }
                //Top Right
                if (topRightPosition.x == 0 || topRightPosition.x < screenPoints[a].x)
                {
                    topRightPosition.x = screenPoints[a].x;
                }
                if (topRightPosition.y == 0 || topRightPosition.y > Screen.height - screenPoints[a].y)
                {
                    topRightPosition.y = Screen.height - screenPoints[a].y;
                }
                //Bottom Left
                if (bottomLeftPosition.x == 0 || bottomLeftPosition.x > screenPoints[a].x)
                {
                    bottomLeftPosition.x = screenPoints[a].x;
                }
                if (bottomLeftPosition.y == 0 || bottomLeftPosition.y < Screen.height - screenPoints[a].y)
                {
                    bottomLeftPosition.y = Screen.height - screenPoints[a].y;
                }
                //Bottom Right
                if (bottomRightPosition.x == 0 || bottomRightPosition.x < screenPoints[a].x)
                {
                    bottomRightPosition.x = screenPoints[a].x;
                }
                if (bottomRightPosition.y == 0 || bottomRightPosition.y < Screen.height - screenPoints[a].y)
                {
                    bottomRightPosition.y = Screen.height - screenPoints[a].y;
                }
            }
            
            GUI.DrawTexture(new Rect(topLeftPosition.x - 16, topLeftPosition.y - 16, 16, 16), topLeftBorder);
            GUI.DrawTexture(new Rect(topRightPosition.x, topRightPosition.y - 16, 16, 16), topRightBorder);
            GUI.DrawTexture(new Rect(bottomLeftPosition.x - 16, bottomLeftPosition.y, 16, 16), bottomLeftBorder);
            GUI.DrawTexture(new Rect(bottomRightPosition.x, bottomRightPosition.y, 16, 16), bottomRightBorder);
        }
    }
}

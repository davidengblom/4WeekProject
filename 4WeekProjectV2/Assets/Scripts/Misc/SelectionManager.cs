using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Misc
{
    public class SelectionManager : MonoBehaviour
    {
        public Texture topLeftBorder, bottomLeftBorder, topRightBorder, bottomRightBorder;

        private Texture2D _borderTexture;
        private Camera _cam;
        private Texture2D BorderTexture
        {
            get
            {
                if (this._borderTexture != null) return this._borderTexture;
                this._borderTexture = new Texture2D(1, 1);
                this._borderTexture.SetPixel(0, 0, Color.white);
                this._borderTexture.Apply();

                return this._borderTexture;
            }
        }

        private bool _selectionStarted = false;
        private Vector3 _mousePosition1;
        
        public static List<SelectableBehaviour> Selectables = new List<SelectableBehaviour>();
        public List<int> _selectedObjects = new List<int>();
        public List<GameObject> Selected = new List<GameObject>();
        
        private void Awake()
        {
            this._cam = Camera.main;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this._selectionStarted = true;
                this._mousePosition1 = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                this._selectionStarted = false;
            }

            if (this._selectionStarted)
            {
                //Unoptimized
                foreach (var t in Selectables.Where(t => t.GetComponent<UnitInfo>()))
                {
                    t.GetComponent<UnitInfo>().state = UnitState.Unselected;
                }
                this._selectedObjects.Clear();
                this.Selected.Clear();
                for (var i = 0; i < Selectables.Count; i++)
                {
                    var viewportBounds = GetViewportBounds(this._cam, this._mousePosition1, Input.mousePosition);
                    if (viewportBounds.Contains(this._cam.WorldToViewportPoint(Selectables[i].transform.position)))
                    {
                        this._selectedObjects.Add(i);
                        this.Selected.Add(Selectables[i].gameObject);
                        Selectables[i].GetComponent<UnitInfo>().state = UnitState.Selected;
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (this._selectionStarted)
            {
                var rect = GetScreenRect(this._mousePosition1, Input.mousePosition);
                DrawScreenRectBorder(rect, 2, Color.cyan);
            }

            if (this._selectedObjects.Count > 0)
            {
                foreach (var obj in this._selectedObjects)
                {
                    DrawSelectionIndicator(this._cam, Selectables[obj].GetObjectBounds());
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
            GUI.DrawTexture(rect, this.BorderTexture);
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
            var v1 = this._cam.ScreenToViewportPoint(screenPosition1);
            var v2 = this._cam.ScreenToViewportPoint(screenPosition2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = this._cam.nearClipPlane;
            max.z = this._cam.farClipPlane;
            
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
            screenPoints[0] = this._cam.WorldToScreenPoint(boundPoint1);
            screenPoints[1] = this._cam.WorldToScreenPoint(boundPoint2);
            screenPoints[2] = this._cam.WorldToScreenPoint(boundPoint3);
            screenPoints[3] = this._cam.WorldToScreenPoint(boundPoint4);
            screenPoints[4] = this._cam.WorldToScreenPoint(boundPoint5);
            screenPoints[5] = this._cam.WorldToScreenPoint(boundPoint6);
            screenPoints[6] = this._cam.WorldToScreenPoint(boundPoint7);
            screenPoints[7] = this._cam.WorldToScreenPoint(boundPoint8);
            
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
            
            GUI.DrawTexture(new Rect(topLeftPosition.x - 16, topLeftPosition.y - 16, 16, 16), this.topLeftBorder);
            GUI.DrawTexture(new Rect(topRightPosition.x, topRightPosition.y - 16, 16, 16), this.topRightBorder);
            GUI.DrawTexture(new Rect(bottomLeftPosition.x - 16, bottomLeftPosition.y, 16, 16), this.bottomLeftBorder);
            GUI.DrawTexture(new Rect(bottomRightPosition.x, bottomRightPosition.y, 16, 16), this.bottomRightBorder);
        }
    }
}

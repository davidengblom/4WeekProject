using UnityEngine;

namespace Misc
{
    public class SelectableBehaviour : MonoBehaviour
    {
        public Renderer[] renderers;

        public Bounds GetObjectBounds()
        {
            var totalBounds = new Bounds();

            foreach (var render in this.renderers)
            {
                if (totalBounds.center == Vector3.zero)
                {
                    totalBounds = render.bounds;
                }
                else
                {
                    totalBounds.Encapsulate(render.bounds);
                }
            }

            return totalBounds;
        }

        private void OnEnable()
        {
            if (!SelectionManager.Selectables.Contains(this))
            {
                SelectionManager.Selectables.Add(this);
            }
        }

        private void OnDisable()
        {
            if (SelectionManager.Selectables.Contains(this))
            {
                SelectionManager.Selectables.Remove(this);
            }
        }
    }
}

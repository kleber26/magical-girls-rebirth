using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Characters.Input
{
    public class InputController : MonoBehaviour
    {
        [CanBeNull]
        public GameObject SelectedTileGo()
        {
            Ray? ray = ScreenPointPosition();

            if (ray is null)
            {
                return null;
            }

            RaycastHit hit;

            if (Physics.Raycast((Ray) ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")) && !EventSystem.current.currentSelectedGameObject)
            {

                if (hit.collider.CompareTag("Tile"))
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        // Returns a Raycast from screen position based on touch or mouse position
        private Ray? ScreenPointPosition()
        {
            Vector3 screenPosition = new Vector3();

            if (Application.isEditor && UnityEngine.Input.GetMouseButton(0))
            {
                screenPosition = UnityEngine.Input.mousePosition;
            }
            else if (UnityEngine.Input.touchCount > 0)
            {
                screenPosition = UnityEngine.Input.GetTouch(0).position;
            }
            else
            {
                return null;
            }

            return Camera.main.ScreenPointToRay(screenPosition);
        }
    }
}

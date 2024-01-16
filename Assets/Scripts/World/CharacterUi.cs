using UnityEngine;

namespace World
{
    public class CharacterUi : MonoBehaviour
    {
        private GameObject canvasCharHpText;
        private GameObject canvasPlayerFlagText;

        void Awake()
        {
            GetChildren();
        }

        void Update()
        {
            LookAtCamera(canvasCharHpText);
            LookAtCamera(canvasPlayerFlagText);
        }

        private void LookAtCamera(GameObject go)
        {
            go.transform.LookAt(go.transform.position + Camera.main.transform.forward);
        }

        private void GetChildren()
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("CanvasCharHP"))
                {
                    canvasCharHpText = child.gameObject;
                }
                else if (child.CompareTag("CanvasPlayerFlag"))
                {
                    canvasPlayerFlagText = child.gameObject;
                }
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] Transform gameCamera;
        [SerializeField] float smoothMovementSpeed = 0.4f;
        [SerializeField] float rotationSpeed = 0.05f;
        [SerializeField] Vector3 rotationOffset;
        [SerializeField] Vector3 positionOffset;

        private GameObject playerGo;
        private Vector3 middleMapTile;
        private Vector3 velocity = Vector3.zero;

        // remove serialize
        private Vector3 currentTarget;
        [SerializeField] private Vector3 currentPositionOffset;
        [SerializeField] private Vector3 currentRotationOffset;
        [SerializeField] private float currentSmoothMovementSpeed;
        [SerializeField] private float currentrotationSpeed;

        [SerializeField] private bool targetPlayer;

        public void Initialize(GameObject playerGo, Vector3 middleMapPosition)
        {
            this.playerGo = playerGo;
            this.middleMapTile = middleMapPosition;

            currentSmoothMovementSpeed = smoothMovementSpeed;
            currentrotationSpeed = rotationSpeed;

            SetMapView();
        }

        void FixedUpdate()
        {
            currentTarget = targetPlayer ? playerGo.transform.position : middleMapTile;

            StartCoroutine(ChangeCameraSizeUp(4f));
            MoveCameraToPlayerPosition();
            RotateCameraToPlayerAngle();
        }

        public void SetCharacterView()
        {
            targetPlayer = true;
            currentPositionOffset = new Vector3(4.11f, 5.04f, -4.54f);
            currentRotationOffset = new Vector3(-1.56f, -1.99f, 1.9f);
        }

        public void SetInteractiveView()
        {
            targetPlayer = true;
            currentPositionOffset = new Vector3(2.25f, 8.05f, -3.8f);
            currentRotationOffset = new Vector3(-0.77f, -4.1f, 1.56f);
        }

        public void SetMapView()
        {
            targetPlayer = false;
            currentPositionOffset = new Vector3(-6.17f, 16.62f, 6.35f);
            currentRotationOffset = new Vector3(0.15f, -1.4f, 1.91f);
        }

        private void MoveCameraToPlayerPosition()
        {
            Vector3 targetPosition = currentTarget + currentPositionOffset;
            gameCamera.position = Vector3.SmoothDamp(gameCamera.position, targetPosition, ref velocity, currentSmoothMovementSpeed);
            transform.LookAt(currentTarget);
        }

        private void RotateCameraToPlayerAngle()
        {
            Vector3 direction = (currentTarget - gameCamera.position).normalized;
            Quaternion rotationGoal = Quaternion.LookRotation(direction + currentRotationOffset);
            gameCamera.rotation = Quaternion.Slerp(gameCamera.rotation, rotationGoal, currentrotationSpeed);
        }

        private IEnumerator ChangeCameraSizeUp(float offset)
        {
            Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            while (camera.orthographicSize < offset)
            {
                camera.orthographicSize += 0.01f;
                yield return null;
            }
        }
        //
        // private IEnumerator ChangeCameraSizeDown(float offset)
        // {
        //     Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //     while (camera.orthographicSize > offset)
        //     {
        //         camera.orthographicSize -= 0.01f;
        //         yield return null;
        //     }
        // }
    }
}

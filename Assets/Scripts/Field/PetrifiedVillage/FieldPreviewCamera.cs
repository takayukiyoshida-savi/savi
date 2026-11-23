using UnityEngine;

namespace PetrifiedVillage
{
    /// <summary>
    /// ジオラマ風に見下ろす確認用カメラ。右ドラッグで回転、ホイールでズーム。
    /// </summary>
    public class FieldPreviewCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 target = new Vector3(0f, 1.5f, 2f);
        [SerializeField] private float distance = 58f;
        [SerializeField] private float pitch = 40f;
        [SerializeField] private float yaw = 0f;

        [SerializeField] private float rotateSpeed = 3f;
        [SerializeField] private float zoomSpeed = 20f;
        [SerializeField] private float minDistance = 12f;
        [SerializeField] private float maxDistance = 90f;
        [SerializeField] private float minPitch = 15f;
        [SerializeField] private float maxPitch = 80f;

        private void OnEnable()
        {
            ApplyTransform();
        }

        private void LateUpdate()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(1))
            {
                yaw += Input.GetAxis("Mouse X") * rotateSpeed;
                pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * rotateSpeed, minPitch, maxPitch);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
#endif
            ApplyTransform();
        }

        public void ApplyTransform()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.position = target - rotation * Vector3.forward * distance;
            transform.rotation = rotation;
        }
    }
}

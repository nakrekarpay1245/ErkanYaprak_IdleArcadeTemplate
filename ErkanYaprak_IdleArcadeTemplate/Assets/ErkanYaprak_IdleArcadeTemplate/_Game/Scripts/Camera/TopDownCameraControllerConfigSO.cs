using UnityEngine;

namespace _Game.Scripts.TopDownCamera
{
    [CreateAssetMenu(fileName = "TopDownCameraControllerData", menuName = "Data/TopDownCameraControllerData")]
    public class TopDownCameraControllerConfigSO : ScriptableObject
    {
        [Header("Camera FOV Settings")]
        [Tooltip("The default field of view (FOV) of the camera.")]
        [Range(30f, 100f)]
        [SerializeField] private float _defaultFOV = 60f;

        [Tooltip("The maximum field of view (FOV) when the target is moving.")]
        [Range(30f, 100f)]
        [SerializeField] private float _maxFOV = 75f;

        [Header("Camera Movement Settings")]
        [Tooltip("The smooth speed at which the camera root moves to follow the target.")]
        [Range(0.1f, 10f)]
        [SerializeField] private float _smoothSpeed = 0.125f;

        [Tooltip("The smooth speed at which the camera adjusts its FOV.")]
        [Range(0.1f, 10f)]
        [SerializeField] private float _zoomSpeed = 0.5f;

        [Header("Camera Offset Settings")]
        [Tooltip("The offset distance between the CameraRoot and the target.")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 10f, -10f);

        // Properties to access private fields
        public float DefaultFOV => _defaultFOV;
        public float MaxFOV => _maxFOV;
        public float SmoothSpeed => _smoothSpeed;
        public float ZoomSpeed => _zoomSpeed;
        public Vector3 Offset => _offset;
    }
}
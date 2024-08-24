using UnityEngine;
using _Game.Scripts.TopDownCharacter;

namespace _Game.Scripts.TopDownCamera
{
    /// <summary>
    /// This class handles the camera movement in a smooth top-down manner.
    /// It follows the target (typically the player) by adjusting the CameraRoot position and zooms in/out 
    /// by modifying the Camera's field of view (FOV) based on the target's movement speed.
    /// </summary>
    public class TopDownCameraController : MonoBehaviour
    {
        [Header("Camera Configuration")]
        [Tooltip("The configuration for camera settings.")]
        [SerializeField] private TopDownCameraControllerConfigSO _cameraConfig;

        [Tooltip("The target that the camera will follow.")]
        [SerializeField] private TopDownCharacterController _targetCharacter;

        [Tooltip("The Camera component attached to the child object for zooming.")]
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponentInChildren<Camera>();
            }

            _targetCharacter = FindObjectOfType<TopDownCharacterController>();

            // Initialize camera settings based on the ScriptableObject configuration
            _camera.fieldOfView = _cameraConfig.DefaultFOV;

            // Validate that the TopDownCharacterController is assigned
            if (_targetCharacter == null)
            {
                Debug.LogWarning("TopDownCharacterController is not assigned in" +
                    " TopDownCameraController.");
            }
        }

        private void LateUpdate()
        {
            if (_targetCharacter == null || _camera == null)
            {
                Debug.LogWarning("Target or Camera not set for TopDownCameraController.");
                return;
            }

            AdjustCameraFOV();
            HandleCameraRootMovement();
        }

        /// <summary>
        /// Adjusts the camera's field of view (FOV) based on the target's movement speed.
        /// </summary>
        private void AdjustCameraFOV()
        {
            float targetSpeed = _targetCharacter.Speed;

            // Lerp between the default and max FOV based on the target's speed
            float targetFOV = Mathf.Lerp(_cameraConfig.DefaultFOV, _cameraConfig.MaxFOV, targetSpeed);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, _cameraConfig.ZoomSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Smoothly moves the CameraRoot towards the target's position.
        /// </summary>
        private void HandleCameraRootMovement()
        {
            Vector3 desiredPosition = _targetCharacter.transform.position + _cameraConfig.Offset;

            // Smoothly move the CameraRoot from its current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _cameraConfig.SmoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
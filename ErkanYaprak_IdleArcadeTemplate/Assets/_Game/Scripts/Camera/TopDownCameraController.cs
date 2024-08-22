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
        /// <summary>
        /// The target that the camera will follow.
        /// This is usually the player character.
        /// </summary>
        [Header("Camera Settings")]
        [Tooltip("The target that the camera will follow.")]
        [SerializeField] private TopDownCharacterController _targetCharacter;

        /// <summary>
        /// The Camera component attached to the child object for zooming.
        /// </summary>
        [Tooltip("The Camera component attached to the child object for zooming.")]
        [SerializeField] private Camera _camera;

        /// <summary>
        /// The default field of view (FOV) of the camera.
        /// </summary>
        [Tooltip("The default field of view (FOV) of the camera.")]
        [SerializeField] private float _defaultFOV = 60f;

        /// <summary>
        /// The maximum field of view (FOV) when the target is moving.
        /// </summary>
        [Tooltip("The maximum field of view (FOV) when the target is moving.")]
        [SerializeField] private float _maxFOV = 75f;

        /// <summary>
        /// The smooth speed at which the camera root moves to follow the target.
        /// </summary>
        [Tooltip("The smooth speed at which the camera root moves to follow the target.")]
        [Range(0.1f, 10f)]
        [SerializeField] private float _smoothSpeed = 0.125f;

        /// <summary>
        /// The smooth speed at which the camera adjusts its FOV.
        /// </summary>
        [Tooltip("The smooth speed at which the camera adjusts its FOV.")]
        [Range(0.1f, 10f)]
        [SerializeField] private float _zoomSpeed = 0.5f;

        /// <summary>
        /// The offset distance between the CameraRoot and the target.
        /// Adjust this to control how far the camera root is from the target.
        /// </summary>
        [Tooltip("The offset distance between the CameraRoot and the target.")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 10f, -10f);

        /// <summary>
        /// Initialize components.
        /// </summary>
        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponentInChildren<Camera>();
            }

            _camera.fieldOfView = _defaultFOV;
        }

        /// <summary>
        /// Update is called once per frame. It handles the camera root's position and the camera's FOV adjustments.
        /// </summary>
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
            float targetFOV = Mathf.Lerp(_defaultFOV, _maxFOV, targetSpeed);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, _zoomSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Smoothly moves the CameraRoot towards the target's position.
        /// </summary>
        private void HandleCameraRootMovement()
        {
            // Desired position is the target's position plus the offset
            Vector3 desiredPosition = _targetCharacter.transform.position + _offset;

            // Smoothly move the CameraRoot from its current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
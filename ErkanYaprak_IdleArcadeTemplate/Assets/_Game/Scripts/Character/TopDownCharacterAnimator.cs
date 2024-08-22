using UnityEngine;

namespace _Game.Scripts.TopDownCharacter
{
    public class TopDownCharacterAnimator : MonoBehaviour
    {
        [Header("TopDownCharacterAnimator Parameters")]
        /// <summary>
        /// Reference to the Animator component.
        /// </summary>
        [Header("References")]
        [Tooltip("")]
        [SerializeField]
        private Animator _animator;
        [Tooltip("")]
        [SerializeField]
        private TopDownCharacterController _controller;
        [Header("Paramaters")]
        [SerializeField]
        private string _speedAnimatorParameterKey = "Speed";
        [SerializeField]
        private string _isHurtAnimatorParameterKey = "IsHurt";
        [SerializeField]
        private string _isDeadAnimatorParameterKey = "IsDead";
        [SerializeField]
        private string _isAttackAnimatorParameterKey = "IsAttack";
        [SerializeField]
        private string _isWinAnimatorParameterKey = "IsWin";

        //Animator Parameter Hashcodes
        private int _speedHashCode;
        private int _isHurtHashCode;
        private int _isDeadHashCode;
        private int _isAttackHashCode;
        private int _isWinHashCode;

        private void Awake()
        {
            _speedHashCode = Animator.StringToHash(_speedAnimatorParameterKey);
            _isHurtHashCode = Animator.StringToHash(_isHurtAnimatorParameterKey);
            _isDeadHashCode = Animator.StringToHash(_isDeadAnimatorParameterKey);
            _isAttackHashCode = Animator.StringToHash(_isAttackAnimatorParameterKey);
            _isWinHashCode = Animator.StringToHash(_isWinAnimatorParameterKey);

            _animator = GetComponent<Animator>();
            _controller = GetComponentInParent<TopDownCharacterController>();
        }

        private void Update()
        {
            SetAnimatorSpeed();
        }

        private void SetAnimatorSpeed()
        {
            _animator.SetFloat(_speedHashCode, _controller.Speed);
        }

        public void PlayHurtAnimation()
        {
            _animator.SetTrigger(_isHurtHashCode);
        }

        public void PlayDeadAnimation()
        {
            _animator.SetTrigger(_isDeadHashCode);
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger(_isAttackHashCode);
        }

        public void PlayWinAnimation()
        {
            _animator.SetTrigger(_isWinHashCode);
        }
    }
}
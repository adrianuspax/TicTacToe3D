using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Handlers;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.Inheritance
{
    public class AnimationHandler : MonoBehaviour
    {
        [InfoBox("Inheritance from " + nameof(TicTacToe3D) + "." + nameof(TicTacToe3D.Inheritance) +"."+ nameof(AnimationHandler))]
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [BoxGroup, SerializeField] private bool _isHandled; // Determines if the animation is handled manually.
        [BoxGroup, SerializeField, ShowIf(nameof(_isHandled))] private int _startingAnimationClipIndex; // Index of the starting animation clip.
        [BoxGroup, SerializeField, ShowIf(nameof(_isHandled))] private int _finishingAnimationClipIndex; // Index of the finishing animation clip.
        [BoxGroup, SerializeField, ShowIf(nameof(_isHandled))] private int _parameterIndex; // Index of the animator parameter.
        [Space(20, order = 0)]
        [Header(Header.READONLY, order = 1), HorizontalLine(order = 2)]
        [Space(-10, order = 3)]
        [Header(Header.variables, order = 4), HorizontalLine(order = 5)]
        [BoxGroup, SerializeField, ReadOnly] private bool _isToOpen;
        [Space(20, order = 0)]
        [Header(Header.components, order = 1), HorizontalLine(order = 2)]
        [BoxGroup, SerializeField, ReadOnly] private Animator _animator; // The Animator component.
        [Space(20, order = 0)]
        [Header(Header.scripts, order = 1), HorizontalLine(order = 2)]
        [BoxGroup, SerializeField, ReadOnly] private AnimatorHandler _animatorHandler; // Handler for the Animator.
        private Coroutine _coroutine; // Coroutine for delayed animations.
#if UNITY_EDITOR
        [Space(20, order = 0)]
        [InfoBox("Only Test!", InfoBoxType.Warning, order = 1)]
        [BoxGroup, SerializeField] private float _delay; // Delay for testing the animation.
        [BoxGroup, SerializeField] private bool _isToOpenTest; // Used to toggle the animation for testing.
        ///<inheritdoc/>
        protected virtual void Reset()
        {
            _startingAnimationClipIndex = 1;
            _finishingAnimationClipIndex = 2;
            _parameterIndex = 0;
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(SetAnimation), SButtonEnableMode.Playmode)]
        private void SetAnimation()
        {
            print($"{nameof(SetAnimation)}({nameof(_isToOpen)}: {_isToOpen}, {nameof(_delay)}: {_delay}) is called! _coroutine = {_c()}.");
            SetAnimation(_isToOpenTest, _delay);
            return;
            string _c()
            {
                if (_coroutine == null)
                    return "NULL!";
                else
                    return "NOT NULL!";
            }
        }
#endif
        ///<inheritdoc/>
        protected virtual void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        protected virtual void Start()
        {
            _coroutine = null;
            _isToOpen = false;

            if (_isHandled)
                return;

            _startingAnimationClipIndex = 1;
            _finishingAnimationClipIndex = 2;
            _parameterIndex = 0;
        }
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Inheritance")]
        public virtual void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref _animator);
            var isNecessaryUpdateInstance = _animatorHandler.IsNecessaryUpdateInstance();
            if (isNecessaryUpdateInstance)
                _animatorHandler = new(_animator);
        }
        /// <summary>
        /// Sets the animation state with an optional delay.
        /// </summary>
        /// <param name="isToOpen">The target state of the animation.</param>
        /// <param name="delay">The delay before the animation starts.</param>
        /// <returns></returns>
        public virtual Coroutine SetAnimation(bool isToOpen, float delay = 0f)
        {
            if (_coroutine == null)
            {
                var routine = PlayAnimation(isToOpen, delay);
                _coroutine = StartCoroutine(routine);
            }
            else
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                SetAnimation(isToOpen, delay);
            }
            return _coroutine;
        }
        /// <summary>
        /// Plays the animation after an optional delay.
        /// </summary>
        /// <param name="isToOpen">The target state of the animation.</param>
        /// <param name="delay">The delay before playing the animation.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected virtual IEnumerator PlayAnimation(bool isToOpen, float delay)
        {
            var duration = _animatorHandler.AnimationClips[isToOpen ? _startingAnimationClipIndex : _finishingAnimationClipIndex].length;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            _animator.SetBool(_animatorHandler.ParameterHandlers[_parameterIndex].ID, isToOpen);
            _isToOpen = isToOpen;
            yield return new WaitForSeconds(duration);
            _coroutine = null;
        }
        /// <summary>
        /// Assign a new instance to the variable. <see cref="_animator"/>.
        /// </summary>
        /// <remarks> This will assign a new instance to <see cref="_animatorHandler"/>. </remarks>
        protected void SetAnimatior(Animator animator)
        {
            _animator = animator;
            _animatorHandler = new(_animator);
        }

        protected void SetToOpen(bool value)
        {
            _isToOpen = value;
        }

        protected void SetHandled(bool value)
        {
            _isHandled = value;
        }

        protected void SetStartingAnimationClipIndex(int value)
        {
            _startingAnimationClipIndex = value;
        }

        protected void SetFinishingAnimationClipIndex(int value)
        {
            _finishingAnimationClipIndex = value;
        }

        protected void SetParameterIndex(int value)
        {
            _parameterIndex = value;
        }
        /// <summary>
        /// Gets a value indicating whether the animation is currently running.
        /// </summary>
        public bool IsRunning => _coroutine != null;
        /// <summary>
        /// Gets the handler for the Animator.
        /// </summary>
        public virtual bool IsToOpen => _isToOpen;

        public virtual AnimatorHandler AnimatorHandler => _animatorHandler;

        protected bool IsHandled => _isHandled;

        protected int StartingAnimationClipIndex => _startingAnimationClipIndex;

        protected int FinishingAnimationClipIndex => _finishingAnimationClipIndex;

        protected int ParameterIndex => _parameterIndex;
    }
}

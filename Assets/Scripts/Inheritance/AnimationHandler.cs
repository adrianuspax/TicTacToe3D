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
        [BoxGroup, SerializeField, ReadOnly] private bool _isRunning; // Is the animation currently running?
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
        [BoxGroup, SerializeField] private bool _isRunningTest; // Used to toggle the animation for testing.
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
            var s = SetAnimation(_isRunningTest, _delay);
            print($"{nameof(SetAnimation)}({nameof(_isRunning)}: {_isRunning}, {nameof(_delay)}: {_delay}) is called!  returns {_s()}.");

            string _s()
            {
                if (s == null)
                    return "null";
                return s.ToString();
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
            _isRunning = false;

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
        /// <param name="isRunning">The target state of the animation.</param>
        /// <param name="delay">The delay before the animation starts.</param>
        /// <returns>The duration of the animation clip, including the delay, or null if the state is already set.</returns>
        public virtual float? SetAnimation(bool isRunning, float delay = 0f)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                this._isRunning = !isRunning;
            }

            if (this._isRunning == isRunning)
                return null;
            var duration = _animatorHandler.AnimationClips[isRunning ? _startingAnimationClipIndex : _finishingAnimationClipIndex].length;
            if (delay > 0f)
                duration += delay;
            var routine = PlayAnimation(isRunning, delay);
            _coroutine = StartCoroutine(routine);
            isRunning.ComparativeAssignment(ref this._isRunning);
            return duration;
        }
        //TESTARRRRRRRR
        /*public virtual Coroutine SetAnimationProvisorio(bool isRunning, float delay = 0f)
        {
            if (_coroutine == null)
            {
                _play();
            }
            else
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                this._isRunning = !isRunning;
                _play();
            }
            return _coroutine;

            void _play()
            {
                var routine = PlayAnimation(isRunning, delay);
                _coroutine = StartCoroutine(routine);
            }
        }*/
        /// <summary>
        /// Plays the animation after an optional delay.
        /// </summary>
        /// <param name="isRunning">The target state of the animation.</param>
        /// <param name="delay">The delay before playing the animation.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected virtual IEnumerator PlayAnimation(bool isRunning, float delay)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            else
                yield return null;

            _animator.SetBool(_animatorHandler.ParameterHandlers[_parameterIndex].ID, isRunning);
            this._isRunning = isRunning;
            _coroutine = null;
        }

        protected void SetRunning(bool value)
        {
            _isRunning = value;
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
        public virtual bool IsRunning => _isRunning;
        /// <summary>
        /// Gets the handler for the Animator.
        /// </summary>
        public virtual AnimatorHandler AnimatorHandler => _animatorHandler;

        protected bool IsHandled => _isHandled;

        protected int StartingAnimationClipIndex => _startingAnimationClipIndex;

        protected int FinishingAnimationClipIndex => _finishingAnimationClipIndex;

        protected int ParameterIndex => _parameterIndex;

        protected Coroutine Coroutine
        {
            get => _coroutine;
            set => _coroutine = value;
        }
    }
}

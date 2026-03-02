using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Handlers;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.Animations
{
    public class Inheritance : MonoBehaviour
    {
        [InfoBox("Inheritance from " + nameof(Animations) +"."+ nameof(Inheritance))]
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [BoxGroup, SerializeField] protected bool isHandled; // Determines if the animation is handled manually.
        [BoxGroup, SerializeField, ShowIf(nameof(isHandled))] protected int startingAnimationClipIndex; // Index of the starting animation clip.
        [BoxGroup, SerializeField, ShowIf(nameof(isHandled))] protected int finishingAnimationClipIndex; // Index of the finishing animation clip.
        [BoxGroup, SerializeField, ShowIf(nameof(isHandled))] protected int parameterIndex; // Index of the animator parameter.
        [Space(20, order = 0)]
        [Header(Header.READONLY, order = 1), HorizontalLine(order = 2)]
        [Space(-10, order = 3)]
        [Header(Header.variables, order = 4), HorizontalLine(order = 5)]
        [BoxGroup, SerializeField, ReadOnly] protected bool isRunning; // Is the animation currently running?
        [Space(20, order = 0)]
        [Header(Header.components, order = 1), HorizontalLine(order = 2)]
        [BoxGroup, SerializeField, ReadOnly] protected Animator animator; // The Animator component.
        [Space(20, order = 0)]
        [Header(Header.scripts, order = 1), HorizontalLine(order = 2)]
        [BoxGroup, SerializeField, ReadOnly] protected AnimatorHandler animatorHandler; // Handler for the Animator.
        protected Coroutine _coroutine; // Coroutine for delayed animations.
#if UNITY_EDITOR
        [Space(20, order = 0)]
        [InfoBox("Only Test!", InfoBoxType.Warning, order = 1)]
        [BoxGroup, SerializeField] private float delay;
        [BoxGroup, SerializeField] private bool isRunning_;
        ///<inheritdoc/>
        protected virtual void Reset()
        {
            startingAnimationClipIndex = 1;
            finishingAnimationClipIndex = 2;
            parameterIndex = 0;
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(SetAnimation), SButtonEnableMode.Playmode)]
        private void SetAnimation()
        {
            var s = SetAnimation(isRunning_, delay);
            print($"{nameof(SetAnimation)}({nameof(isRunning)}: {isRunning}, {nameof(delay)}: {delay}) is called!  returns {_s()}.");

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
            isRunning = false;

            if (isHandled)
                return;
            startingAnimationClipIndex = 1;
            finishingAnimationClipIndex = 2;
            parameterIndex = 0;
        }
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Inheritance")]
        public virtual void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref animator);
            if (animatorHandler.IsNecessaryUpdateInstance())
                animatorHandler = new(animator);
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
                this.isRunning = !isRunning;
            }

            if (this.isRunning == isRunning)
                return null;
            var duration = animatorHandler.AnimationClips[isRunning ? startingAnimationClipIndex : finishingAnimationClipIndex].length;
            if (delay > 0f)
                duration += delay;
            var routine = PlayAnimation(isRunning, delay);
            _coroutine = StartCoroutine(routine);
            isRunning.ComparativeAssignment(ref this.isRunning);
            return duration;
        }
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
            animator.SetBool(animatorHandler.ParameterHandlers[parameterIndex].ID, isRunning);
            this.isRunning = isRunning;
            _coroutine = null;
        }
        /// <summary>
        /// Gets a value indicating whether the animation is currently running.
        /// </summary>
        public virtual bool IsRunning => isRunning;
        /// <summary>
        /// Gets the handler for the Animator.
        /// </summary>
        public virtual AnimatorHandler AnimatorHandler => animatorHandler;
    }
}

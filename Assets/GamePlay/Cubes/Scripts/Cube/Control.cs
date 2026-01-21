using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Handlers;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cube
{
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private bool isRunning;
        [SerializeField, ReadOnly] private Input.KindOf input;

        [Header(Header.components, order = 0)]
        [SerializeField, ReadOnly] private Animator animator;
        [SerializeField, ReadOnly] private Transform transform_;
        [SerializeField, NonReorderable, ReadOnly] private Input[] inputs;

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private Pointer pointer;
        [SerializeField, ReadOnly] private AnimatorHandler animatorHandler;
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset))]
        private void Reset()
        {
            input = Input.KindOf.hide;
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(X))]
        private void X()
        {
            SetInput(Input.KindOf.x);
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(O))]
        private void O()
        {
            SetInput(Input.KindOf.o);
        }
#endif
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void OnEnable()
        {
            pointer.ClickHandler += () =>
            {
                SetInput(Input.KindOf.x);
            };
        }
        ///<inheritdoc/>
        private void Start()
        {
            input = Input.KindOf.hide;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            return;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref animator);
            this.GetComponentIfNull(ref transform_, 0);
            this.GetComponentsInAllChildrenIfNull(ref inputs);
            this.GetComponentIfNull(ref pointer, 0);
            var conditional = animatorHandler.IsNecessaryUpdateInstance();
            if (conditional)
                animatorHandler = new(animator);
        }

        public Coroutine SetAnimation(bool isRunning, float delay = 0f)
        {
            isRunning.ComparativeAssignment(ref this.isRunning);
            var routine = _routine();
            return StartCoroutine(routine);

            IEnumerator _routine()
            {
                if (delay > 0f)
                    yield return new WaitForSeconds(delay);

                animator.SetBool(animatorHandler.ParameterHandlers[0].ID, isRunning);
                yield return new WaitForSeconds(animatorHandler.AnimationClips[1].length);
            }
        }

        public Coroutine SetInput(Input.KindOf input, float delay = 0)
        {
            var routine = _routine();
            input.ComparativeAssignment(ref this.input);
            return StartCoroutine(routine);

            IEnumerator _routine()
            {
                inputs[(int)input].SetVisibility(true);
                yield return SetAnimation(true, delay);

                if (input == Input.KindOf.hide)
                    yield break;

                yield return inputs[(int)input].SetTurnFlicker(true, 0.25f);
            }
        }
    }
}

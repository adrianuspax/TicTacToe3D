using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Handlers;
using ASPax.Utilities;
using System;
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

        [Header(Header.components, order = 0)]
        [SerializeField, ReadOnly] private Animator animator;
        [SerializeField, ReadOnly] private Transform transform_;
        [SerializeField, NonReorderable, ReadOnly] private Input[] inputs;

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private Data data;
        [SerializeField, ReadOnly] private Pointer pointer;
        [SerializeField, ReadOnly] private AnimatorHandler animatorHandler;

        public static event EventHandler<Args> Handler;
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset))]
        private void Reset()
        {
            Start();
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
            /*pointer.ClickHandler += () =>
            {
                SetInput(Input.KindOf.x);
            };*/
            return;
        }
        ///<inheritdoc/>
        private void Start()
        {
            var index = transform.GetSiblingIndex();
            data = new(index);
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
            if (data.IsInputted)
                return null;

            var routine = _routine();
            LastInput = data.Input = input;
            var e = new Args(data);
            Handler.Invoke(this, e);
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

        public Data Data
        {
            get => data;
            set => data = value;
        }

        public static Input.KindOf LastInput { get; private set; }
    }
}

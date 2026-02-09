using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TicTacToe3D.GamePlay.Cube
{
    public class Pointer : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private bool isInteractable;

        [Header(Header.components, order = 0)]
        [SerializeField, ReadOnly] private MeshRenderer meshRenderer;
        [SerializeField, ReadOnly] private BoxCollider boxCollider;

        private static bool _isInteractable;
        public event UnityAction ClickHandler;
        public event UnityAction DownHandler;
        public event UnityAction UpHandler;
        public static event UnityAction<bool> InteractableHandler;
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void OnEnable()
        {
            InteractableHandler += OnInteractable;
        }
        ///<inheritdoc/>
        private void Start()
        {
            _isInteractable = true;
            isInteractable = true;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            InteractableHandler -= OnInteractable;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref meshRenderer);
            this.GetComponentIfNull(ref boxCollider);
        }
        ///<inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isInteractable && _isInteractable)
                ClickHandler?.Invoke();
        }
        ///<inheritdoc/>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isInteractable && _isInteractable)
                UpHandler?.Invoke();
        }
        ///<inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInteractable && _isInteractable)
                DownHandler?.Invoke();
        }

        private void OnInteractable(bool value)
        {
            _isInteractable = value;

            if (isInteractable)
                isInteractable = value;
        }

        public void SetInteractable(bool value)
        {
            isInteractable = value;
        }

        public static void SetAllInteractable(bool value)
        {
            InteractableHandler?.Invoke(value);
        }
        /// <summary>
        /// Get if there is interactivity for this
        /// </summary>
        /// <remarks>It's read-only</remarks>
        public bool IsInteractable => isInteractable && _isInteractable;
        public MeshRenderer MeshRenderer => meshRenderer;
        public BoxCollider BoxCollider => boxCollider;
    }
}

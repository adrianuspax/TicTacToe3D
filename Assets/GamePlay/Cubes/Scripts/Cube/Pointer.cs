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
        /// <summary>
        /// Evento para ser chamado no clique.
        /// </summary>
        public event UnityAction ClickHandler;
        /// <summary>
        /// Evento para ser chamado quando o ponteiro é pressionado.
        /// </summary>
        public event UnityAction DownHandler;
        /// <summary>
        /// Evento para ser chamado quando o ponteiro é levantado.
        /// </summary>
        public event UnityAction UpHandler;
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void Start()
        {
            isInteractable = true;
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
            if (isInteractable)
                ClickHandler?.Invoke();
        }
        ///<inheritdoc/>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isInteractable)
                UpHandler?.Invoke();
        }
        ///<inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInteractable)
                DownHandler?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetInteractable(bool value)
        {
            isInteractable = value;
        }
        /// <summary>
        /// Get if there is interactivity for this.<br/>
        /// Read-only.
        /// </summary>
        public bool IsInteractable => isInteractable;
        public MeshRenderer MeshRenderer => meshRenderer;
        public BoxCollider BoxCollider => boxCollider;
    }
}

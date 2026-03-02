using ASPax.Attributes.Drawer;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.UI.Interaction.Inheritance
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Pressable : MonoBehaviour, IPressable
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] protected Selectable selectable;
        [SerializeField, ReadOnly] protected Image icon;
        [SerializeField, ReadOnly] protected TextMeshProUGUI tmp;
        [SerializeField, ReadOnly] protected CanvasGroup canvasGroup;
        [SerializeField, ReadOnly] private RectTransform rectTransform;
        [Space(-10, order = 0)]
        [Header(Header.variables, order = 1)]
        [SerializeField, ReadOnly] protected bool isVisible;
        ///<inheritdoc/>
        protected virtual void Awake()
        {
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        protected virtual void Start()
        {
            isVisible = true;
        }
        /// <inheritdoc/>
        [ContextMenu("Components Assignment Root")]
        public virtual void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref selectable);
            this.GetComponentInChildrenIfNull(ref icon);
            this.GetComponentInChildrenIfNull(ref tmp);
            this.GetComponentIfNull(ref canvasGroup);
            this.GetComponentIfNull(ref rectTransform);
        }
        /// <summary>
        /// Remove all listeners from the button or toggle
        /// </summary>
        public abstract void RemoveAllListeners();
        /// <summary>
        /// Set the visibility of the button
        /// </summary>
        /// <param name="value">true to visible</param>
        public virtual void SetVisible(bool value)
        {
            selectable.interactable = value;
            canvasGroup.blocksRaycasts = value;
            canvasGroup.alpha = value.ToInt();
            value.ComparativeAssignment(ref isVisible);
        }
        /// <summary>
        /// Set the Anchors max and min
        /// </summary>
        /// <param name="max">anchor max x and y</param>
        /// <param name="min">anchor min x and y</param>
        public virtual void SetAnchors((float x, float y) max, (float x, float y) min)
        {
            rectTransform.anchorMax = new(max.x, max.y);
            rectTransform.anchorMin = new(min.x, min.y);
        }
        /// <summary>
        /// Fade in the button
        /// </summary>
        /// <param name="time">Time to fade in</param>
        public virtual void FadeIn(float time)
        {
            canvasGroup.FadeIn(time, 1f, this);
        }
        /// <summary>
        /// Fade out the button
        /// </summary>
        /// <param name="time">Time to fade out</param>
        public virtual void FadeOut(float time)
        {
            canvasGroup.FadeOut(time, this);
        }
        /// <summary>
        /// Is this object interactable
        /// </summary>
        public virtual bool Interactable
        {
            set => selectable.interactable = value;
            get => selectable.interactable;
        }
        /// <summary>
        /// Return the <see cref="UnityEngine.CanvasGroup"/> component
        /// </summary>
        public CanvasGroup CanvasGroup => canvasGroup;
        /// <summary>
        /// The button is visible?
        /// </summary>
        public bool IsVisible => isVisible;
        /// <summary>
        /// Return the <see cref="TextMeshProUGUI"/> component.
        /// </summary>
        public TextMeshProUGUI TMPro => tmp;
        /// <summary>
        /// return the <see cref="UnityEngine.UI.Image"/> components: <see cref="icon"/> and <see cref="ring"/>
        /// </summary>
        public Image Icon => icon;
        /// <summary>
        /// Pivot Custom Component
        /// </summary>
        public RectTransform RectTransform => rectTransform;
    }
}

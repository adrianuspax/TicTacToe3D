using ASPax.Attributes.Drawer;
using ASPax.Attributes.Meta;
using ASPax.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe3D.UI.Interaction.Toggle
{
    using Toggle = UnityEngine.UI.Toggle;

    public class Default : Inheritance.Pressable
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.assets, order = 2)]
        [SerializeField] private Sprite toggleOnSprite;
        [SerializeField] private Sprite toggleOffSprite;
        [Header(Header.variables, order = 0)]
        [SerializeField] private string toggleOnText;
        [SerializeField] private string toggleOffText;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Toggle toggle;
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            toggle.onValueChanged.AddListener(ToggleBehaviour);
        }
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Root")]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            toggle = Get<Toggle>();
        }

        public virtual void AddListener(UnityEngine.Events.UnityAction<bool> call)
        {
            toggle.onValueChanged.AddListener(call);
        }

        public virtual void RemoveListener(UnityAction<bool> call)
        {
            toggle.onValueChanged.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }

        public virtual void ToggleBehaviour(bool isOn)
        {
            if (toggleOnSprite == null || toggleOffSprite == null)
                Debug.LogWarning("Toggle sprites are not assigned!", this);
            else
                icon.sprite = isOn ? toggleOnSprite : toggleOffSprite;

            var isTextNullOrEmpty = string.IsNullOrEmpty(toggleOnText) || string.IsNullOrEmpty(toggleOffText);

            if (isTextNullOrEmpty)
                Debug.LogWarning("Toggle texts are not assigned!", this);
            else
                tmp.text = isOn ? toggleOnText : toggleOffText;
        }

        public void SetSpriteOn(Sprite value)
        {
            toggleOnSprite = value;
        }

        public void SetSpriteOff(Sprite value)
        {
            toggleOffSprite = value;
        }

        public void SetTextOn(string value)
        {
            toggleOnText = value;
        }

        public void SetTextOff(string value)
        {
            toggleOffText = value;
        }

        public (Sprite On, Sprite Off) Sprite => (toggleOnSprite, toggleOffSprite);
        public (string On, string Off) Text => (toggleOnText, toggleOffText);
        public Toggle Toggle => toggle;
    }
}

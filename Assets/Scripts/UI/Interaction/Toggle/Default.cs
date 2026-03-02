using ASPax.Attributes.Drawer;
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
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            ((Toggle)selectable).onValueChanged.AddListener(ToggleBehaviour);
        }

        public virtual void AddListener(UnityEngine.Events.UnityAction<bool> call)
        {
            ((Toggle)selectable).onValueChanged.AddListener(call);
        }

        public virtual void RemoveListener(UnityAction<bool> call)
        {
            ((Toggle)selectable).onValueChanged.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            ((Toggle)selectable).onValueChanged.RemoveAllListeners();
        }

        public virtual void ToggleBehaviour(bool isOn)
        {
            if (toggleOnSprite == null || toggleOffSprite == null)
                Debug.LogWarning("Toggle sprites are not assigned!");
            else
                icon.sprite = isOn ? toggleOnSprite : toggleOffSprite;
        }
    }
}

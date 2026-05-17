using ASPax.Attributes.Drawer;
using ASPax.Attributes.Meta;
using ASPax.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe3D.UI.Interaction.Button
{
    using Button = UnityEngine.UI.Button;

    public class Default : Inheritance.Pressable
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Button button;
        /*///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            ((Button)selectable).onClick.AddListener(ButtonBehaviour);
        }*/
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Root")]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            button = Get<Button>();
        }
        public virtual void AddListener(UnityEngine.Events.UnityAction call)
        {
            button.onClick.AddListener(call);
        }

        public virtual void RemoveListener(UnityAction call)
        {
            button.onClick.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            button.onClick.RemoveAllListeners();
        }

        /*public virtual void ButtonBehaviour()
        {
            
        }*/

        public Button Button => button;
    }
}

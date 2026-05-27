using ASPax.Attributes.Drawer;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Default : Animation.Inheritance.Default
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.scripts, order = 2)]
        [SerializeField, ReadOnly] protected TogglePlayer togglePlayer;
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Inheritance")]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            this.GetComponentInChildrenIfNull(ref togglePlayer);
        }

        public TogglePlayer TogglePlayer => togglePlayer;
    }
}
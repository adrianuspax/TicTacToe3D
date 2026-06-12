using ASPax.Attributes.Drawer;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Default : Inheritance.AnimationHandler
    {
        [InfoBox("Inheritance from " + nameof(TicTacToe3D) + "." + nameof(TicTacToe3D.GamePlay) +"."+ nameof(TicTacToe3D.GamePlay.UI) +"."+ nameof(TicTacToe3D.GamePlay.UI.PanelSelection))]
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.scripts, order = 2)]
        [BoxGroup, SerializeField, ReadOnly] protected ToggleDefault toggleDefault;
        ///<inheritdoc/>
        [ContextMenu("Components Assignment Inheritance")]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            this.GetComponentInChildrenIfNull(ref toggleDefault);
        }

        public ToggleDefault TogglePlayer => toggleDefault;
    }
}
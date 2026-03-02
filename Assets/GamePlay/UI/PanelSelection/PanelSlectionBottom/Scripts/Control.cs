using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection.Bottom
{
    public class Control : Animation.Inheritance.Default
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.scripts, order = 2)]
        [SerializeField, ReadOnly] private TogglePlayer togglePlayer;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            this.GetComponentInChildrenIfNull(ref togglePlayer);
        }
    }
}
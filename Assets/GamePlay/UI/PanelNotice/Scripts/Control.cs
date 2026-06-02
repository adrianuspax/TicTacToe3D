using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.PanelNotice
{
    [RequireComponent(typeof(Manager))]
    public class Control : Animation.Inheritance.Default
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Button.Nop nopButton;
        [SerializeField, ReadOnly] private Button.Yes yesButton;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            this.GetComponentInChildrenIfNull(ref nopButton);
            this.GetComponentInChildrenIfNull(ref yesButton);
        }
    }
}
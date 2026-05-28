using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    [RequireComponent(typeof(Manager))]
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Upper upper;
        [SerializeField, ReadOnly] private Bottom bottom;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentInChildrenIfNull(ref upper);
            this.GetComponentInChildrenIfNull(ref bottom);
        }

        public Upper Upper => upper;
        public Bottom Bottom => bottom;
    }
}

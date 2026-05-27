using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Upper : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        ///<inheritdoc/>
        private void OnEnable()
        {
            toggleDefault.OnToggleValueChanged += ToggleFunction;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            toggleDefault.OnToggleValueChanged -= ToggleFunction;
        }

        [SerializeField, ReadOnly] private Bottom bottomControl;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            transform.parent.GetComponentInChildrenIfNull(ref bottomControl);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ToggleFunction(bool isOn)
        {
            bottomControl.SetAnimation(isOn);
        }
    }
}

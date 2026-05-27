using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection.Upper
{
    public class Control : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        ///<inheritdoc/>
        private void OnEnable()
        {
            togglePlayer.OnToggleValueChanged += ToggleFunction;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            togglePlayer.OnToggleValueChanged -= ToggleFunction;
        }

        [SerializeField, ReadOnly] private Bottom.Control bottonControl;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            transform.parent.GetComponentInChildrenIfNull(ref bottonControl);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ToggleFunction(bool isOn)
        {
            bottonControl.SetAnimation(isOn);
        }
    }
}

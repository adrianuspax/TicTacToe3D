using ASPax.Attributes.Drawer.SpecialCases;
using UnityEngine.Events;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class ToggleDefault : TicTacToe3D.UI.Interaction.Toggle.Default
    {
        public event UnityAction<bool> OnToggleValueChanged;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
        ///<inheritdoc/>
        public override void ToggleBehaviour(bool isOn)
        {
            base.ToggleBehaviour(isOn);
            OnToggleValueChanged?.Invoke(isOn);
        }
    }
}

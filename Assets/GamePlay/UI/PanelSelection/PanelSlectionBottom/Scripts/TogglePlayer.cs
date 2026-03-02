using ASPax.Attributes.Drawer.SpecialCases;
using UnityEngine.Events;

namespace TicTacToe3D.GamePlay.UI.PanelSelection.Bottom
{
    public class TogglePlayer : TicTacToe3D.UI.Interaction.Toggle.Default
    {
        public static event UnityAction<bool> OnToggleValueChanged;
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }

        public override void ToggleBehaviour(bool isOn)
        {
            base.ToggleBehaviour(isOn);
            tmp.text = isOn ? "| IA |" : "| YOU |";
            OnToggleValueChanged?.Invoke(isOn);
        }
    }
}

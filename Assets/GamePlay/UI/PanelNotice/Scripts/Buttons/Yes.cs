using ASPax.Attributes.Drawer.SpecialCases;

namespace TicTacToe3D.GamePlay.UI.PanelNotice.Button
{
    public class Yes : TicTacToe3D.UI.Interaction.Button.Default
    {
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
    }
}

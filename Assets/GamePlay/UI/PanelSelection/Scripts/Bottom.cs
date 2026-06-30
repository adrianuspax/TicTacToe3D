using ASPax.Attributes.Drawer.SpecialCases;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Bottom : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            toggleDefault.Toggle.isOn = false;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
    }
}

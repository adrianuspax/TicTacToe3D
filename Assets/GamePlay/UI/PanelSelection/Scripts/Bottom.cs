using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Utilities;
using UnityEngine;
using UnityEngine.Localization;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Bottom : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header("Localization", order = 2)]
        [SerializeField, ReadOnly] private LocalizedString ui_you = new("UI",nameof(ui_you));
        [SerializeField, ReadOnly] private LocalizedString ui_AI = new("UI",nameof(ui_AI));
        ///<inheritdoc/>
        private void OnEnable()
        {
            ui_you.StringChanged += toggleDefault.SetTextOn;
            ui_AI.StringChanged += toggleDefault.SetTextOff;
        }
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            toggleDefault.Toggle.isOn = false;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            ui_you.StringChanged -= toggleDefault.SetTextOn;
            ui_AI.StringChanged -= toggleDefault.SetTextOff;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
    }
}

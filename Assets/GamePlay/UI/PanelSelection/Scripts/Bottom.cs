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
        [Header(Header.scripts, order = 2)]
        [SerializeField, ReadOnly] private Bottom bottom;
        [Space(-10, order = 0)]
        [Header("Localization", order = 1)]
        [SerializeField, ReadOnly] private LocalizedString you = new("UI",nameof(you));
        [SerializeField, ReadOnly] private LocalizedString AI = new("UI",nameof(AI));
        ///<inheritdoc/>
        private void OnEnable()
        {
            you.StringChanged += toggleDefault.SetTextOn;
            AI.StringChanged += toggleDefault.SetTextOff;
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
            you.StringChanged -= toggleDefault.SetTextOn;
            AI.StringChanged -= toggleDefault.SetTextOff;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
    }
}

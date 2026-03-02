using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection.Bottom
{
    public class Control : Animations.Inheritance
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.assets, order = 2)]
        [SerializeField] private Sprite toggleOnSprite;
        [SerializeField] private Sprite toggleOffSprite;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Toggle toggle;
        [SerializeField, ReadOnly] private TextMeshProUGUI tmp;
        public static event UnityAction<bool> OnToggleValueChanged;
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            toggle.onValueChanged.AddListener(ToggleBehaviour);
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            this.GetComponentInChildrenIfNull(ref toggle);
            this.GetComponentInChildrenIfNull(ref tmp);
        }

        public void ToggleBehaviour(bool isOn)
        {
            tmp.text = isOn ? "AI" : "YOU";
            toggle.image.sprite = isOn ? toggleOnSprite : toggleOffSprite;
            OnToggleValueChanged?.Invoke(isOn);
        }
    }
}
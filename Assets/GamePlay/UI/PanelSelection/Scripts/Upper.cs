using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Upper : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.sprites, order = 2)]
        [SerializeField] private Sprite humanSprite;
        [SerializeField] private Sprite robotSprite;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.scripts, order = 2)]
        [SerializeField, ReadOnly] private Bottom bottom;
        [Space(-10, order = 0)]
        [Header("Localization", order = 1)]
        [SerializeField, ReadOnly] private LocalizedString selectWhoGoesFirst = new("UI","selectWhoGoesFirst");
        [SerializeField, ReadOnly] private LocalizedString humanString = new("UI", "yourTime");
        [SerializeField, ReadOnly] private LocalizedString robotString = new("UI", "myTurn");
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset))]
        protected override void Reset()
        {
            base.Reset();
            selectWhoGoesFirst = new("UI", "selectWhoGoesFirst");
            humanString = new("UI", "yourTime");
            robotString = new("UI", "myTurn");
        }
#endif
        ///<inheritdoc/>
        private void OnEnable()
        {
            toggleDefault.OnToggleValueChanged += ToggleFunction;
            selectWhoGoesFirst.StringChanged += toggleDefault.SetTextOn;
        }
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            var routine = _routine();
            StartCoroutine(routine);
            return;
            // Routine to wait for the Bottom script to be initialized before assigning it
            IEnumerator _routine()
            {
                bottom.TogglePlayer.Toggle.interactable = false;
                toggleDefault.Toggle.interactable = false;
                yield return SetAnimation(true);
                toggleDefault.Toggle.interactable = true;
                bottom.TogglePlayer.Toggle.interactable = true;
                toggleDefault.Toggle.isOn = true;
            }
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            toggleDefault.OnToggleValueChanged -= ToggleFunction;
            selectWhoGoesFirst.StringChanged -= toggleDefault.SetTextOn;
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            transform.parent.GetComponentInChildrenIfNull(ref bottom);
        }
        /// <summary>
        /// Toggle function
        /// </summary>
        private void ToggleFunction(bool isOn)
        {
            bottom.SetAnimation(isOn);
        }
    }
}

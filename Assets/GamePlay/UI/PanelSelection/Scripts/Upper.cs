using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;
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
        [Space(-10, order = 0)]
        [Header(Header.strings, order = 1)]
        [SerializeField] private string humanString;
        [SerializeField] private string robotString;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.scripts, order = 2)]
        [SerializeField, ReadOnly] private Bottom bottom;
        ///<inheritdoc/>
        private void OnEnable()
        {
            toggleDefault.OnToggleValueChanged += ToggleFunction;
        }
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            var routine = _routine();
            StartCoroutine(routine);
            return;
            // 
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
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
            transform.parent.GetComponentInChildrenIfNull(ref bottom);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ToggleFunction(bool isOn)
        {
            bottom.SetAnimation(isOn);
        }
    }
}

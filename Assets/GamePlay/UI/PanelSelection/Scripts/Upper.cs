using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    public class Upper : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        [SerializeField, ReadOnly] private Bottom bottomControl;
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
            IEnumerator _routine()
            {
                yield return SetAnimation(true);
                toggleDefault.Toggle.isOn = true;
            }

            /*var routine = _routine();
            StartCoroutine(routine);
            return;
            IEnumerator _routine()
            {
                yield return SetAnimation(true);
                bottomControl.SetAnimation(true);
            }*/
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
            transform.parent.GetComponentInChildrenIfNull(ref bottomControl);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ToggleFunction(bool isOn)
        {
            bottomControl.SetAnimation(isOn);
        }
    }
}

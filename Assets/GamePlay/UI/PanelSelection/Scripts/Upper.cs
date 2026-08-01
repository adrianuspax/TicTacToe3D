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
    /// <summary>
    /// Classe responsável pelo painel superior
    /// </summary>
    public partial class Upper : TicTacToe3D.GamePlay.UI.PanelSelection.Default
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.sprites, order = 2)]
        [SerializeField] private Sprite humanSprite;
        [SerializeField] private Sprite robotSprite;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private CurrentPlayer currentPlayer;

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private Bottom bottom;
        [Space(-10, order = 0)]
        [Header("Localization", order = 1)]
        [SerializeField, ReadOnly] private LocalizedString ui_selectWhoGoesFirst = new("UI",nameof(ui_selectWhoGoesFirst));
        [SerializeField, ReadOnly] private LocalizedString ui_currentPlayer = new("UI",nameof(ui_currentPlayer));
        [SerializeField, ReadOnly] private LocalizedString ui_yourTime = new("UI", nameof(ui_yourTime));
        [SerializeField, ReadOnly] private LocalizedString ui_myTurn = new("UI", nameof(ui_myTurn));
        ///<inheritdoc/>
        private void OnEnable()
        {
            toggleDefault.OnToggleValueChanged += ToggleFunction;
            ui_selectWhoGoesFirst.StringChanged += toggleDefault.SetTextOn;
            ui_currentPlayer.StringChanged += toggleDefault.SetTextOff;
        }
        ///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            currentPlayer = CurrentPlayer.None;
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
                toggleDefault.Toggle.onValueChanged.AddListener((_) => toggleDefault.Toggle.interactable = false);
            }
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            toggleDefault.OnToggleValueChanged -= ToggleFunction;
            ui_selectWhoGoesFirst.StringChanged -= toggleDefault.SetTextOn;
            ui_currentPlayer.StringChanged -= toggleDefault.SetTextOff;
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
        /// <summary>
        /// Atribua o Player atual no painel superior
        /// </summary>
        /// <param name="currentPlayer">Player atual</param>
        public void SetCurrentPlayer(CurrentPlayer currentPlayer)
        {
            switch (currentPlayer)
            {
                case CurrentPlayer.Human:
                    _human();
                    break;
                case CurrentPlayer.Robot:
                    _robot();
                    break;
                default:
                    _default();
                    break;
            }

            this.currentPlayer = currentPlayer;

            void _human()
            {
                toggleDefault.TMPro.text = ui_yourTime.GetLocalizedString();
                toggleDefault.Icon.sprite = humanSprite;
            }

            void _robot()
            {
                toggleDefault.TMPro.text = ui_myTurn.GetLocalizedString();
                toggleDefault.Icon.sprite = robotSprite;
            }

            void _default()
            {
                return;
            }
        }
        /// <summary>
        /// Retorna qual é o player atual
        /// </summary>
        public CurrentPlayer CurrentPlayer => currentPlayer;
    }
}

using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Handlers;
using ASPax.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cube
{
    /// <summary>
    /// Classe controle do cubo em gameplay.
    /// </summary>
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [Tooltip("Indica se o cubo está em execução.")]
        [SerializeField, ReadOnly] private bool _isRunning;

        [Header(Header.components, order = 0)]
        [Tooltip("Referência ao componente Animator do cubo.")]
        [SerializeField, ReadOnly] private Animator _animator;
        [Tooltip("Referência ao componente Transform do cubo.")]
        [SerializeField, ReadOnly] private Transform _transform;
        [Tooltip("Array de componentes Input associados ao cubo.")]
        [SerializeField, NonReorderable, ReadOnly] private Input[] _inputs;

        [Header(Header.scripts, order = 0)]
        [Tooltip("Dados associados à interação com o cubo.")]
        [SerializeField, ReadOnly] private Data _data;
        [Tooltip("Referência ao script Pointer para detectar interações do usuário.")]
        [SerializeField, ReadOnly] private Pointer _pointer;
        [Tooltip("Referência ao script AnimatorHandler para controlar as animações do cubo.")]
        [SerializeField, ReadOnly] private AnimatorHandler _animatorHandler;
        private static bool _isInputting;
        private static Input.KindOf _lastInput;
        /// <summary>
        /// Evento para notificar quando o cubo é interagido, passando os dados associados à interação.
        /// </summary>
        public static event EventHandler<Args> InputHandler;
        /// <summary>
        /// Evento para a manipulação da fluxo do processo de input.<br/> 
        /// Use true para início do fluxo e false para final do fluxo.
        /// </summary>
        public static event Action<bool> InputtingHandler;
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset), SButtonEnableMode.Editor)]
        private void Reset()
        {
            Start();
        }
        /// <summary>
        /// Método de teste para atribuir a escolha X.
        /// </summary>
        [Button(nameof(X), SButtonEnableMode.Playmode)]
        private void X()
        {
            SetInput(Input.KindOf.x);
        }
        /// <summary>
        /// Método de teste para atribuir a escolha O.
        /// </summary>
        [Button(nameof(O), SButtonEnableMode.Playmode)]
        private void O()
        {
            SetInput(Input.KindOf.o);
        }
#endif
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void Start()
        {
            var index = transform.GetSiblingIndex();
            _isInputting = false;
            _lastInput = Input.KindOf.hide;
            _data = new(index);
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref _animator);
            this.GetComponentIfNull(ref _transform, 0);
            this.GetComponentsInAllChildrenIfNull(ref _inputs);
            this.GetComponentIfNull(ref _pointer, 0);
            var conditional = _animatorHandler.IsNecessaryUpdateInstance();
            if (conditional)
                _animatorHandler = new(_animator);
        }
        /// <summary>
        /// Dispara a animação do cubo: "X" ou "O", dependendo do tipo de entrada fornecido.
        /// </summary>
        /// <param name="isRunning">Atribua o valor para diparar a aniamação caso ela não esteja disparada.</param>
        /// <param name="delay">Delay para a disparada da animação em segundos.<br/>
        ///                     O valor padrão é 0 e não pode ser menor que 0, caso contrário, converterá-se em 0.</param>
        /// <returns>Retorna a coroutina que está em execução no método em questão.</returns>
        public Coroutine SetAnimation(bool isRunning, float delay = 0f)
        {
            isRunning.ComparativeAssignment(ref this._isRunning);
            var routine = _routine();
            return StartCoroutine(routine);
            // Local function para a execução da coroutina.
            IEnumerator _routine()
            {
                if (delay > 0f)
                    yield return new WaitForSeconds(delay);

                _animator.SetBool(_animatorHandler.ParameterHandlers[0].ID, isRunning);
                yield return new WaitForSeconds(_animatorHandler.AnimationClips[1].length);
            }
        }
        /// <summary>
        /// Dispara a animação do cubo: gira o cubo para mostrar o lado oposto.
        /// </summary>
        /// <param name="input"> Atribua  </param>
        /// <param name="delay"></param>
        /// <returns>Retorna a coroutina que está em execução no método em questão.</returns>
        public Coroutine SetInput(Input.KindOf input, float delay = 0)
        {
            if (_data.IsInputted)
                return default;

            _isInputting = true;
            InputtingHandler?.Invoke(true);
            var routine = _routine();
            var coroutine = StartCoroutine(routine);
            _data.SetInput(input);
            _lastInput = _data.Input;
            var e = new Args(_data);
            InputHandler?.Invoke(this, e);
            return coroutine;
            // Local function para a execução da coroutina.
            IEnumerator _routine()
            {
                _inputs[(int)input].SetVisibility(true);
                yield return SetAnimation(true, delay);

                if (input == Input.KindOf.hide)
                    yield break;

                yield return _inputs[(int)input].SetTurnFlicker(true, 0.25f);
                InputtingHandler?.Invoke(false);
                _isInputting = false;
            }
        }
        /// <summary>
        /// Atribui o valor para <see cref="_data"/><br/>
        /// Apenas atribua se estiver certo disso!
        /// </summary>
        public void SetData(Data data)
        {
            _data = data;
        }
        /// <summary>
        /// Retorna e/ou Atribui <see cref="_data"/>
        /// </summary>
        /// <value>Dados associados a interação com o cubo.</value>
        public Data Data => _data;
        /// <summary>
        /// Retorna os possíveis inputs do cubo (X e O).
        /// </summary>
        public Input[] Inputs => _inputs;
        /// <summary>
        /// Retorna a instância de <see cref="Pointer"/>
        /// </summary>
        public Pointer Pointer => _pointer;
        /// <summary>
        /// Retorna o que foi inputado no cubo (X ou O).
        /// </summary>
        public Input Inputted => _inputs[(int)_data.Input];
        /// <summary>
        /// Atributo estático que indica se o cubo está em processo de input.<br/>
        /// Incuindo a animação de input e o delay de input.
        /// </summary>
        public static bool IsInputting => _isInputting;
        /// <summary>
        /// Atributo estático para armazenar a última entrada registrada em qualquer cubo.
        /// </summary>
        public static Input.KindOf LastInput => _lastInput;
    }
}

using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe3D.GamePlay.Main
{
    /// <summary>
    /// Tic Tac Toe GamePlay Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private Cube.Input.KindOf player;
        [Tooltip("The result of the game.")]
        [SerializeField, ReadOnly] private AI.Result result;

        [Header(Header.scripts, order = 0)]
        [Tooltip("An array of Cube data representing the state of the board.")]
        [SerializeField, NonReorderable, ReadOnly] private Cube.Data[] board;
        [Space(20)]
        [Tooltip("The AI instance for the game.")]
        [SerializeField, ReadOnly] private AI ai;
        private Coroutine _movement;
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset))]
        private void Reset()
        {
            player = Cube.Input.KindOf.hide;
            ai = new();
        }
#endif
        /// <inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        private void OnEnable()
        {
            Cube.Control.InputHandler += OnPlayable;
            Cube.Control.InteractivityHandler += OnInteractivity;
            AI.NotifyHandler += OnNotify;
        }
        /// <inheritdoc/>
        private void Start()
        {
            IEnumerator routine;
            _movement = null;
            result = new();
            UI.PanelSelection.Manager.Instance.Bottom.TogglePlayer.Toggle.onValueChanged.AddListener(SetPlayer);
            UI.PanelSelection.Manager.Instance.Upper.TogglePlayer.Toggle.onValueChanged.AddListener(_call);
            return;
            void _call(bool value)
            {
                routine = _routine();

                if (value)
                    return;
                else
                    StartCoroutine(routine);
            }
            // 
            IEnumerator _routine()
            {
                InstantiateSafetyAI(value => ai = value);
                routine = FirstMovement();
                yield return StartCoroutine(routine);
                Main.Cubes.Manager.SetInputPlayerInAllCubes(player);
                UI.PanelSelection.Manager.Instance.Upper.TogglePlayer.Toggle.onValueChanged.RemoveListener(_call);
            }
        }
        /// <inheritdoc/>
        private void OnDisable()
        {
            Cube.Control.InputHandler -= OnPlayable;
            Cube.Control.InteractivityHandler -= OnInteractivity;
            AI.NotifyHandler -= OnNotify;
        }
        /// <inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            var isNullOrEmpty = board.IsNullOrEmpty();
            if (isNullOrEmpty)
                board = GetBoard();
        }
        /// <summary>
        /// Function used to be called when <see cref="Cube.Control.InputHandler"/> is invoked.
        /// </summary>
        /// <param name="sender">Sender Object<br/>Must receive <see cref="Cube.Control"/> as object</param>
        /// <param name="e">Arguments to Handler</param>
        /// <remarks>Default arguments when using <see cref="System.EventHandler"/></remarks>
        public void OnPlayable(object sender, Cube.Args e)
        {
            Main.Cubes.Manager.Instance.Array[e.Data.Index].SetData(e.Data);
            var isEnd = ResultBehaviour();
            var routine = _routine();

            if (isEnd)
                return;

            if (e.Data.Input == player)
                _movement = StartCoroutine(routine);

            return;
            // Coroutine para aguardar o movimento do player.
            IEnumerator _routine()
            {
                yield return new WaitWhile(() => Cube.Control.IsInputting);
                board = GetBoard();
                (var bestMove, var bestScore) = ai.GetBest(board);

                if (bestMove != -1)
                {
                    if (bestScore > 0)
                        yield return new WaitWhile(() => UI.PanelNotice.Manager.Instance.IsToOpen);

                    yield return SetInputAI(bestMove, 0.5f);
                }
            }
        }
        /// <summary>
        /// Manipula o comportamento do resultado do jogo.
        /// </summary>
        /// <returns>Retorna true se o jogo terminou.</returns>
        private bool ResultBehaviour()
        {
            board = GetBoard();
            result = ai.CheckForWinner(board);
            var routine = _beahviour();

            return result.main switch
            {
                Main.Result.draw => _draw(),
                Main.Result.youLose => _youLose(),
                Main.Result.youWin => _youWin(),
                _ => _none(),
            };
            // Método interno chamado em caso de empate.
            bool _draw()
            {
                Main.Cubes.Manager.Instance.SetInteractable(false);

                foreach (var cube in Main.Cubes.Manager.Instance.Array)
                    cube.Inputted.SetTurnFlicker(false);

                return true;
            }
            // Método interno chamado em caso de derrota.
            bool _youLose()
            {
                Main.Cubes.Manager.Instance.SetInteractable(false);
                StartCoroutine(routine);
                return true;
            }
            // Método interno chamado em caso de vitória.
            bool _youWin()
            {
                Main.Cubes.Manager.Instance.SetInteractable(false);
                StartCoroutine(routine);
                return true;
            }
            // Método interno chamado em caso de o jogo continuar.
            bool _none()
            {
                return false;
            }
            // Comportamento dos cubos quando o jogo termina e há um ganhador.
            IEnumerator _beahviour()
            {
                Cube.Control cube;
                var exceptIndexes = result.GetExceptIndexes();
                yield return _movement;
                for (int i = 0; i < exceptIndexes.Length; i++)
                {
                    var x = exceptIndexes[i];
                    cube = Main.Cubes.Manager.Instance.Array[x];

                    if (cube.Data.IsInputted)
                        cube.Inputted.SetTurnFlicker(false);
                }
            }
        }
        /// <summary>
        /// Initiates the AI's turn after a specified delay.
        /// </summary>
        public Coroutine SetInputAI(int bestMove, float delay = 0f)
        {
            var nextInput = GetNextInput();
            return Main.Cubes.Manager.Instance.Array[bestMove].SetInput(nextInput, delay);
        }

        public void SetPlayer(bool isHuman)
        {
            player = isHuman ? Cube.Input.KindOf.x : Cube.Input.KindOf.o;
        }
        /// <summary>
        /// Routine para instaciar a IA com seguraça.
        /// </summary>
        /// <param name="call">Ação a ser chamada com a instância da IA.</param>
        private void InstantiateSafetyAI(UnityAction<AI> call)
        {
            var value = new AI(player);
            call?.Invoke(value);
        }
        /// <summary>
        /// Routine para o primeiro movimento da IA do jogo onde há uma aleatoriedade de escolha para que a IA não inicie na mesma posição sempre.
        /// </summary>
        /// <param name="delay">Delay para início do movimento em segundos.</param>
        /// <returns><see cref="IEnumerator"/> para coroutines.</returns>
        private IEnumerator FirstMovement(float delay = 0f)
        {
            yield return new WaitWhile(() => player == Cube.Input.KindOf.hide);

            if (player == Cube.Input.KindOf.o)
            {
                var values = new int[] { 0, 2, 6, 8 };
                var index = Random.Range(0, values.Length);
                var value = values[index];
                var nextInput = GetNextInput();
                yield return Main.Cubes.Manager.Instance.Array[value].SetInput(nextInput, delay);
            }
        }
        /// <summary>
        /// Função para ser atribuído ao evento <see cref="AI.NotifyHandler"/><br/>
        /// para que haja a manipulãção da tela de notificação em <see cref="AI"/>.
        /// </summary>
        private void OnNotify()
        {
            if (result.main == Main.Result.none)
                UI.PanelNotice.Manager.Instance.SetAnimation(true);

            AI.NotifyHandler -= OnNotify;
        }
        /// <summary>
        /// Retorna uma cópia do estado atual do tabuleiro.
        /// </summary>
        /// <returns><see cref="Cube.Data[]"/></returns>
        // Em tempo de execução, a instancia monosingleton não funcionará
#if UNITY_EDITOR
        private Cube.Data[] GetBoard()
        {
            var cubes = FindAnyObjectByType<Main.Cubes.Control>().Array;

            var isNullOrEmpty = cubes.IsNullOrEmpty();
            if (isNullOrEmpty)
            {
                Debug.LogError($"{nameof(Main.Cubes.Control.Array)} is empty or null!", this);
                return null;
            }

            return cubes.Select(cube => cube.Data).ToArray();
        }
        // Em tempo de execução, a instancia monosingleton funcionará
#else
        private Cube.Data[] GetBoard()
        {
            var isNullOrEmpty = Main.Cubes.Manager.Instance.Array.IsNullOrEmpty();
            if (isNullOrEmpty)
            {
                Debug.LogError($"{nameof(Main.Cubes.Manager.Instance.Array)} is empty or null!", this);
                return null;
            }

            return Main.Cubes.Manager.Instance.Array.Select(cube => cube.Data).ToArray();
        }
#endif
        /// <summary>
        /// Retorna o próximo input baseado no input atual.
        /// </summary>
        /// <returns>Tipo de input.<br/>
        /// </returns>
        public Cube.Input.KindOf GetNextInput()
        {
            return Cube.Control.LastInput switch
            {
                Cube.Input.KindOf.x => Cube.Input.KindOf.o,
                Cube.Input.KindOf.o => Cube.Input.KindOf.x,
                _ => Cube.Input.KindOf.x,
            };
        }
        /// <summary>
        /// Função para ser atribuído ao evento <see cref="Cube.Control.InteractivityHandler"/><br/>
        /// para que haja a manipulãção da interatividade de todos os cubos em <see cref="Cube.Control"/>.
        /// </summary>
        private void OnInteractivity(bool value)
        {
            if (result.main == Main.Result.none)
                Main.Cubes.Manager.Instance.SetInteractable(value);
            else
                Main.Cubes.Manager.Instance.SetInteractable(false);
        }
        /// <summary>
        /// Return all Cubes.<br/>
        /// Somente leitura.
        /// </summary>
        public Cube.Control[] Cubes => Main.Cubes.Manager.Instance.Array;
        /// <summary>
        /// Gets the input type of the human player.<br/>
        /// Somente leitura.
        /// </summary>
        public Cube.Input.KindOf Player => player;
        /// <summary>
        /// Gets the current result of the game.<br/>
        /// Somente leitura.
        /// </summary>
        public AI.Result Result => result;
        /// <summary>
        /// An array of Cube data representing the state of the board.<br/>
        /// Somente leitura.
        /// </summary>
        public Cube.Data[] Board => board;
    }
}

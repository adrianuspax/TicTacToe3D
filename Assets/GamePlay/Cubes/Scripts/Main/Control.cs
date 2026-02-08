using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
        [SerializeField, ReadOnly] private AI.Result result; // The result of the game.

        [Header(Header.scripts, order = 0)]
        [SerializeField, NonReorderable, ReadOnly] private Cube.Control[] cubes; // An array of Cube controls representing the cells of the board.
        [SerializeField, NonReorderable, ReadOnly] private Cube.Data[] board; // An array of Cube data representing the state of the board.
        [Space(20)]
        [SerializeField, ReadOnly] private AI ai; // The AI instance for the game.
        /// <inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        private void OnEnable()
        {
            Cube.Control.Handler += OnPlayable;
            AI.NotifyHandler += OnNotify;
        }
        /// <inheritdoc/>
        private void Start()
        {
            var routine = InstantiateSafetyAI(value => ai = value);
            result = new();
            //restartButton.AddListener(ResetGame);
            //playerToggle.AddListener(SetPlayer);
            StartCoroutine(routine);
            routine = FirstMovement();
            StartCoroutine(routine);
        }
        /// <inheritdoc/>
        private void OnDisable()
        {
            Cube.Control.Handler -= OnPlayable;
            AI.NotifyHandler -= OnNotify;
        }
        /// <inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.FindObjectsByTypeIfNull(ref cubes);
            board = GetBoard();
        }
        /// <summary>
        /// Function used to be called when <see cref="Cube.Control.Handler"/> is invoked.
        /// </summary>
        /// <param name="sender">Sender Object<br/>Must receive <see cref="Cube.Control"/> as object</param>
        /// <param name="e">Arguments to Handler</param>
        /// <remarks>Default arguments when using <see cref="System.EventHandler"/></remarks>
        public void OnPlayable(object sender, Cube.Args e)
        {
            cubes[e.Data.Index].Data = e.Data;
            var isEnd = ResultBehaviour();

            if (isEnd)
                return;

            if (e.Data.Input == player)
                SetInputAI();
        }

        private bool ResultBehaviour()
        {
            board = GetBoard();
            result = ai.CheckForWinner(board);

            return result.main switch
            {
                Main.Result.draw => _draw(),
                Main.Result.youLose => _youLose(),
                Main.Result.youWin => _youWin(),
                _ => _none(),
            };

            bool _draw()
            {
                SetCubesInteractable(false);
                return true;
            }

            bool _youLose()
            {
                SetCubesInteractable(false);
                _beahviour();
                return true;
            }

            bool _youWin()
            {
                SetCubesInteractable(false);
                _beahviour();
                return true;
            }

            bool _none()
            {
                return false;
            }

            void _beahviour()
            {
                for (var i = 0; i < result.indexes.Length; i++)
                {
                    //cubes[result.indexes[i]].SetColorText(Color.indianRed);
                }
            }
        }
        /// <summary>
        /// Initiates the AI's turn after a specified delay.
        /// </summary>
        public Coroutine SetInputAI(float delay = 0f)
        {
            board = GetBoard();
            var bestSlotIndex = ai.GetBestMove(board);
            if (bestSlotIndex == -1)
                return null;

            var nextInput = GetNextInput();
            return cubes[bestSlotIndex].SetInput(nextInput, delay);
        }

        public void SetCubesInteractable(bool value)
        {
            /*foreach (var cube in cubes)
                cube.SetInteractable(value);*/
        }

        public void ResetGame()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void SetPlayer(bool isHuman)
        {
            player = isHuman ? Cube.Input.KindOf.x : Cube.Input.KindOf.o;
        }

        private IEnumerator InstantiateSafetyAI(UnityAction<AI> call)
        {
            yield return null; //new WaitUntil(() => playerToggle.didStart);
            var value = new AI(player);
            call?.Invoke(value);
        }

        private IEnumerator FirstMovement(float delay = 0f)
        {
            yield return new WaitWhile(() => player == Cube.Input.KindOf.hide);

            if (player == Cube.Input.KindOf.o)
            {
                var values = new int[] { 0, 2, 6, 8 };
                var index = Random.Range(0, values.Length);
                var value = values[index];
                var nextInput = GetNextInput();
                yield return cubes[value].SetInput(nextInput, delay);
            }
        }

        private void OnNotify()
        {
            var routine = _routine();
            StartCoroutine(routine);
            AI.NotifyHandler -= OnNotify;

            IEnumerator _routine()
            {
                var waitForSeconds = new WaitForSeconds(0.75f);
                yield return waitForSeconds;
                /*if (result.main == Main.Result.none)
                    panel.SetActive(true);*/
            }
        }

        private Cube.Data[] GetBoard()
        {
            var isNullOrEmpty = cubes.IsNullOrEmpty();
            if (isNullOrEmpty)
            {
                Debug.LogError($"{nameof(cubes)} is empty or null!", this);
                return null;
            }

            return cubes.Select(cube => cube.Data).ToArray();
        }

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
        /// Return all Cubes
        /// </summary>
        /// <remarks>Read only</remarks>
        public Cube.Control[] Cubes => cubes;
        /// <summary>
        /// Gets the input type of the human player.
        /// </summary>
        public Cube.Input.KindOf Player => player;
        /// <summary>
        /// Gets the current result of the game.
        /// </summary>
        public AI.Result Result => result;

        public Cube.Data[] Board
        {
            get
            {
                board = GetBoard();
                return board;
            }
        }
    }
}

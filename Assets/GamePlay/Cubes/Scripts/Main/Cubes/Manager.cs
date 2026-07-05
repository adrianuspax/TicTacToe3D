using System.Collections.Generic;
using UnityEngine.Events;

namespace TicTacToe3D.GamePlay.Main.Cubes
{
    /// <inheritdoc/>
    public class Manager : Inheritance.Manager<Control>
    {
        private static Dictionary<Cube.Control, UnityAction> _trackedActions;
        ///<inheritdoc/>
        private void Start()
        {
            _trackedActions = new();
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            RemoveInputPlayerInAllCubes();
        }
        /// <summary>
        /// Adicione uma ação para ser chamada no clique de todos os cubos.
        /// </summary>
        /// <param name="player">O input do player.</param>
        public static void SetInputPlayerInAllCubes(Cube.Input.KindOf player)
        {
            foreach (var cube in Instance.Array)
            {
                var conditional = _trackedActions.ContainsKey(cube);

                if (conditional)
                    continue;

                UnityAction action = () => cube.SetInput(player);
                _trackedActions[cube] = action;
                cube.Pointer.ClickHandler += action;
            }
        }
        /// <summary>
        /// Remova uma ação para ser chamada no clique de todos os cubos.
        /// </summary>
        public static void RemoveInputPlayerInAllCubes()
        {
            if (Instance == null)
                return;

            foreach (var cube in Instance.Array)
            {
                var conditional = _trackedActions.TryGetValue(cube, out UnityAction action);

                if (conditional)
                {
                    cube.Pointer.ClickHandler -= action;
                    _trackedActions.Remove(cube);
                }
            }
        }
    }
}

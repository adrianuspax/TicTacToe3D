using System;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cube
{
    /// <summary>
    /// Struct para armazenar os dados associados à interação com o cubo.
    /// </summary>
    [Serializable]
    public struct Data
    {
        [Tooltip("Índice do cubo")]
        [SerializeField] private int _index;
        [Tooltip("O input atribuído ao cubo: X ou O")]
        [SerializeField] private Cube.Input.KindOf _input;
        /// <summary>
        /// Construtor da struct <see cref="Data"/>.
        /// </summary>
        /// <param name="index">Índice do cubo.<br/>Deve compreender entre 0 e 8.</param>
        /// <param name="input">Atual input do player</param>
        public Data(int index = -1, Cube.Input.KindOf input = Cube.Input.KindOf.hide)
        {
            if (index < 0 || index > 8)
                index = -1;

            _index = index;
            _input = input;
        }
        /// <summary>
        /// Índice do cubo.
        /// </summary>
        public int Index
        {
            readonly get => _index; set => _index = value;
        }
        /// <summary>
        /// Input atribuído ao cubo: X ou O.
        /// </summary>
        public Cube.Input.KindOf Input
        {
            readonly get => _input; set => _input = value;
        }
        /// <summary>
        /// Retorna verdadeiro se o cubo tiver um input atribuído, caso contrário, retorna falso.
        /// </summary>
        public readonly bool IsInputted => ((int)_input) > -1;
    }
}

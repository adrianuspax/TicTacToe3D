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
        /// <param name="index">Índice do cubo.<br/>
        /// Deve compreender entre 0 e 8. Caso contrário, será atribuído o valor -1!</param>
        /// <param name="input">Atual input do player</param>
        public Data(int index = -1, Cube.Input.KindOf input = Cube.Input.KindOf.hide)
        {
            if (index < 0 || index > 8)
                index = -1;

            _index = index;
            _input = input;
        }
        /// <summary>
        /// Atribua um índice para <see cref="_index"/><br/>Atribua somente se tiver certeza!
        /// </summary>
        /// <param name="index">Índice do cubo.<br/>Deve compreender entre 0 e 8. Caso contrário, será atribuído o valor -1!</param>
        public void SetIndex(int index)
        {
            if (index < 0 || index > 8)
                index = -1;

            _index = index;
        }
        /// <summary>
        /// Atribua um input para <see cref="_input"/><br/>
        /// Atribua somente se tiver certeza!
        /// </summary>
        public void SetInput(Cube.Input.KindOf input)
        {
            _input = input;
        }
        /// <summary>
        /// Índice do cubo.<br/>
        /// Somente leitura.
        /// </summary>
        public int Index => _index;
        /// <summary>
        /// Input atribuído ao cubo: X ou O.<br/>
        /// Somente leitura.
        /// </summary>
        public Cube.Input.KindOf Input => _input;
        /// <summary>
        /// Retorna verdadeiro se o cubo tiver um input atribuído, caso contrário, retorna falso.<br/>
        /// Somente leitura.
        /// </summary>
        /// <value><see cref="_input"/> > -1</value>
        public bool IsInputted => ((int)_input) > -1;
    }
}

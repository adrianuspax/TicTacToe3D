using System;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cube
{
    /// <summary>
    /// Argumentos para a manipulação de eventos
    /// </summary>
    [Serializable]
    public class Args : EventArgs
    {
        [Tooltip("Dados associados a interaçao com o cubo")]
        [SerializeField] private Data _data;
        private Coroutine _coroutine;
        /// <summary>
        /// Construtor da classe <see cref="Args"/>.
        /// </summary>
        /// <param name="data">Recebe os dados associados a interação com o cubo.</param>
        public Args(Data data, Coroutine coroutine)
        {
            this._data = data;
            _coroutine = coroutine;
        }
        /// <summary>
        /// Retorna <see cref="_data"/> (Apenas leitura)
        /// </summary>
        /// <value>Dados associados a interação com o cubo.</value>
        public Data Data => _data;

        public Coroutine Coroutine => _coroutine;
    }
}

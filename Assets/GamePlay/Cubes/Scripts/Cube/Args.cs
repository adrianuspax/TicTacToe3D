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
        [SerializeField] private Data _data; // Dados associados a interaçao com o cubo.
        /// <summary>
        /// Construtor da classe <see cref="Args"/>.
        /// </summary>
        /// <param name="data">Recebe os dados associados a interação com o cubo.</param>
        public Args(Data data)
        {
            this._data = data;
        }
        /// <summary>
        /// Retorna <see cref="_data"/> (Apenas leitura)
        /// </summary>
        /// <value>Dados associados a interação com o cubo.</value>
        public Data Data => _data;
    }
}

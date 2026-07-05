using System;
using System.Linq;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Main
{
    /// <inheritdoc/>
    public partial class AI
    {
        /// <summary>
        /// Struct responsável por armazenar o resultado do jogo.
        /// </summary>
        [Serializable]
        public struct Result
        {
            /// <summary>
            /// Resultado do jogo.
            /// </summary>
            public Main.Result main;
            /// <summary>
            /// Índices dos cubos que compõem a vitória.
            /// </summary>
            [NonReorderable] public int[] indexes;
            /// <summary>
            /// Construct da struct <see cref="Result"/>.
            /// </summary>
            /// <param name="result">Resultado do jogo.</param>
            /// <param name="indexes">Índices dos cubos que compõem a vitória.</param>
            public Result(Main.Result result = Main.Result.none, params int[] indexes)
            {
                main = result;
                this.indexes = indexes;
            }
            /// <summary>
            /// Obtenha os índices dos cubos que não fazem parte da vitória.
            /// </summary>
            /// <returns>Array dos índices</returns>
            public int[] GetExceptIndexes()
            {
                var array = new int[9]{0, 1, 2, 3, 4, 5, 6, 7, 8};
                return array.Except(indexes).ToArray();
            }
        }
    }
}

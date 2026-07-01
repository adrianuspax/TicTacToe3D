using UnityEngine;

namespace TicTacToe3D.Inheritance
{
    /// <inheritdoc/>
    public abstract class Manager<T> : Management<T> where T : Component
    {
        protected override void Awake()
        {
            base.Awake();
            Instance = base.Instance;
        }

        public new static T Instance { get; private set; }
    }
}

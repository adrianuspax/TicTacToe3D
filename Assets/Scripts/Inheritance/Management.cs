using ASPax.Attributes.Meta;
using ASPax.Extensions;
using UnityEngine;

namespace TicTacToe3D.Inheritance
{
    /// <summary>
    /// Generic management class that provides a base for components that need to manage another component. <br/>
    /// It handles the automatic assignment of the component reference.
    /// </summary>
    /// <typeparam name="T">The type of the component to be managed.</typeparam>
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class Management<T> : MonoBehaviour where T : Component
    {
#if UNITY_EDITOR
        ///<inheritdoc/>
        protected virtual void Reset()
        {
            ComponentsAssignment();
        }
#endif
        [SerializeField, ReadOnly] private T _instance; // The component being controlled.
        ///<inheritdoc/>
        protected virtual void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        public virtual void ComponentsAssignment()
        {
            _instance = GetComponent<T>();
            hideFlags = HideFlags.HideInInspector;
            this.GetComponentIfNull(ref _instance);
        }
        /// <summary>
        /// The component being controlled.
        /// </summary>
        protected T Instance => _instance;
    }
}

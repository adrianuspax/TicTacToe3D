using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Main.Cubes
{
    /// <summary>
    /// Classe reponsável pelo acesso a todos os cubos.<br/>
    /// Use <see cref="Manager"/> para acessar a classe como monosingleton.
    /// </summary>
    [RequireComponent(typeof(Manager))]
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [Tooltip("Array de todos os cubos.")]
        [SerializeField, NonReorderable, ReadOnly] private Cube.Control[] cubes;
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentsInAllChildrenIfNull(ref cubes);
        }
        /// <summary>
        /// Array de todos os cubos.
        /// </summary>
        public Cube.Control[] Array => cubes;
    }
}

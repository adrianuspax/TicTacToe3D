using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Handlers;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.UI.Animations
{
    public class Behaviour : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Animator animator;

        [Header(Header.scripts, order = 0), HorizontalLine]
        [SerializeField, ReadOnly] private AnimatorHandler animatorHandler;
        ///<inheritdoc/>
        protected virtual void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public virtual void ComponentsAssignment()
        {

        }
    }
}

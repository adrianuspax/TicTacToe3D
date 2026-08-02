
using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cubes.Sparks
{
    public class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 0)]
        [SerializeField, ReadOnly] private Animator animator;
        [SerializeField, ReadOnly] private ParticleSystem particleSys;

        private readonly int ID = Animator.StringToHash("start");
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentInChildrenIfNull(ref animator);
            this.GetComponentInChildrenIfNull(ref particleSys);
        }
        /// <summary>
        /// Aciona a animação e o sistema de partículas "Sparks".
        /// </summary>
        [Button(nameof(Play), SButtonEnableMode.Playmode)]
        public void Play()
        {
            var y = Random.Range(0f, 360f);
            animator.SetTrigger(ID);
            particleSys.transform.localRotation = Quaternion.Euler(0f, y, 0f);
            particleSys.Play();
        }
    }
}

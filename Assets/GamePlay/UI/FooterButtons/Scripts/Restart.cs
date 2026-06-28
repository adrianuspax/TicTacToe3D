using ASPax.Attributes.Drawer.SpecialCases;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TicTacToe3D.GamePlay.UI.Footer.Button
{
    public class Restart : TicTacToe3D.UI.Interaction.Button.Default
    {
        public event UnityAction OnButtonAddListner;
        ///<inheritdoc/>
        protected override void Start()
        {
            base.AddListener(ResetGame);
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public override void ComponentsAssignment()
        {
            base.ComponentsAssignment();
        }
        ///<inheritdoc/>
        public override void AddListener(UnityEngine.Events.UnityAction call)
        {
            base.AddListener(call);
            OnButtonAddListner?.Invoke();
        }

        public void ResetGame()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

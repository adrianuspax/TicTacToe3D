using UnityEngine.Events;

namespace TicTacToe3D.UI.Interaction.Button
{
    using Button = UnityEngine.UI.Button;

    public class Default : Inheritance.Pressable
    {
        /*///<inheritdoc/>
        protected override void Start()
        {
            base.Start();
            ((Button)selectable).onClick.AddListener(ButtonBehaviour);
        }*/

        public virtual void AddListener(UnityEngine.Events.UnityAction call)
        {
            ((Button)selectable).onClick.AddListener(call);
        }

        public virtual void RemoveListener(UnityAction call)
        {
            ((Button)selectable).onClick.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            ((Button)selectable).onClick.RemoveAllListeners();
        }

        /*public virtual void ButtonBehaviour()
        {
            
        }*/
    }
}

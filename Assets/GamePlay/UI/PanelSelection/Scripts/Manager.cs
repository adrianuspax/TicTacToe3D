namespace TicTacToe3D.GamePlay.UI.PanelSelection
{
    /// <summary>
    /// Manages the lifecycle and provides static access to the Popup Notice control script.<br/>
    /// This class follows a singleton-like pattern for easy access from other modules.
    /// </summary>
    public class Manager : Inheritance.Management<Control>
    {
        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();
            Control = base.Control;
        }
        /// <summary>
        /// Gets the static instance of the popup notice's main control script.<br/>
        /// This allows other modules to easily interact with the popup notice functionalities.
        /// </summary>
        public new static Control Control { private set; get; }
    }
}

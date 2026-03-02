using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D.UI.Interaction.Inheritance
{
    /// <summary>
    /// Interface to be implemented by buttons
    /// </summary>
    public interface IPressable
    {
        /// <summary>
        /// Remove all listeners from the button
        /// </summary>
        void RemoveAllListeners();
        /// <summary>
        /// Set the visibility of the button or toggle
        /// </summary>
        void SetVisible(bool value);
        /// <summary>
        /// Fade in the button or toggle
        /// </summary>
        /// <param name="time">Time to fade in</param>
        void FadeIn(float time);
        /// <summary>
        /// Fade out the button or toggle
        /// </summary>
        /// <param name="time">Time to fade out</param>
        void FadeOut(float time);
        /// <summary>
        /// Set the Anchors max and min
        /// </summary>
        /// <param name="max">anchor max x and y</param>
        /// <param name="min">anchor min x and y</param>
        void SetAnchors((float x, float y) max, (float x, float y) min);
        /// <summary>
        /// Return the <see cref="TextMeshProUGUI"/> component.
        /// </summary>
        TextMeshProUGUI TMPro { get; }
        /// <summary>
        /// Return the <see cref="UnityEngine.CanvasGroup"/> component.
        /// </summary>
        CanvasGroup CanvasGroup { get; }
        /// <summary>
        /// The button is visible?
        /// </summary>
        bool IsVisible { get; }
        /// <summary>
        /// Return <see cref="UnityEngine.UI.Image"/> components: icon and ring.
        /// </summary>
        Image Icon { get; }
        /// <summary>
        /// Pivot Custom Component
        /// </summary>
        RectTransform RectTransform { get; }
    }
}

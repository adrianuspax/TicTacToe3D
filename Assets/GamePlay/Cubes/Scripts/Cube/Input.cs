using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.GamePlay.Cube
{
    public partial class Input : MonoBehaviour
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField] private float time;
        [SerializeField] private float frequence;
        [SerializeField] private Vector2 rangeAlpha;
        [SerializeField, Range(0f, 1f)] private float softChance;
        [SerializeField] private float softTime;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private bool isVisible;
        [SerializeField, ReadOnly] private bool isOn;
        [SerializeField, ReadOnly] private Color originalColor;
        [SerializeField, ReadOnly] private int emissionColorID;
        [Header(Header.components, order = 0)]
        [SerializeField, ReadOnly] private MeshRenderer meshRenderer;
        [SerializeField, ReadOnly] private Light light_;

        private MaterialPropertyBlock _materialPropertyBlock;
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset), SButtonEnableMode.Editor)]
        private void Reset()
        {
            emissionColorID = Shader.PropertyToID("_EmissionColor");
            originalColor = meshRenderer.sharedMaterial.GetColor(emissionColorID);
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(Visibility))]
        private void Visibility()
        {
            SetVisibility(!isVisible);
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(Light))]
        private void Light()
        {
            SetTurn(!isOn);
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(TurnFlicker))]
        private void TurnFlicker()
        {
            SetTurnFlicker(!isOn, 0.5f, true);
        }
#endif
        ///<inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void Start()
        {
            isVisible = true;
            isOn = true; // Required for proper function // Must match the material's default value in the editor

            emissionColorID = Shader.PropertyToID("_EmissionColor");
            _materialPropertyBlock = new();
            meshRenderer.GetPropertyBlock(_materialPropertyBlock);
            originalColor = meshRenderer.sharedMaterial.GetColor(emissionColorID);

            SetTurn(false);
            SetVisibility(false); // The order is important here!
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref meshRenderer);
            this.GetComponentIfNull(ref light_, 0);
        }

        public void SetVisibility(bool value)
        {
            if (value != isVisible)
                meshRenderer.enabled = value;

            light_.enabled = value && isOn;
            value.ComparativeAssignment(ref isVisible);
        }

        public void SetMaterialLightColor(Color color)
        {
            _materialPropertyBlock.SetColor(emissionColorID, color);
            meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public Coroutine SetTurn(bool isOn, float delay = 0f)
        {
            if (isVisible)
            {
                var routine = _coroutine();
                return StartCoroutine(routine);
            }
            else
            {
                Debug.LogWarning("Cannot change the turn state when the input is not visible!");
                return null;
            }

            IEnumerator _coroutine()
            {
                if (delay > 0f)
                    yield return new WaitForSeconds(delay);

                SetMaterialLightColor(isOn ? originalColor : Color.black);
                light_.enabled = isOn;
                isOn.ComparativeAssignment(ref this.isOn);
            }
        }

        public Coroutine SetTurnFlicker(bool isOn, float delay = 0f, bool forceVisibility = false)
        {
            if (forceVisibility)
                SetVisibility(true);

            if (isVisible)
            {
                var routine = _coroutine();
                return StartCoroutine(routine);
            }
            else
            {
                Debug.LogWarning("Cannot change the turn state when the input is not visible!");
                return null;
            }

            IEnumerator _coroutine()
            {
                if (delay > 0f)
                    yield return new WaitForSeconds(delay);

                var routine = Flicker(time, frequence, rangeAlpha, softChance, softTime);
                var coroutine = StartCoroutine(routine);


                if (isOn)
                {
                    yield return SetTurn(true);
                    yield return coroutine;
                }
                else
                {
                    yield return coroutine;
                    yield return SetTurn(false);
                }
            }
        }

        public IEnumerator Flicker(float time, float frequence, Vector2 rangeAlpha, float softChance, float softTime)
        {
            if (time <= 0f)
            {
                Debug.LogError("Flicker time must be greater than zero!");
                yield break;
            }

            if (rangeAlpha.x < 0f || rangeAlpha.y > 1f)
            {
                rangeAlpha = new()
                {
                    x = Mathf.Clamp01(rangeAlpha.x),
                    y = Mathf.Clamp01(rangeAlpha.y)
                };

                Debug.LogWarning("Alpha range values must be between 0 and 1! Clamped values have been applied.");
            }

            if (softChance < 0f || softChance > 1f)
            {
                softChance = Mathf.Clamp01(softChance);
                Debug.LogWarning("Soft flicker chance must be between 0 and 1! Clamped value has been applied.");
            }

            var runningTime = 0f;
            var interval = 1.0f / Mathf.Max(0.1f, frequence);
            var originalIntensity = light_.intensity;

            while (runningTime < time)
            {
                var intensity = Random.Range(rangeAlpha.x, rangeAlpha.y);
                var isSoft = Random.value < softChance;

                if (isSoft)
                {
                    var t = 0f;
                    var corAlvo = originalColor * intensity;

                    while (t < softTime)
                    {
                        t += Time.deltaTime;
                        var novaCor = Color.Lerp(originalColor, corAlvo, t / softTime);
                        SetMaterialLightColor(novaCor);
                        light_.intensity = Mathf.Lerp(originalIntensity, intensity, t / softTime);
                        yield return null;
                    }

                    runningTime += softTime;
                }
                else
                {
                    light_.intensity = intensity;
                    SetMaterialLightColor(originalColor * intensity);
                    var wait = Random.Range(interval * 0.5f, interval * 1.5f);
                    yield return new WaitForSeconds(wait);
                    runningTime += wait;
                }

                if (!isSoft && Random.value > 0.5f)
                {
                    light_.intensity = 0;
                    SetMaterialLightColor(Color.black);
                    var random = Random.Range(0.01f, 0.05f);
                    yield return new WaitForSeconds(random);
                }
            }

            SetMaterialLightColor(originalColor);
            light_.intensity = originalIntensity;
        }

        public bool IsVisible => isVisible;
        public bool IsOn => isOn;
    }
}
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
        [Tooltip("Tempo para a nimação do Flicker")]
        [SerializeField] private float _time;
        [Tooltip("Frequência para a animação do Flicker")]
        [SerializeField] private float _frequence;
        [Tooltip("Intervalo de valores de alpha para a animação do Flicker")]
        [SerializeField] private Vector2 _rangeAlpha;
        [Tooltip("Chance de ocorrer a animação do Flicker de forma suave")]
        [SerializeField, Range(0f, 1f)] private float _softChance;
        [Tooltip("Tempo para a animação do Flicker ocorrer de forma suave")]
        [SerializeField] private float _softTime;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [Tooltip("Verdadeiro se o input está visível")]
        [SerializeField, ReadOnly] private bool _isVisible;
        [Tooltip("Verdadeiro se o input está ligado (Luz)")]
        [SerializeField, ReadOnly] private bool _isOn;
        [Tooltip("Cor do input ligado")]
        [SerializeField, ReadOnly] private Color _originalColor;
        [Tooltip("ID do shader referente ao Emission Color")]
        [SerializeField, ReadOnly] private int _emissionColorID;
        [Header(Header.components, order = 0)]
        [Tooltip("Componente do Mehs Renderer")]
        [SerializeField, ReadOnly] private MeshRenderer _meshRenderer;
        [Tooltip("Componente Light")]
        [SerializeField, ReadOnly] private Light _light;

        private MaterialPropertyBlock _materialPropertyBlock; // Propriedade para controlar as propriedades do material.
#if UNITY_EDITOR
        ///<inheritdoc/>
        [Button(nameof(Reset), SButtonEnableMode.Editor)]
        private void Reset()
        {
            _emissionColorID = Shader.PropertyToID("_EmissionColor");
            _originalColor = _meshRenderer.sharedMaterial.GetColor(_emissionColorID);
            _time = 0.5f;
            _frequence = 50f;
            _rangeAlpha = new(0.2f, 1f);
            _softChance = 0.3f;
            _softTime = 0.1f;
        }
        /// <summary>
        /// Método de teste para deixar vísivel o input.
        /// </summary>
        [Button(nameof(Visibility), SButtonEnableMode.Playmode)]
        private void Visibility()
        {
            SetVisibility(!_isVisible);
        }
        /// <summary>
        /// Método de teste para ativar ou desativar o input (Luz).
        /// </summary>
        [Button(nameof(Light), SButtonEnableMode.Playmode)]
        private void Light()
        {
            SetTurn(!_isOn);
        }
        /// <summary>
        /// Método de teste para iniciar a animação de Flicker.
        /// </summary>
        [Button(nameof(TurnFlicker), SButtonEnableMode.Playmode)]
        private void TurnFlicker()
        {
            SetTurnFlicker(!_isOn, 0.5f, true);
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
            _isVisible = true;
            _isOn = true; // Required for proper function // Must match the material's default value in the editor

            _emissionColorID = Shader.PropertyToID("_EmissionColor");
            _materialPropertyBlock = new();
            _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
            _originalColor = _meshRenderer.sharedMaterial.GetColor(_emissionColorID);

            SetTurn(false);
            SetVisibility(false); // A ordem é importante aqui, pois o método SetTurn depende do estado de visibilidade para funcionar corretamente.
        }
        ///<inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref _meshRenderer);
            this.GetComponentIfNull(ref _light, 0);
        }
        /// <summary>
        /// Método para controlar a visibilidade do input.
        /// </summary>
        /// <param name="value">Verdadeiro para ativar a visibilidade. Falso para desativar a visibilidade.</param>
        public void SetVisibility(bool value)
        {
            if (value != _isVisible)
                _meshRenderer.enabled = value;

            _light.enabled = value && _isOn;
            value.ComparativeAssignment(ref _isVisible);
        }
        /// <summary>
        /// Método para atribuir a cor ao material do input.
        /// </summary>
        /// <param name="color">Cor</param>
        public void SetMaterialLightColor(Color color)
        {
            _materialPropertyBlock.SetColor(_emissionColorID, color);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Coroutine SetTurn(bool isOn, float delay = 0f)
        {
            if (_isVisible)
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

                SetMaterialLightColor(isOn ? _originalColor : Color.black);
                _light.enabled = isOn;
                isOn.ComparativeAssignment(ref this._isOn);
            }
        }

        public Coroutine SetTurnFlicker(bool isOn, float delay = 0f, bool forceVisibility = false)
        {
            if (forceVisibility)
                SetVisibility(true);

            if (_isVisible)
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

                var routine = Flicker(_time, _frequence, _rangeAlpha, _softChance, _softTime);
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
            var originalIntensity = _light.intensity;

            while (runningTime < time)
            {
                var intensity = Random.Range(rangeAlpha.x, rangeAlpha.y);
                var isSoft = Random.value < softChance;

                if (isSoft)
                {
                    var t = 0f;
                    var corAlvo = _originalColor * intensity;

                    while (t < softTime)
                    {
                        t += Time.deltaTime;
                        var novaCor = Color.Lerp(_originalColor, corAlvo, t / softTime);
                        SetMaterialLightColor(novaCor);
                        _light.intensity = Mathf.Lerp(originalIntensity, intensity, t / softTime);
                        yield return null;
                    }

                    runningTime += softTime;
                }
                else
                {
                    _light.intensity = intensity;
                    SetMaterialLightColor(_originalColor * intensity);
                    var wait = Random.Range(interval * 0.5f, interval * 1.5f);
                    yield return new WaitForSeconds(wait);
                    runningTime += wait;
                }

                if (!isSoft && Random.value > 0.5f)
                {
                    _light.intensity = 0;
                    SetMaterialLightColor(Color.black);
                    var random = Random.Range(0.01f, 0.05f);
                    yield return new WaitForSeconds(random);
                }
            }

            SetMaterialLightColor(_originalColor);
            _light.intensity = originalIntensity;
        }

        public bool IsVisible => _isVisible;
        public bool IsOn => _isOn;
    }
}
#nullable enable

using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Core.Presentation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GanShin.UI
{
    [RequireComponent(typeof(ContextHolder), typeof(CanvasGroup))]
    public abstract class UIRootBase : MonoBehaviour
    {
        [Header("UI Root Base")] [SerializeField]
        private float fadeDuration = 0.2f;

        private CanvasGroup _canvasGroup = null!;

        protected readonly Dictionary<Type, Object[]> _objects = new();

        [UsedImplicitly]
        protected UIManager? UIManager { get; private set; } = ProjectManager.Instance.GetManager<UIManager>();

        protected ContextHolder? ContextHolder { get; private set; }

        public Context? DataContext { get; protected set; }

        protected CanvasRoot? CanvasRoot { get; private set; }
        
        private Tweener? _fadeTweener;

        protected virtual void Awake()
        {
            _canvasGroup          = GetComponent<CanvasGroup>();
            ContextHolder         = GetComponent<ContextHolder>();
            DataContext           = InitializeDataContext();
            ContextHolder.Context = DataContext;
        }

        protected abstract Context? InitializeDataContext();

        public void CreateContext()
        {
            if (ContextHolder == null)
            {
                GanDebugger.LogError(nameof(UIRootBase), "ContextHolder is null");
                return;
            }

            ContextHolder.CreateContext = true;

            var newContext = Activator.CreateInstance(ContextHolder.ContextType);
            ContextHolder.SetContext(newContext, null);
        }

        public void InjectCanvasRoot(CanvasRoot root)
        {
            CanvasRoot = root;
        }

        public void Show()
        {
            _fadeTweener?.Kill();
            _fadeTweener = _canvasGroup.DOFade(1, fadeDuration);
        }

        public void Hide()
        {
            _fadeTweener?.Kill();
            _fadeTweener = _canvasGroup.DOFade(0, fadeDuration);
        }

        protected virtual void OnDestroy()
        {
            _fadeTweener?.Kill();
        }

        #region Enum-based Binding

        protected void Bind<T>(Type enumType) where T : Object
        {
            var type = typeof(T);
            if (_objects.ContainsKey(type))
            {
                GanDebugger.LogWarning(nameof(UIRootBase), $"Type {type.Name} is already bound. Skipping duplicate binding.");
                return;
            }

            var names = Enum.GetNames(enumType);
            var objects = new Object[names.Length];
            _objects.Add(type, objects);

            for (var i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true)!;
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true)!;

                if (objects[i] == null)
                {
                    GanDebugger.LogWarning(nameof(UIRootBase), $"Failed to bind {type.Name} with name '{names[i]}' in {gameObject.name}");
                }
            }
        }

        protected void BindGameObjects(Type enumType) => Bind<GameObject>(enumType);
        protected void BindTexts(Type enumType) => Bind<TMP_Text>(enumType);
        protected void BindImages(Type enumType) => Bind<Image>(enumType);
        protected void BindButtons(Type enumType) => Bind<Button>(enumType);
        protected void BindToggles(Type enumType) => Bind<Toggle>(enumType);
        protected void BindSliders(Type enumType) => Bind<Slider>(enumType);
        protected void BindDropdowns(Type enumType) => Bind<TMP_Dropdown>(enumType);
        protected void BindInputFields(Type enumType) => Bind<TMP_InputField>(enumType);

        protected T? Get<T>(int idx) where T : Object
        {
            if (!_objects.TryGetValue(typeof(T), out var objects))
                return null;

            if (idx < 0 || idx >= objects.Length)
                return null;

            return objects[idx] as T;
        }

        protected GameObject? GetGameObject(int idx) => Get<GameObject>(idx);
        protected TMP_Text? GetText(int idx) => Get<TMP_Text>(idx);
        protected Image? GetImage(int idx) => Get<Image>(idx);
        protected Button? GetButton(int idx) => Get<Button>(idx);
        protected Toggle? GetToggle(int idx) => Get<Toggle>(idx);
        protected Slider? GetSlider(int idx) => Get<Slider>(idx);
        protected TMP_Dropdown? GetDropdown(int idx) => Get<TMP_Dropdown>(idx);
        protected TMP_InputField? GetInputField(int idx) => Get<TMP_InputField>(idx);

        #endregion
    }
}
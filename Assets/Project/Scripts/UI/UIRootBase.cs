using System.ComponentModel;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    [RequireComponent(typeof(ContextHolder))]
    public abstract class UIRootBase : MonoBehaviour
    {
        [UsedImplicitly]
        [Inject] protected UIManager UIManager { get; private set; }
        
        protected ContextHolder ContextHolder { get; private set; }
        
        public INotifyPropertyChanged DataContext { get; protected set; }

        protected virtual void Awake()
        {
            ContextHolder         = GetComponent<ContextHolder>();
            DataContext           = InitializeDataContext();
            ContextHolder.Context = DataContext;
        }
        
        protected abstract INotifyPropertyChanged InitializeDataContext();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using GanShin.AssetManagement;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
	[UsedImplicitly]
    public partial class UIManager : IInitializable
    {
	    [Inject] private ResourceManager _resource;
	    
	    private readonly Dictionary<Type ,INotifyPropertyChanged> _dataContexts = new();

	    public GameObject GlobalRoot { get; private set; } = null;

	    [UsedImplicitly]
	    public UIManager()
	    {
	    }

	    #region Context Management Interface
	    public T GetContext<T>() where T : class, INotifyPropertyChanged
	    {
		    if(_dataContexts.ContainsKey(typeof(T)))
			    return _dataContexts[typeof(T)] as T;
		    return null;
	    }
	    
	    public T GetOrAddContext<T>() where T : class, INotifyPropertyChanged, new()
	    {
		    if(_dataContexts.ContainsKey(typeof(T)))
			    return _dataContexts[typeof(T)] as T;
		    var context = new T();
		    AddContext(context);
		    return context;
	    }
	    
	    public void AddContext<T>(T context) where T : class, INotifyPropertyChanged
	    {
		    var result = _dataContexts.TryAdd(typeof(T), context);
		    if (!result)
			    GanDebugger.LogWarning(nameof(UIManager), "Context already exists");
		}
	    
	    public void RemoveContext<T>(T context) where T : class, INotifyPropertyChanged
	    {
		    _dataContexts.Remove(typeof(T));
	    }
	    #endregion Context Management Interface

	    public void Initialize()
	    {
		    AddEventSystem();
		    AddGlobalUIRoot();
	    }
    }
}

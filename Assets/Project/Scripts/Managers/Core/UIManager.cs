using System.Collections.Generic;
using GanShin.AssetManagement;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GanShin.UI
{
	[UsedImplicitly]
    public partial class UIManager : IInitializable
    {
	    [Inject] private ResourceManager _resource;

	    public GameObject GlobalRoot { get; private set; } = null;

	    [UsedImplicitly]
	    public UIManager()
	    {
	    }

	    public void Initialize()
	    {
		    AddEventSystem();
		    AddGlobalUIRoot();
		    InitializeGlobalUIs();
	    }
    }
}

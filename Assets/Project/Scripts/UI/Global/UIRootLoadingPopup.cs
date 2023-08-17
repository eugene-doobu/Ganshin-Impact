#nullable enable

using System.Collections.Generic;
using System.ComponentModel;

namespace GanShin.UI
{
    public class UIRootLoadingPopup : GlobalUIRootBase
    {
        private readonly HashSet<int> _set = new();

        public void AddHash(int hash) => _set.Add(hash);
        
        public void RemoveHash(int hash) => _set.Remove(hash);
        
        public void ClearHash() => _set.Clear();
        
        public bool IsEmpty() => _set.Count == 0;
        
        protected override INotifyPropertyChanged? InitializeDataContext() => null;

        public override void InitializeContextData() { }

        public override void ClearContextData() { }
    }
}
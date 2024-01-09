using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class GanContext : Context, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System.ComponentModel;

namespace GanShin.UI
{
    public interface IDataContextOwner
    {
        public INotifyPropertyChanged DataContext { get; }
    }
}

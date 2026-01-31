using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public interface IDataContextOwner
    {
        public Context DataContext { get; }
    }
}
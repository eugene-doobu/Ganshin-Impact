namespace GanShin.Data.Core
{
    /// <summary>
    /// Base interface for all stat tables.
    /// Defines common properties shared across character and monster data.
    /// </summary>
    public interface IStatTable
    {
        float Hp { get; }
    }
}

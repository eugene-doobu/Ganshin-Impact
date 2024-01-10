using GanShin.GanObject;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class FieldMonsterManagerContext : ActorManagerContext
    {
        protected override void AddContext(Actor actor)
        {
            Add(actor.Id, new FieldMonsterContext());
        }
    }
}

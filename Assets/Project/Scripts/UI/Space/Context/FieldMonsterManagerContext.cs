using GanShin.Content.Creature.Monster;
using GanShin.GanObject;
using JetBrains.Annotations;

namespace GanShin.UI.Space
{
    [UsedImplicitly]
    public class FieldMonsterManagerContext : ActorManagerContext
    {
        protected override void AddContext(Actor actor)
        {
            if (actor is not FieldMonsterController)
                return;

            Add(actor.Id, new FieldMonsterContext());
        }
    }
}

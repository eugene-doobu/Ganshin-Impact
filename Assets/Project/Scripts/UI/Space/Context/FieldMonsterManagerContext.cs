using GanShin.Entities;
using GanShin.Entities.Enemy.FieldEnemy;
using JetBrains.Annotations;

namespace GanShin.UI.Space
{
    [UsedImplicitly]
    public class FieldMonsterManagerContext : ActorManagerContext
    {
        protected override void AddContext(Actor actor)
        {
            if (actor is not FieldEnemyController)
                return;

            Add(actor.Id, new FieldMonsterContext());
        }
    }
}

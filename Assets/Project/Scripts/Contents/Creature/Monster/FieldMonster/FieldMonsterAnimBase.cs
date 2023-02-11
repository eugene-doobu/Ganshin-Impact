using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    public enum eFieldMonsterType
    {
        DEFAULT,
        HUMANOID,
    }
    
    public abstract class FieldMonsterAnimBase
    {
        public abstract void Initialize(Animator animator);
        public abstract void OnAttack();
        public abstract void OnDamaged();
        public abstract void OnDie();
        public abstract void OnIdle();
        public abstract void OnMove();
    }
}

using UnityEngine;

namespace GanShin.Entities.Enemy.FieldEnemy
{
    public enum eFieldEnemyType
    {
        DEFAULT,
        HUMANOID
    }

    public abstract class FieldEnemyAnimBase
    {
        public abstract void Initialize(Animator animator);
        public abstract void OnAttack();
        public abstract void OnDamaged();
        public abstract void OnDie();
        public abstract void OnIdle();
        public abstract void OnMove();
    }
}
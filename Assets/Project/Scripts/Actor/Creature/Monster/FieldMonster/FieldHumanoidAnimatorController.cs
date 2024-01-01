using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    public class FieldHumanoidAnimatorController : FieldMonsterAnimBase
    {
        private static readonly int AnimParam = Animator.StringToHash("animation");

        private Animator _animator;
        private bool     _isInitialized;

        public override void Initialize(Animator animator)
        {
            _animator = animator;
        }

        public override void OnAttack()
        {
            if (!_isInitialized) return;
        }

        public override void OnDamaged()
        {
            if (!_isInitialized) return;
        }

        public override void OnDie()
        {
            if (!_isInitialized) return;
        }

        public override void OnIdle()
        {
            if (!_isInitialized) return;
        }

        public override void OnMove()
        {
            if (!_isInitialized) return;
        }
    }
}
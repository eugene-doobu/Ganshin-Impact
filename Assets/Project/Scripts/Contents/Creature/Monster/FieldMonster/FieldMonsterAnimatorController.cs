using System.Collections;
using System.Collections.Generic;
using GanShin.Content.Creature.Monsters;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    /// <summary>
    /// 가장 기본적인 형태의 Generic 모델 몬스터 위한 애니메이터 컨트롤러<br/>
    /// 단순히 AnimParam에 애니메이션 인덱스를 넣어주는 형태<br/>
    /// </summary>
    public class FieldMonsterAnimatorController : FieldMonsterAnimBase
    {
        private enum eAnimation
        {
            IDLE = 1,
            MOVE,
            ATTACK,
            DAMAGED,
            DIE,
        }
        
        private static readonly int AnimParam = Animator.StringToHash("animation");
        
        private Animator _animator;
        private bool     _isInitialized;
        
        private eAnimation _currentAnimation;

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
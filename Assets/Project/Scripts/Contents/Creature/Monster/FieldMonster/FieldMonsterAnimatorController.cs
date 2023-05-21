using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    /// <summary>
    /// 가장 기본적인 형태의 Generic 모델 몬스터 위한 애니메이터 컨트롤러<br/>
    /// 단순히 AnimParam에 애니메이션 인덱스를 넣어주는 형태<br/>
    /// </summary>
    public class FieldMonsterAnimatorController : FieldMonsterAnimBase
    {
        private enum eAnimState
        {
            IDLE = 1,
            MOVE,
            ATTACK,
            DAMAGED,
            DIE,
        }

        private static readonly int AnimParam = Animator.StringToHash("animation");

        private float    _attackAnimLength;
        private Animator _animator;
        private bool     _isInitialized;

        public override void Initialize(Animator animator)
        {
            _animator      = animator;
            _isInitialized = true;
        }

        public override void OnAttack()
        {
            if (!_isInitialized) return;
            _animator.SetInteger(AnimParam, (int) eAnimState.ATTACK);
        }

        public override void OnDamaged()
        {
            if (!_isInitialized) return;
            _animator.SetInteger(AnimParam, (int) eAnimState.DAMAGED);
        }

        public override void OnDie()
        {
            if (!_isInitialized) return;
            _animator.SetInteger(AnimParam, (int) eAnimState.DIE);
        }

        public override void OnIdle()
        {
            if (!_isInitialized) return;
            _animator.SetInteger(AnimParam, (int) eAnimState.IDLE);
        }

        public override void OnMove()
        {
            if (!_isInitialized) return;
            _animator.SetInteger(AnimParam, (int) eAnimState.MOVE);
        }
    }
}
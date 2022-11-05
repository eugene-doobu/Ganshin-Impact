using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Creature
{
    public abstract class CreatureController : MonoBehaviour
    {
        #region Variables

        private bool     _hasAnimator;
        private Animator _animator;
        
        #endregion Variables

        #region Properties

        protected Animator Animator => _animator;
        protected bool HasAnimator => _hasAnimator;
        
        #endregion Properties

        #region TableData // TODO: 별도의 클래스로 분리 후 json 파일로 관리 예정

        private float _moveSpeed   = 4f;
        private float _rotateSpeed = 2f;

        #endregion TableData
        
        #region Mono
        // TODO: MapController의 Init로 변경
        protected virtual void Awake()
        {
            // Unity Null체크의 비용은 무겁기 때문에 미리 체크한 후 결과를 캐싱하여 사용
            _hasAnimator = TryGetComponent(out _animator);
        }

        protected virtual void Start()
        {
        
        }

        // TODO: MapController의 OnUpdate로 변경
        protected virtual void Update()
        {
            Movement(_moveSpeed);
        }
        #endregion Mono

        protected abstract void Movement(float moveSpeed);
    }
}

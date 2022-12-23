using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.MapObject
{
    /// <summary>
    /// (다른 타입보다 더) 스스로의 판단으로 움직이는 오브젝트들
    /// 플레이어와 몬스터가 이 클래스에 속함
    /// 투사체의 경우도 특정 목표를 가지로 스스로 움직인다고 판단하여 Creature로 분류
    /// </summary>
    public class CreatureObject : MapObject
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}

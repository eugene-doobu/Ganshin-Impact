using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.MapObject
{
    /// <summary>
    /// (겉으로 보이기에)스스로 판단하여 움직일 수 없으며,
    /// CreatureObject의 행동의 결과로 움직이는 오브젝트
    /// 무기, 움직일 수 있는 구조물 등이 MapObject에 속한다
    /// </summary>
    public class PassiveObject : MapObject
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
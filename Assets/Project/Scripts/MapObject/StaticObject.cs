using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.MapObject
{
    /// <summary>
    /// 움직이지 않는 오브젝트
    /// MapObject의 가장 기본적인 구현
    /// </summary>
    public class StaticObject : MapObject
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
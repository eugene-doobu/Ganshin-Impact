using System.Collections;
using System.Collections.Generic;
using GanShin.Data;

namespace GanShin.Content.Weapon
{
    public class AiStaff : PlayerWeaponBase
    {
        public override void OnAttack()
        {
            var stat = Owner.Stat as AiStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not AiStatTable");
                return;
            }

            switch (AttackType)
            {
                case ePlayerAttack.AI_ATTACK1:
                    MakeProjectile(1);
                    break;
                case ePlayerAttack.AI_ATTACK2:
                    MakeProjectile(2);
                    break;
            }
        }
        
        private void MakeProjectile(int num)
        {
            GanDebugger.LogWarning(num + "번째 공격");
        }

        public override void OnSkill()
        {
            // 번쩍이 이펙트 노랭
        }

        public override void OnUltimate()
        {
            // 번쩍이 이펙트 파랭
        }
    }
}

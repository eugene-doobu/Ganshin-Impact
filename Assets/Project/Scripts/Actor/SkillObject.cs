using Cysharp.Threading.Tasks;

namespace GanShin.GanObject
{
    /// <summary>
    /// Owner인 CreatureObject가 존재하는 오브젝트
    /// Creature의 행동으로 인해 생성되며, Owner에 종속되는 오브젝트이다.
    /// 스킬 이펙트, 투사체등이 속한다
    /// </summary>
    public class SkillObject : Actor
    {
        private CreatureObject _owner;
        
        public virtual CreatureObject Owner
        {
            get => _owner;
            set
            {
                if (_owner != null)
                {
                    GanDebugger.ActorLogError("Owner is already set");
                    return;
                }
                _owner = value;
            }
        }
    }
}
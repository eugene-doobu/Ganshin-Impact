namespace GanShin.GanObject
{
    /// <summary>
    /// 가장 기본적인 형태로 움직이지 않는 오브젝트
    /// 월드의 구조물, 아이템, 무기 등이 속한다
    /// 스킬 이펙트, 투사체, 무기, 움직일 수 있는 구조물 등이 속한다
    /// </summary>
    public class PassiveObject : Actor
    {
        public override void Tick()
        {
            base.Tick();
        }
    }
}
namespace GanShin.Director
{
#nullable enable

    /// <summary>
    ///     유니티 에셋시스템 이슈 대응을 위한 빈 디렉터
    ///     Build Settings의 Scenes In Build에 포함된 씬에
    ///     빈 타임라인 에셋을 포함하여 사용할 것
    /// </summary>
    public class EmptyDirector : GanshinDirector
    {
        public override void Play(IDirectorMessage? message = null)
        {
        }

        public override void Stop()
        {
        }

        public override void Resume()
        {
        }

        public override void Pause()
        {
        }
    }
}
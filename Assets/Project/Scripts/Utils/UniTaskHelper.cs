#nullable enable

using System.Threading;
using Cysharp.Threading.Tasks;

namespace GanShin
{
    public static class UniTaskHelper
    {
        public static async UniTask<bool> Delay(int delayMs, CancellationTokenSource? tokenSource,
            PlayerLoopTiming timing = PlayerLoopTiming.Update)
        {
            if (tokenSource == null || tokenSource.IsCancellationRequested)
                return false;

            var isCancelled = await UniTask.Delay(delayMs, DelayType.UnscaledDeltaTime, timing, tokenSource.Token)
                .SuppressCancellationThrow();
            return !isCancelled;
        }
    }
}
using System;
using System.Runtime.CompilerServices;

namespace PostEnot.Toolkits
{
    internal static class TimeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ElapsedUnclamped(float startTime, float currentTime) => currentTime - startTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Elapsed(float startTime, float currentTime, float duration)
        {
            float elapsedUnclamped = ElapsedUnclamped(startTime, currentTime);
            return MathF.Min(elapsedUnclamped, duration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ElapsedRatio(float startTime, float currentTime, float duration, float zeroDurationReturn)
        {
            float elapsed = Elapsed(startTime, currentTime, duration);
            return CalculateRatio(elapsed, duration, zeroDurationReturn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ElapsedRatioUnclamped(float startTime, float currentTime, float duration, float zeroDurationReturn)
        {
            float elapsedUnclamped = ElapsedUnclamped(startTime, currentTime);
            return CalculateRatio(elapsedUnclamped, duration, zeroDurationReturn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemainingUnclamped(float startTime, float currentTime, float duration)
        {
            float elapsed = ElapsedUnclamped(startTime, currentTime);
            return duration - elapsed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remaining(float startTime, float currentTime, float duration)
        {
            float elapsed = Elapsed(startTime, currentTime, duration);
            return duration - elapsed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemainingRatioUnclamped(float startTime, float currentTime, float duration, float zeroDurationReturn)
        {
            float remainingUnclamped = RemainingUnclamped(startTime, currentTime, duration);
            return CalculateRatio(remainingUnclamped, duration, zeroDurationReturn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemainingRatio(float startTime, float currentTime, float duration, float zeroDurationReturn)
        {
            float remaining = Remaining(startTime, currentTime, duration);
            return CalculateRatio(remaining, duration, zeroDurationReturn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateRatio(float time, float duration, float zeroDurationReturn)
            => duration == 0.0f ? zeroDurationReturn : time / duration;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Reverse(float startTime, float currentTime, float duration)
            => currentTime - Remaining(startTime, currentTime, duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReverseUnclamped(float startTime, float currentTime, float duration)
            => currentTime - RemainingUnclamped(startTime, currentTime, duration);
    }
}

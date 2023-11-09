using System;

namespace Utilities
{
    public static class UtilityService
    {
        public static float ConvertToNewRange(int startValue, int first_range_min, int first_range_max, int second_range_min, int second_range_max)
        {
            int first_range_len = first_range_max - first_range_min;
            int second_range_len = second_range_max - second_range_min;

            float offset = startValue - first_range_min;
            float normalizedValue = offset / first_range_len;
            float upscaledValue = normalizedValue * second_range_len;
            float newValue = upscaledValue + second_range_min;
            return newValue;
        }
    }
}
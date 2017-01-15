namespace KPlugin.Extension
{
    using System;

    public static class ObjectExtension
    {
        public static object ChangeTypeWithCompatibility(this object input, Type targetType, out int compatibility)
        {
            Type type = input.GetType();

            if (targetType != typeof(bool) && targetType != typeof(int) && targetType != typeof(float) && targetType != typeof(string)
                || type != typeof(bool) && type != typeof(int) && type != typeof(float) && type != typeof(string))
                throw new NotSupportedException();

            compatibility = -1;

            if (type == targetType)
            {
                compatibility = 0;
                return input;
            }

            if (type == typeof(bool) && targetType == typeof(string))
            {
                compatibility = 8;
                return ((bool)input).ToString();
            }

            if (type == typeof(int) && targetType == typeof(float))
            {
                compatibility = 1;
                return (float)(int)input;
            }

            if (type == typeof(int) && targetType == typeof(string))
            {
                compatibility = 4;
                return ((int)input).ToString();
            }

            if (type == typeof(float) && targetType == typeof(string))
            {
                compatibility = 6;
                return ((float)input).ToString();
            }

            return null;
        }
    }
}

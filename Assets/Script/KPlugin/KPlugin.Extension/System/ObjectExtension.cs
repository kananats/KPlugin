namespace KPlugin.Extension
{
    using System;

    public static class ObjectExtension
    {
        public static object ChangeTypeWithCompatibility(this object input, Type targetType, out int compatibility)
        {
            Type type = input.GetType();

            if (type != typeof(bool) && type != typeof(int) && type != typeof(float) && type != typeof(string)
                || targetType != typeof(bool) && targetType != typeof(int) && targetType != typeof(float) && targetType != typeof(string) && !targetType.IsEnum)
                throw new NotSupportedException();

            if (type == targetType && (type == typeof(bool) || type == typeof(int) || type == typeof(float)))
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

            if (type == typeof(int) && targetType.IsEnum)
            {
                try
                {
                    compatibility = 6;
                    return Enum.ToObject(targetType, (int)input);
                }

                catch (Exception) { }
                compatibility = -1;
                return null;
            }

            if (type == typeof(float) && targetType == typeof(string))
            {
                compatibility = 8;
                return ((float)input).ToString();
            }

            if (type == typeof(string) && targetType == typeof(string))
            {
                compatibility = 1;
                return input;
            }

            if (type == typeof(string) && targetType.IsEnum)
            {
                try
                {
                    compatibility = 0;
                    return Enum.Parse(targetType, (string)input, true);
                }

                catch (Exception) { }
                compatibility = -1;
                return null;
            }

            compatibility = -1;
            return null;
        }
    }
}

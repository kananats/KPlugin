namespace KPlugin.Extension.Internal
{
    using System;

    public static class ObjectExtensionInternal
    {
        public static object ChangeTypeWithCompatibility(this object input, Type targetType, out int compatibility)
        {
            Type inputType = input.GetType();

            if (inputType != typeof(bool) && inputType != typeof(int) && inputType != typeof(float) && inputType != typeof(string)
                || targetType != typeof(bool) && targetType != typeof(int) && targetType != typeof(float) && targetType != typeof(string) && !targetType.IsEnum)
                throw new NotSupportedException();

            if (inputType == targetType && (inputType == typeof(bool) || inputType == typeof(int) || inputType == typeof(float)))
            {
                compatibility = 0;
                return input;
            }

            if (inputType == typeof(bool) && targetType == typeof(string))
            {
                compatibility = 8;
                return ((bool)input).ToString();
            }

            if (inputType == typeof(int) && targetType == typeof(float))
            {
                compatibility = 1;
                return (float)(int)input;
            }

            if (inputType == typeof(int) && targetType == typeof(string))
            {
                compatibility = 4;
                return ((int)input).ToString();
            }

            if (inputType == typeof(int) && targetType.IsEnum)
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

            if (inputType == typeof(float) && targetType == typeof(string))
            {
                compatibility = 8;
                return ((float)input).ToString();
            }

            if (inputType == typeof(string) && targetType == typeof(string))
            {
                compatibility = 1;
                return input;
            }

            if (inputType == typeof(string) && targetType.IsEnum)
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

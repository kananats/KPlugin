namespace KPlugin.Extension.Internal
{
    using System;

    public static class ArrayExtensionInternal
    {
        public static string ToSimpleString(this Array array, bool showType = false)
        {
            int rank = array.Rank;
            int[] lengths = new int[rank];
            for (int i = 0; i < rank; i++)
                lengths[i] = array.GetLength(i);

            int[] indices = new int[rank];
            return array.ToSimpleString(lengths, indices, 0, showType);
        }

        public static string ToSimpleString(this Array array, int[] lengths, int[] indices, int index, bool showType = false)
        {
            string s = "{";
            int i;
            if (index == lengths.Length - 1)
                for (i = 0; i < lengths[index]; i++)
                {
                    indices[index] = i;
                    s = s + array.GetValue(indices).ToSimpleString(showType) + ", ";
                }

            else
                for (i = 0; i < lengths[index]; i++)
                {
                    indices[index] = i;
                    s = s + array.ToSimpleString(lengths, indices, index + 1).ToSimpleString(showType) + ", ";
                }

            return s.Length >= 2 ? s.Substring(0, s.Length - 2) + "}" : "{ }";
        }
    }
}

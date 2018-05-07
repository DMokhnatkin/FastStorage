namespace FastStorage.Expressions.EqualityComparer
{
    static class HashCodeHelper
    {
        public static int CalcHashCode(params object[] values)
        {
            int r = 0;
            foreach (var val in values)
            {
                if (val == null)
                    continue;
                r = CombineHashCodes(r, val.GetHashCode());
            }
            return r;
        }

        public static int CombineHashCodes(params int[] hashCodes)
        {
            int r = 0;
            foreach (var val in hashCodes)
            {
                if (r == 0)
                    r = val;
                else
                    r = (r * 31) ^ val;
            }
            return r;
        }
    }
}

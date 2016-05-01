#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("nxwSHS2fHBcfnxwcHYGdzmlpreHYHtFvBsHcBm6QppSPFhydGd7vOB0GdiNEvpXh3msAwlUA+pFbNR+hHvTDoFyNzl2CoSOmIS1rCQQGW5Itnxw/LRAbFDebVZvqEBwcHBgdHuDGfuLV/ABksTsP6vdAcnWBPVByHNoZZZgqzxefLe1xaNHNx3fxxghIOQ03QTAr0UhR1KGwWscvgAm5PFrwO9nMYjaWTH6+N4CSQDR8lLrGCagZILfX69Y2Y9zjjadEybrroj+iU/YzxQShCUaMmcZXH723pF2bDUL5vxJ0O1jKEhCqpTMn3bhjUOXb4LccjE44G+cWf1Xhgikjphn0T/gXt7TVrrDr7W8jnytXZKCizoNJHaPbz6eLZSqQZh8eHB0c");
        private static int[] order = new int[] { 1,2,8,9,8,5,8,7,10,9,12,13,13,13,14 };
        private static int key = 29;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

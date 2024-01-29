// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("S+5OsvUuBnTRz9/7OpjRL0l26saEl5LmWekgMyw13przgdskmunyAAnU0XaB2jiJjALF7SiHCnaOjhrr8uESZyz1tUrwOH76QG4NyHMmFWpJSjgFncX4TsIvZFa0OfiXcSbyxDu4trmJO7izuzu4uLkoyUjnZ+uc45mCSZY3w1O2710E5U4I0z4HcXEnje89eefTkhDlSyLmoKfPOOp6Hok7uJuJtL+wkz/xP060uLi4vLm6a8ICNwT8D2Zb7qZBsHQhFCFfbzOTiCZ1P14jN/QniIZjISzLW7P05Spe1Dl4Im7+wCrqileVREuFjOlgY6OaVwmDbk0jpUWPM/y8IG7ERgXKadU7bNJKCNcYbv1IM3s3u13XDhMQChP/WxoiSLu6uLm4");
        private static int[] order = new int[] { 10,11,4,13,8,11,9,12,10,13,11,11,13,13,14 };
        private static int key = 185;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

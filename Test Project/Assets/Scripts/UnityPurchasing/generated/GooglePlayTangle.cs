// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("WfORQweZrexumzVcmN7ZsUaUBGCMn2wZUovLNI5GAIQ+EHO2DVhrFPrp7Jgnl15NUkug5I3/pVrkl4x+tBerRRKsNHapZhCDNk0FScUjqXBUIKpHBlwQgL5UlPQp6zo1+/KXHjWQMMyLUHgKr7GhhUTmr1E3CJS47fZYC0EgXUmKWfb4HV9StSXNipud5/w36Em9LciRI3qbMHatQHkPD3eqrwj/pEb38ny7k1b5dAjw8GSVFbx8SXqCcRglkNg/zgpfal8hEU33Rcbl98rBzu1Bj0EwysbGxsLHxB3d5Cl3/RAzXds78U2Cwl4Qujh7NzRGe+O7hjC8URooykeG6Q9YjLpFxsjH90XGzcVFxsbHVrc2mRmV4m1udG2BJWRcNsXExsfG");
        private static int[] order = new int[] { 12,13,11,7,11,10,10,9,11,9,12,12,12,13,14 };
        private static int key = 199;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

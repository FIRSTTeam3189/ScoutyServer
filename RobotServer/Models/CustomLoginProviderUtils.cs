using System.Security.Cryptography;

namespace ScoutingServer.Models {
    class CustomLoginProviderUtils {
        public static byte[] hash(string plaintext, byte[] salt) {
            SHA512 hashFunc = SHA512.Create();
            byte[] plainBytes = System.Text.Encoding.ASCII.GetBytes(plaintext);
            byte[] toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }

        public static byte[] generateSalt() {
            byte[] salt = new byte[256];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public static bool slowEquals(byte[] a, byte[] b) {
            int diff = a.Length ^ b.Length;
            for(int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}

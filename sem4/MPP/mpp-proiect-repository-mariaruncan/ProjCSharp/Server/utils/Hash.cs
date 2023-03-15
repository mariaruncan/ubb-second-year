using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.utils
{
    public class Hash
    {
        public static String hash(String password)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                return hash;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cloudinary
{
    public class CloudinaryURISignature
    {
        /// <summary>
        /// Calculates signature of parameters
        /// </summary>
        /// <param name="parameters">Parameters to sign</param>
        /// <returns>Signature of parameters</returns>
        public static string SignParameters(IDictionary<string, object> parameters, string Cloudinary_SecretKey)
        {
            StringBuilder signBase = new StringBuilder(String.Join("&", parameters
                .Where(pair => pair.Value != null)
                .Select(pair => String.Format("{0}={1}", pair.Key,
                    pair.Value is IEnumerable<string>
                    ? String.Join(",", ((IEnumerable<string>)pair.Value).ToArray())
                    : pair.Value.ToString()))
                .ToArray()));

            signBase.Append(Cloudinary_SecretKey);

            //Debug.WriteLine(signBase.ToString());

            var hash = CloudinaryURISignature.ComputeHash(signBase.ToString());
            StringBuilder sign = new StringBuilder();
            foreach (byte b in hash)
                sign.Append(b.ToString("x2"));

            return sign.ToString();
        }

        /// <summary>
        /// Signs the specified URI part.
        /// </summary>
        /// <param name="uriPart">The URI part.</param>
        /// <returns></returns>
      /*  public static string SignUriPart(string uriPart)
        {
            var hash = CloudinaryURISignature.ComputeHash(uriPart + Cloudinary_SecretKey);
            var sign = Convert.ToBase64String(hash);
            return "s--" + sign.Substring(0, 8).Replace("+", "-").Replace("/", "_") + "--/";
        }*/

        /// <summary>
        /// Computs the SHA1 hash of the input string
        /// </summary>
        /// <param name="s">The input string</param>
        /// <returns>The hash of the input string in bytes</returns>
        private static byte[] ComputeHash(string s)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
        }
    }
}

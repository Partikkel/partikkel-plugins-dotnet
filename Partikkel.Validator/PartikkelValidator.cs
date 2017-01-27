using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Partikkel.Validator
{
    public class PartikkelValidator
    {
        public IDictionary<string, object> Validate(string token, string certPath)
        {
            var bytes = Convert.FromBase64String(token);
            var result = Encoding.UTF8.GetString(bytes);
            var pubKey = LoadCert(certPath);
            var claims = Jose.JWT.Decode<IDictionary<string, object>>(result, pubKey);
            ValidateJwtToken(claims);
            return claims;
        }

        private void ValidateJwtToken(IDictionary<string, object> claims)
        {
            var expiry = claims["exp"];
            var unixEpoc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiryDouble = Convert.ToDouble((Int32)expiry);
            var expiryDate = unixEpoc.AddSeconds(expiryDouble);
            var currentDate = DateTime.UtcNow;
            if (currentDate > expiryDate)
            {
                throw new Exception("JSON token has expired.");
            }
        }

        private RSACryptoServiceProvider LoadCert(string path)
        {
            return (RSACryptoServiceProvider)X509(path).PublicKey.Key;
        }

        private X509Certificate2 X509(string path)
        {
            return new X509Certificate2(path, "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
        }
    }
}

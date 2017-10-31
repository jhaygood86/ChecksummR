using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace ChecksummR
{
    public static class ChecksummR
    {
        public static string GetChecksum(object target)
        {
            var targetType = target.GetType();

            var checksumAttribute = (ChecksumAttribute)targetType.GetCustomAttributes(typeof(ChecksumAttribute), false).SingleOrDefault(x => x is ChecksumAttribute);

            if (checksumAttribute == null)
            {
                throw new ArgumentOutOfRangeException(nameof(target),"Target doesn't have the Checksum attribute");
            }

            var properties = targetType.GetProperties().Where(x => x.GetCustomAttributes<ChecksumPropertyAttribute>().Any()).Where(x => x.GetCustomAttribute<ChecksumPropertyAttribute>().IncludeProperty);

            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                foreach (var property in properties)
                {
                    formatter.Serialize(ms,property);
                }

                var bytes = ms.ToArray();
                byte[] hashedBytes = null;

                switch (checksumAttribute.ChecksumType)
                {
                    case ChecksumType.Md5:
                        MD5 md5 = MD5.Create();
                        hashedBytes = md5.ComputeHash(bytes);
                        break;
                    case ChecksumType.Sha1:
                        SHA1 sha1 = SHA1.Create();
                        hashedBytes = sha1.ComputeHash(bytes);
                        break;
                    case ChecksumType.Sha256:
                        SHA256 sha256 = SHA256.Create();
                        hashedBytes = sha256.ComputeHash(bytes);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown ChecksumType: " + checksumAttribute.ChecksumType);
                }

                string hashString = string.Empty;
                foreach (byte x in hashedBytes)
                {
                    hashString += string.Format("{0:x2}", x);
                }
                return hashString;

            }
        }
    }
}

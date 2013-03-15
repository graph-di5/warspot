using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace WarSpot.MetroClient.Common
{
    public class HashHelper
    {
        /// <summary>
        /// выдаёт последовательность из 32 шестнадцатеричных цифр (md5 хеш от аргумента)
        /// </summary>
        public static string GetMd5Hash(string input)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);

            var data = new byte[hashed.Length];

            CryptographicBuffer.CopyToByteArray(hashed,out data);

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}

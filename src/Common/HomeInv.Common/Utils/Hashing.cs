using System.Security.Cryptography;
using System.Text;

namespace HomeInv.Common.Utils
{
    public static class Hashing
    {
        public static string GetHash(string input)
        {
            //create new instance of sha512
            SHA512 sha1 = SHA512.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(StringToByteArray(input));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hashed string
            return returnValue.ToString();
        }

        private static byte[] StringToByteArray(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
    }
}

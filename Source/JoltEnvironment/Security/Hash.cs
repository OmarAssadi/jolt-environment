/* 
    Jolt Environment
    Copyright (C) 2010 Jolt Environment Team

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Security.Cryptography;
using System.Text;

namespace JoltEnvironment.Security
{
    /// <summary>
    /// Provides method of hashing and comparing hashes.
    /// </summary>
    public static class Hash
    {
        #region Methods
        /// <summary>
        /// Hashes a given text provided by the hash type.
        /// </summary>
        /// <param name="text">The text to encrpyt.</param>
        /// <param name="hashType">The hash type.</param>
        /// <returns>Returns a string.</returns>
        public static string GetHash(string text, HashType hashType)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5:
                    hashString = GetMD5(text);
                    break;
                case HashType.SHA1:
                    hashString = GetSHA1(text);
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(text);
                    break;
                case HashType.SHA384:
                    hashString = GetSHA384(text);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(text);
                    break;
                default:
                    hashString = "Invalid Hash Type";
                    break;
            }
            return hashString;
        }

        /// <summary>
        /// Checks two strings, one being the original text, and second being the encrpyted one.
        /// </summary>
        /// <param name="original">The original text.</param>
        /// <param name="hashString">The hashed text.</param>
        /// <param name="hashType">The hash type to check by.</param>
        /// <returns>Returns a boolean; true if the values match; false if not.</returns>
        public static bool CheckHash(string original, string hashString, HashType hashType)
        {
            string originalHash = GetHash(original, hashType);
            return (originalHash == hashString);
        }

        /// <summary>
        /// Gets the MD5 hashed version of the given text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>Returns a string that's been hashed.</returns>
        private static string GetMD5(string text)
        {
            byte[] message = new MD5CryptoServiceProvider().ComputeHash(
                new UTF8Encoding().GetBytes(text));
            string hex = "";
            foreach (byte b in message)
                hex += b.ToString("x2").ToLower();
            return hex;
        }

        /// <summary>
        /// Gets the SHA1 hashed version of the given text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>Returns a string that's been hashed.</returns>
        private static string GetSHA1(string text)
        {
            byte[] message = new SHA1CryptoServiceProvider().ComputeHash(
                new UTF8Encoding().GetBytes(text));
            string hex = "";
            foreach (byte b in message)
                hex += b.ToString("x2").ToLower();
            return hex;
        }

        /// <summary>
        /// Gets the SHA256 hashed version of the given text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>Returns a string that's been hashed.</returns>
        private static string GetSHA256(string text)
        {
            byte[] message = new SHA256CryptoServiceProvider().ComputeHash(
                new UTF8Encoding().GetBytes(text));
            string hex = "";
            foreach (byte b in message)
                hex += b.ToString("x2").ToLower();
            return hex;
        }

        /// <summary>
        /// Gets the SHA384 hashed version of the given text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>Returns a string that's been hashed.</returns>
        private static string GetSHA384(string text)
        {
            byte[] message = new SHA384CryptoServiceProvider().ComputeHash(
                new UTF8Encoding().GetBytes(text));
            string hex = "";
            foreach (byte b in message)
                hex += b.ToString("x2").ToLower();
            return hex;
        }

        /// <summary>
        /// Gets the SHA512 hashed version of the given text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>Returns a string that's been hashed.</returns>
        private static string GetSHA512(string text)
        {
            byte[] message = new SHA512CryptoServiceProvider().ComputeHash(
                new UTF8Encoding().GetBytes(text));
            string hex = "";
            foreach (byte b in message)
                hex += b.ToString("x2").ToLower();
            return hex;
        }
        #endregion Methods
    }
}

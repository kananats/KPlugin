namespace KPlugin
{
    using UnityEngine;
    using System.IO;
    using System;
    using Extension;
    using Constant.Internal;

    public static class FileManager
    {
        private static string password = "somepassword";

        public static T Read<T>(string fileName, bool encrypted = false)
        {
            T content = default(T);

            string path = Application.persistentDataPath + "/" + fileName;

            try
            {
                string encryptedContent = File.ReadAllText(path);
                string serializedContent = encrypted ? Encryptor.Decrypt(encryptedContent, password) : encryptedContent;

                content = Serializer.Deserialize<T>(serializedContent);

                Debug.Console.Log(StringConstantInternal.readSuccessfully.ReplacedBy(path));
            }
            catch (Exception)
            {
                Debug.Console.Log(StringConstantInternal.readUnsuccessfully.ReplacedBy(path));
            }

            return content;
        }

        public static void Write<T>(string fileName, T content, bool encrypted = false)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            try
            {
                string serializedContent = Serializer.Serialize<T>(content);
                string encryptedContent = encrypted ? Encryptor.Encrypt(serializedContent, password) : serializedContent;

                File.WriteAllText(path, encryptedContent);

                Debug.Console.Log(StringConstantInternal.writeSuccessfully.ReplacedBy(path));
            }
            catch (Exception)
            {
                Debug.Console.Log(StringConstantInternal.writeUnsuccessfully.ReplacedBy(path));
            }
        }
    }
}

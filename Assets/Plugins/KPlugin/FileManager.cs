using System;
using System.IO;
using UnityEngine;

namespace KPlugin
{
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

                StringConstantInternal.readSuccess.ReplacedBy(path).LogConsole();
            }
            catch (Exception)
            {
                StringConstantInternal.readError.ReplacedBy(path).Color(Color.red).LogConsole();
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

                StringConstantInternal.writeSuccess.ReplacedBy(path).LogConsole();
            }
            catch (Exception)
            {
                StringConstantInternal.writeError.ReplacedBy(path).Color(Color.red).LogConsole();
            }
        }
    }
}

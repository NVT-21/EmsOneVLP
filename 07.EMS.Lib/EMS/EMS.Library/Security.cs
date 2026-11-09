using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Library
{
    public static class Security
    {
        public const string PrivateKey = "<RSAKeyValue><Modulus>o66hTWTFLBgRrbQ1jCYaq7/qiQAsZdppWx3s70/mDE1XJDKA7NuiKXO1kHnONvKhXW4UfXiazvluO0M39aQ/v6IUFZp9LBxZJ5WQNniQfkZcaZ8KZMMmF3tumaRwxPmtZbqLU+/FNzUx9kHry+tlX+iQD6lzCeD44RDNTRyrIp0=</Modulus><Exponent>AQAB</Exponent><P>3sQGy52ri4zgz0ImSFPHnAEYBxoqQAisgm9dxhsD17weQUpd0p3o1PHosmigE3iJhAt/3GtuBv+Q0z2/Px3Grw==</P><Q>vBoOQOAA8idEY0Zsf0xnqMcXzVdUUV7HRi10Jm1xEskO4d4RZBecjp7mzPP0brH4XjXmK2EoJi/SwzX+qbu+cw==</Q><DP>iIYw9/dAnD0RxlPvLGKUMgsFk8y4Whg2yZXBLdl+qGf1TuRpzdvTRzae+0Bd4rD77n4xtX/tpcJZGu172Q3Bpw==</DP><DQ>UtauvEdqYzqD1ayoD4/Yfu3fP6FcEtHItNFDu8Cq8Xj2QuaHhisX2X+FaXFujml2VhcqnKQXCGo7SZk35Aw4nQ==</DQ><InverseQ>jvHEvn+QdXgsW4nRWvhQYuMDHCuIu2ylaDTiT0H/awWOgjpd4C5FsbY/glvtbUyxSlQhJoGS+qsiOKNL2VoG+A==</InverseQ><D>aE4pRit4JpAz+9CB6wyBJH+JEsLEiCd1yfW4/guZWq+SX2O7mrSfkvDEfAvS6FsXj3VNo2NOq6RxpVxBdcy0PgFUXWgfsBDFsE6xGJPxnzcvYOCFLPP4O1MBsda1OQdfpyh5ga2YncUrf4quPy5UsBWu3RRBASxJsydbmRD5e/U=</D></RSAKeyValue>";
        public const string PublicKey = "<RSAKeyValue><Modulus>o66hTWTFLBgRrbQ1jCYaq7/qiQAsZdppWx3s70/mDE1XJDKA7NuiKXO1kHnONvKhXW4UfXiazvluO0M39aQ/v6IUFZp9LBxZJ5WQNniQfkZcaZ8KZMMmF3tumaRwxPmtZbqLU+/FNzUx9kHry+tlX+iQD6lzCeD44RDNTRyrIp0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string PASSWORD_SALT = "Ems@2021";
        public const string DEFAULT_PASSWORD = "Ems247@2021";
        public const string DEFAULT_PASSWORD_EMPLOYEE = "Nv@123456";
        public const string DEFAULT_PASSWORD_USER = "Be@123456";
        public const int IV_SIZE = 12;
        public const int TAG_SIZE = 16;
        public const int TAG_LENGHT_BIT = 128;
        #region Sercurity Method
        public static string SHA256(string input)
        {
            string output = string.Empty;
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            output = sb.ToString();
            return output;
        }

        public static string EncodeBase64(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            byte[] textAsBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(string encodedText)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(textAsBytes);
        }

        public static byte[] DecodeBase64Byte(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Token is empty.");

            string base64 = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch(base64.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
                default:
                    throw new FormatException("Invalid Base64Url format.");
            }

            return Convert.FromBase64String(base64);
        }

        public static string RSAEncrypt(string inputString, string publicKeyString)
        {
            try
            {
                int dwKeySize = 0x400;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(dwKeySize);
                provider.FromXmlString(publicKeyString);
                int num2 = dwKeySize / 8;
                byte[] bytes = Encoding.UTF32.GetBytes(inputString);
                int num3 = num2 - 0x2a;
                int length = bytes.Length;
                int num5 = length / num3;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i <= num5; i++)
                {
                    byte[] dst = new byte[((length - (num3 * i)) > num3) ? num3 : (length - (num3 * i))];
                    Buffer.BlockCopy(bytes, num3 * i, dst, 0, dst.Length);
                    byte[] array = provider.Encrypt(dst, true);
                    Array.Reverse(array);
                    builder.Append(Convert.ToBase64String(array));
                }
                provider.Clear();
                provider = null;
                return builder.ToString();
            }
            catch
            {
            }
            return string.Empty;
        }

        public static string RSADecrypt(string inputString, string privateKeyString)
        {
            try
            {
                int dwKeySize = 0x400;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(dwKeySize);
                provider.FromXmlString(privateKeyString);
                int length = (((dwKeySize / 8) % 3) != 0) ? ((((dwKeySize / 8) / 3) * 4) + 4) : (((dwKeySize / 8) / 3) * 4);
                int num3 = inputString.Length / length;
                ArrayList list = new ArrayList();
                for (int i = 0; i < num3; i++)
                {
                    byte[] array = Convert.FromBase64String(inputString.Substring(length * i, length));
                    Array.Reverse(array);
                    list.AddRange(provider.Decrypt(array, true));
                }
                provider.Clear();
                return Encoding.UTF32.GetString(list.ToArray(Type.GetType("System.Byte")) as byte[]);
            }
            catch
            {

            }
            return string.Empty;
        }

        public static string MD5Encrypt(string intputString, string privateKeyString)
        {
            try
            {
                bool useHashing = true;
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(intputString);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(privateKeyString));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(privateKeyString);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch
            {

            }
            return string.Empty;
        }

        public static string MD5Decrypt(string intputString, string privateKeyString)
        {
            try
            {
                bool useHashing = true;
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(intputString);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(privateKeyString));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(privateKeyString);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {

            }
            return string.Empty;
        }

        public static string SignRSA(string data, string xmlPrivateKey)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(1024);
            rsaProvider.FromXmlString(xmlPrivateKey);
            byte[] signature = rsaProvider.SignData(Encoding.UTF8.GetBytes(data), CryptoConfig.CreateFromName("SHA256"));
            return Convert.ToBase64String(signature);
        }

        public static bool VerifyRSA(string data, string signature, string xmlPublicKey)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(1024);
            rsaProvider.FromXmlString(xmlPublicKey);
            bool verify = rsaProvider.VerifyData(Encoding.UTF8.GetBytes(data), CryptoConfig.CreateFromName("SHA256"), Convert.FromBase64String(signature));
            return verify;
        }
        #endregion
        public static string buildSignString(string platform, string clientId, string clientSecret, string device_name, string device_model, string device_id, string date_time, string time_zone)
        {
            return $"{platform} | {clientId} | {clientSecret} | {device_name} | {device_model} | {device_id} | {date_time} | {time_zone}";
        }
        public static string buildSignString2(params string[] args)
        {
            return string.Join(" | ", args);
        }
        public static RSACryptoServiceProvider CreateRsaProviderFromPublicKey(string publicKeyBase64)
        {
            byte[] keyBytes = Convert.FromBase64String(publicKeyBase64);

            // Parse public key bytes với BouncyCastle
            var publicKey = PublicKeyFactory.CreateKey(keyBytes) as RsaKeyParameters;

            var rsaParams = new RSAParameters
            {
                Modulus = publicKey.Modulus.ToByteArrayUnsigned(),
                Exponent = publicKey.Exponent.ToByteArrayUnsigned()
            };

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParams);
            return rsa;
        }
        
        public static string BuildParamSign(string code, string data, string chanel, string signature)
        {
            try
            {
                // Nếu data là JSON thì xử lý lấy các value
                string dataSignPart;
                var obj = JsonConvert.DeserializeObject<JObject>(data);
                if(obj != null)
                {
                    var values = obj.Properties()
                                    .Select(p => p.Value.Type == JTokenType.Null ? "" : p.Value.ToString())
                                    .ToArray();

                    dataSignPart = buildSignString2(values);
                }
                else
                {
                    dataSignPart = buildSignString2(data);
                }

                // Ghép các phần theo thứ tự để tạo param ký
                return buildSignString2(code, data, chanel, signature);
            }
            catch
            {
                // Nếu parse JSON lỗi thì dùng nguyên chuỗi data
                return buildSignString2(code, data, chanel, signature);
            }
        }

        public static string DecryptVIB(string cipherBase64, string keyBase64)
        {
            byte[] key = Convert.FromBase64String(keyBase64);
            byte[] input = Convert.FromBase64String(cipherBase64);

            // IV
            byte[] iv = new byte[IV_SIZE];
            Array.Copy(input, 0, iv, 0, IV_SIZE);

            // Cipher + Tag
            byte[] cipherTag = new byte[input.Length - IV_SIZE];
            Array.Copy(input, IV_SIZE, cipherTag, 0, cipherTag.Length);

            var cipher = new GcmBlockCipher(new AesEngine());

            var parameters = new AeadParameters(
                new KeyParameter(key),
                128,
                iv,
                null
            );

            cipher.Init(false, parameters);

            byte[] output = new byte[cipher.GetOutputSize(cipherTag.Length)];

            int len = cipher.ProcessBytes(cipherTag, 0, cipherTag.Length, output, 0);
            cipher.DoFinal(output, len);

            return Encoding.UTF8.GetString(output).TrimEnd('\0');
        }

        public static XHmReturnPayload DecryptTokenHM(string encryptedToken, string sharedSecretKeyBase64)
        {
            if(string.IsNullOrWhiteSpace(encryptedToken))
                throw new ArgumentException("Encrypted token is required.");

            if(string.IsNullOrWhiteSpace(sharedSecretKeyBase64))
                throw new ArgumentException("Shared secret key is required.");

            byte[] key = Convert.FromBase64String(sharedSecretKeyBase64);

            if(key.Length != 32)
                throw new ArgumentException("Shared secret key must be 32 bytes for AES-256.");

            byte[] tokenBytes = DecodeBase64Byte(encryptedToken);

            if(tokenBytes.Length <= IV_SIZE + TAG_SIZE)
                throw new ArgumentException("Invalid token length.");

            byte[] iv = new byte[IV_SIZE];
            byte[] tag = new byte[TAG_SIZE];
            byte[] cipherText = new byte[tokenBytes.Length - IV_SIZE - TAG_SIZE];

            Buffer.BlockCopy(tokenBytes, 0, iv, 0, IV_SIZE);
            Buffer.BlockCopy(tokenBytes, IV_SIZE, tag, 0, TAG_SIZE);
            Buffer.BlockCopy(tokenBytes, IV_SIZE + TAG_SIZE, cipherText, 0, cipherText.Length);

            byte[] plainTextBytes = DecryptAesGcm(cipherText, key, iv, tag);

            string json = Encoding.UTF8.GetString(plainTextBytes);

            XHmReturnPayload payload = JsonConvert.DeserializeObject<XHmReturnPayload>(json);

            if(payload == null || string.IsNullOrWhiteSpace(payload.OrderId))
                throw new Exception("Invalid payload.");

            ValidateExpiry(payload.Exp);

            return payload;
        }

        private static byte[] DecryptAesGcm(byte[] cipherText, byte[] key, byte[] iv, byte[] tag)
        {
            try
            {
                GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());

                AeadParameters parameters = new AeadParameters(
                    new KeyParameter(key),
                    TAG_LENGHT_BIT,
                    iv,
                    null
                );

                cipher.Init(false, parameters);

                byte[] cipherTextWithTag = new byte[cipherText.Length + tag.Length];

                Buffer.BlockCopy(cipherText, 0, cipherTextWithTag, 0, cipherText.Length);
                Buffer.BlockCopy(tag, 0, cipherTextWithTag, cipherText.Length, tag.Length);

                byte[] output = new byte[cipher.GetOutputSize(cipherTextWithTag.Length)];

                int len = cipher.ProcessBytes(
                    cipherTextWithTag,
                    0,
                    cipherTextWithTag.Length,
                    output,
                    0
                );

                len += cipher.DoFinal(output, len);

                byte[] plainText = new byte[len];
                Buffer.BlockCopy(output, 0, plainText, 0, len);

                return plainText;
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid token, modified token, or incorrect secret key.");
            }
        }

        private static void ValidateExpiry(long exp)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if(now > exp)
                throw new UnauthorizedAccessException("Token has expired.");
        }
        public class XHmReturnPayload
        {
            public string OrderId { get; set; }
            public long Exp { get; set; }
        }
    }
}

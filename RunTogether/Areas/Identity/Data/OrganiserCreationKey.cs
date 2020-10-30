using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RunTogether.Areas.Identity.Data
{
    public class OrganiserCreationKey
    {
        public OrganiserCreationKey(int generatedById, DateTime? expirationDatetime = null)
        {
            GeneratedById = generatedById;
            if (expirationDatetime.HasValue)
            {
                ExpirationDatetime = expirationDatetime.Value;
            }
        }

        [Key]
        public string Key { get; private set; } = CreateRandomKey(36);
        public DateTime ExpirationDatetime { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        public int GeneratedById { get; private set; }
        [Required]
        [ForeignKey("GeneratedById")]
        public ApplicationUser GeneratedBy { get; private set; }


        protected static string CreateRandomKey(int length)
        {
            // Create a new provider which makes cryptographically strong bytes.
            // Then from those providers choose (length) number of characters and return
            // as a string
            RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
            string allowed_characters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = "";

            for (int i = 0; i < length; i++)
            {
                int index = GetRandomByte(rngProvider, allowed_characters.Length);
                result += allowed_characters[index];
            }

            return result;
        }

        protected static byte GetRandomByte(RNGCryptoServiceProvider rngProvider, int maxSize)
        {
            // Using the provider, get a random byte and check if it is considered fair
            // (Ex. a number can be considered fair if the maximum size of the random numbers are
            // not more than the largest multiple of the maxSize that is smaller than the maximum Byte size)
            byte[] randomNumber = new byte[1];
            do
            {
                rngProvider.GetBytes(randomNumber);
            } while (IsFairByte(randomNumber[0], maxSize));

            return randomNumber[0];
        }

        protected static bool IsFairByte(byte number, int arraySize)
        {
            // A number can be considered fair if the maximum size of the random numbers are
            // not more than the largest multiple of the maxSize that is smaller than the maximum Byte size
            int validIntFactor = Byte.MaxValue / arraySize;
            return number < validIntFactor * arraySize;
        }
    }
}


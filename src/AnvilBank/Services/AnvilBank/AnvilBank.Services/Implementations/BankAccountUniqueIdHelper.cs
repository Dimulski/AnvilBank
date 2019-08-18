using AnvilBank.Common.Configuration;
using AnvilBank.Services.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace AnvilBank.Services.Implementations
{
    public class BankAccountUniqueIdHelper : IBankAccountUniqueIdHelper
    {
        private readonly BankConfiguration bankConfiguration;
        private Random random;

        public BankAccountUniqueIdHelper(IOptions<BankConfiguration> bankConfigurationOptions)
        {
            this.bankConfiguration = bankConfigurationOptions.Value;
        }

        public string GenerateAccountUniqueId()
        {
            if (this.random == null)
            {
                this.random = new Random();
            }

            char[] generated = new char[12];

            for (int i = 0; i < 3; i++)
            {
                generated[i] = this.bankConfiguration.UniqueIdentifier[i];
            }

            for (int i = 0; i < 8; i++)
            {
                generated[i + 4] = (char)('0' + this.random.Next(10));
            }

            generated[3] = GenerateCheckCharacter(generated);

            return string.Join("", generated);
        }

        public bool IsUniqueIdValid(string id)
        {
            throw new NotImplementedException();
        }

        private static char GenerateCheckCharacter(IReadOnlyList<char> uniqueId)
        {
            int sum = 0;
            for (int i = 0; i < uniqueId.Count; i++)
            {
                if (i == 3)
                {
                    continue;
                }

                sum += (i + 1) * uniqueId[i];
            }

            return (char)('A' + sum % 26);
        }
    }
}

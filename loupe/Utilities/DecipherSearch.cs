using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace loupe.Utilities
{
    public static class DecipherSearch
    {
        public static string Decipher(string text)
        {
            int blockNumber;
            if (int.TryParse(text, out blockNumber))
            {
                return $"/Block?{text}";
            }

            Utilities.Web3Client w3client = new Utilities.Web3Client();

            switch (text.Length)
            {
                case 42:
                    return $"/Address?{text}";

                case 66:
                    return $"/{w3client.DeciperTypeOfHash(text).GetAwaiter().GetResult()}?{text}";

                default:
                    return "/Index";
            }
        }
    }
}

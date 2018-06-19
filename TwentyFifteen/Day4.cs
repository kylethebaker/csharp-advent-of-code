using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

// Santa needs help mining some AdventCoins (very similar to bitcoins) to use
// as gifts for all the economically forward-thinking little girls and boys.
//
// To do this, he needs to find MD5 hashes which, in hexadecimal, start with at
// least five zeroes. The input to the MD5 hash is some secret key (your puzzle
// input, given below) followed by a number in decimal. To mine AdventCoins,
// you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3,
// ...) that produces such a hash.
//
// For example:
//
//   - If your secret key is abcdef, the answer is 609043, because the MD5 hash
//     of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the
//     lowest such number to do so.
//   - If your secret key is pqrstuv, the lowest number it combines with to
//     make an MD5 hash starting with five zeroes is 1048970; that is, the MD5
//     hash of pqrstuv1048970 looks like 000006136ef....
namespace AdventOfCode.TwentyFifteen
{
    class Day4
    {
        private const string SECRET = "ckczppom";

        // Runs the challenge
        public static void Run()
        {
            Console.WriteLine($"Part1: {validCoins(5).First()}");
            Console.WriteLine($"Part2: {validCoins(6).First()}");
        }

        // Yields valid coins for a given difficulty
        private static IEnumerable<(string coin, int counter)> validCoins(int leadingZeroes)
        {
            var counter = 0;
            var validPrefix = new String('0', leadingZeroes);

            while (true)
            {
                var coin = md5Hash($"{SECRET}{counter}");
                if (coin.StartsWith(validPrefix)) {
                    yield return (coin, counter);
                }
                counter++;
            }
        }

        // Returns a hex encoded md5 hash of a string input
        private static string md5Hash(string input)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}

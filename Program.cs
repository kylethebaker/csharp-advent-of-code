using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Program
    {
        // Maps a year/day slug to the specific challenge runner
        private static Dictionary<string, Action> runners =
            new Dictionary<string, Action> {
                {"2015.1", AdventOfCode.TwentyFifteen.Day1.Run},
                {"2015.2", AdventOfCode.TwentyFifteen.Day2.Run},
                {"2015.3", AdventOfCode.TwentyFifteen.Day3.Run}
            };

        static void Main(string[] args)
        {
            // Need a single argument with the year/day slug
            if (args.Length != 2) {
                Console.WriteLine("Usage: ./AdventOfCode <year>.<day>");
                Environment.Exit(-1);
            }

            var challengeSlug = args[1];

            // Need a single argument with the year/day slug
            if (!runners.ContainsKey(challengeSlug)) {
                Console.WriteLine($"No challenge associated with slug `{challengeSlug}`");
                Environment.Exit(-1);
            }

            Console.WriteLine($"## Running challenge `{challengeSlug}`");
            runners[challengeSlug]();
        }
    }
}

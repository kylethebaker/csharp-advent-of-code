using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.TwentyFifteen
{
    class Day5
    {
        private const string inputFile = "./TwentyFifteen/Day5.input";

        private static HashSet<char> vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };

        // Holds the bad character combos for Part1
        private static HashSet<(char, char)> badSlugs => new HashSet<(char, char)> {
            ('a', 'b'), ('c', 'd'), ('p', 'q'), ('x', 'y')
        };

        // Runs the challenge
        public static void Run()
        {
            try
            {
                var input = File.ReadLines(inputFile);
                Console.WriteLine($"Part1: {Part1(input)}");
                Console.WriteLine($"Part2: {Part2(input)}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Missing input file for Day5");
            }
        }

        // Santa needs help figuring out which strings in his text file are naughty or
        // nice.
        //
        // A nice string is one with all of the following properties:
        //
        //   - It contains at least three vowels (aeiou only), like aei, xazegov, or
        //     aeiouaeiouaeiou.
        //   - It contains at least one letter that appears twice in a row, like xx,
        //     abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
        //   - It does not contain the strings ab, cd, pq, or xy, even if they are part
        //     of one of the other requirements.
        //
        // For example:
        //
        //   - ugknbfddgicrmopn is nice because it has at least three vowels
        //     (u...i...o...), a double letter (...dd...), and none of the disallowed
        //     substrings.  aaa is nice because it has at least three vowels and a
        //     double letter, even though the letters used by different rules overlap.
        //   - jchzalrnumimnmhp is naughty because it has no double letter.
        //   - haegwjzuvuyypxyu is naughty because it contains the string xy.
        //   - dvszwmarrgswjxmb is naughty because it contains only one vowel.
        //
        // How many strings are nice?
        private static int Part1(IEnumerable<string> input)
        {
            return input.Select(isNice).Where(x => x == true).Count();
        }

        // Checks if a string meets all of the 'niceness' constraints for Part 1
        private static bool isNice(string word)
        {
          var initialState = (
              vowelCount: 0,
              hasDoubleLetter: false,
              previousLetter: ' ',
              hasBadSlug: false
          );

          var finalState = word.ToCharArray().Aggregate(initialState, (state, letter) => {
              // We need at least three vowels
              if (vowels.Contains(letter))
              {
                  state.vowelCount++;
              }

              // We need two of the same letter back to back
              if (state.previousLetter == letter)
              {
                  state.hasDoubleLetter = true;
              }

              // We can't have any character combos that are considered 'bad'
              if (badSlugs.Contains((state.previousLetter, letter)))
              {
                  state.hasBadSlug = true;
              }

              // We need to keep track of the previous letter for our checks
              state.previousLetter = letter;

              return state;
          });

          return (finalState.vowelCount >= 3 && finalState.hasDoubleLetter && !finalState.hasBadSlug);
        }

        // Now, a nice string is one with all of the following properties:
        //
        //   - It contains a pair of any two letters that appears at least
        //     twice in the string without overlapping, like xyxy (xy) or
        //     aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
        //   - It contains at least one letter which repeats with exactly one
        //     letter between them, like xyx, abcdefeghi (efe), or even aaa.
        //
        // For example:
        //
        //   - qjhvhtzxzqqjkmpb is nice because is has a pair that appears
        //     twice (qj) and a letter that repeats with exactly one letter
        //     between them (zxz).
        //   - xxyxx is nice because it has a pair that appears twice and a
        //     letter that repeats with one between, even though the letters
        //     used by each rule overlap.
        //   - uurcxstgmygtbstg is naughty because it has a pair (tg) but no
        //     repeat with a single letter between them.
        //   - ieodomkazucvgmuy is naughty because it has a repeating letter
        //     with one between (odo), but no pair that appears twice.
        //
        // How many strings are nice under these new rules?
        private static int Part2(IEnumerable<string> input)
        {
            return input.Select(isNicer).Where(x => x == true).Count();
        }

        // Checks if a string meets all of the 'niceness' constraints for Part 2
        private static bool isNicer(string word)
        {
            var initialState = (
                pairs: new Dictionary<(char, char), int>(),
                oneLetterBack: ' ',
                twoLettersBack: ' ',
                threeLettersBack: ' ',
                hasGappedRepeat: false
            );

            var finalState = word.ToCharArray().Aggregate(initialState, (state, letter) => {
                // If we're on the first letter then just seed the state
                if (state.oneLetterBack == ' ')
                {
                    state.oneLetterBack = letter;
                    return state;
                }

                // If we're on the second letter then we're still seeding, but we
                // still need to add the pair so we don't miss it
                if (state.twoLettersBack == ' ')
                {
                    state.pairs.Add((state.oneLetterBack, letter), 1);
                    state.twoLettersBack = state.oneLetterBack;
                    state.oneLetterBack = letter;
                    return state;
                }

                // See if we have a repeat with a gap
                if (letter == state.twoLettersBack)
                {
                    state.hasGappedRepeat = true;
                }

                // Add the pair to the tally, unless it is a overlap
                if (!(letter == state.oneLetterBack && letter == state.twoLettersBack))
                {
                    var pair = (state.oneLetterBack, letter);

                    if (state.pairs.ContainsKey(pair))
                    {
                        state.pairs[pair] = state.pairs[pair] + 1;
                    }
                    else
                    {
                        state.pairs.Add(pair, 1);
                    }
                }

                // Shift around our stored previous values
                state.twoLettersBack = state.oneLetterBack;
                state.oneLetterBack = letter;

                return state;
            });

            // If we had a non-overlapping pair that appeared more than once then
            // that is good
            var hasTwoPairs = finalState.pairs.Where(kv => kv.Value >= 2).Any();

            return hasTwoPairs && finalState.hasGappedRepeat;
        }
    }
}

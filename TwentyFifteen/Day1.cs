using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// Santa is trying to deliver presents in a large apartment building, but he
// can't find the right floor - the directions he got are a little confusing.
// He starts on the ground floor (floor 0) and then follows the instructions
// one character at a time.
//
// An opening parenthesis, (, means he should go up one floor, and a closing
// parenthesis, ), means he should go down one floor.
//
// For example:
//
//    - (()) and ()() both result in floor 0.
//    - ((( and (()(()( both result in floor 3.
//    - ))((((( also results in floor 3.
//    - ()) and ))( both result in floor -1 (the first basement level).
//    - ))) and )())()) both result in floor -3.
//
namespace AdventOfCode.TwentyFifteen
{
    class Day1
    {
        private static readonly string inputFile = "./TwentyFifteen/Day1.input";

        public static void Run()
        {
            try {
                var input = File.ReadAllText(inputFile).ToCharArray();
                Console.WriteLine($"Part1: {Part1(input)}");
                Console.WriteLine($"Part2: {Part2(input)}");
            } catch (FileNotFoundException) {
                Console.WriteLine("Missing input file for Day1");
            }
        }

        // Find the final floor if all directions are followed
        private static int Part1(IEnumerable<char> input)
        {
            return input
                .Aggregate(0, (floor, direction) =>
                    (direction == '(')
                        ? (floor + 1)
                        : (floor - 1)
                );
        }

        // Find the position of the character where santa first enters the
        // basement (negative floor)
        private static int Part2(IEnumerable<char> input)
        {
            var currentFloor = 0;
            var floorsTraversed = 0;

            foreach (var direction in input) {
                floorsTraversed++;

                if (direction == '(') {
                    currentFloor++;
                } else {
                    currentFloor--;
                }

                if (currentFloor == -1) {
                    return floorsTraversed;
                }
            }

            return 0;
        }
    }
}

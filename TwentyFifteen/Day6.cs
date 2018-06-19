using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.TwentyFifteen
{
    class Day6
    {
        private const string inputFile = "./TwentyFifteen/Day6.input";

        enum Switch { On, Off, Toggle };

        // Runs the challenge
        public static void Run()
        {
            try
            {
                var input = parseInput(File.ReadLines(inputFile));
                Console.WriteLine($"Part1: {Part1(input)}");
                Console.WriteLine($"Part2: {Part2(input)}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Missing input file for Day6");
            }
        }

        // Creates an empty 1000x1000 grid
        private static Dictionary<(int x, int y), int> createEmptyGrid()
        {
            var grid = new Dictionary<(int x, int y), int>();

            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    grid[(x, y)] = 0;
                }
            }

            return grid;
        }

        // Parses each line from the input to a a tuple containing Switch
        // direction, start point, and stop point
        private static IEnumerable<(Switch, (int x, int y), (int x, int y))> parseInput(IEnumerable<string> rawInput)
        {
            return rawInput.Select(line => {
                var pieces = line.Split(' ');

                // Toggle lines will only have 4 items and have different
                // offsets for the start/stop coords
                if (pieces.Length == 4)
                {
                    return (Switch.Toggle, parsePoint(pieces[1]), parsePoint(pieces[3]));
                }

                var switchDirection = (pieces[1] == "on") ? Switch.On : Switch.Off;

                return (switchDirection, parsePoint(pieces[2]), parsePoint(pieces[4]));
            });
        }

        // Parses a point from the string form "x,y" into an x/y tuple
        private static (int x, int y) parsePoint(string pointSlug)
        {
            var split = pointSlug.Split(',').Select(int.Parse).ToArray();
            return (x: split[0], y: split[1]);
        }

        // Lights in your grid are numbered from 0 to 999 in each direction;
        // the lights at each corner are at 0,0, 0,999, 999,999, and 999,0. The
        // instructions include whether to turn on, turn off, or toggle various
        // inclusive ranges given as coordinate pairs. Each coordinate pair
        // represents opposite corners of a rectangle, inclusive; a coordinate
        // pair like 0,0 through 2,2 therefore refers to 9 lights in a 3x3
        // square. The lights all start turned off.
        //
        // To defeat your neighbors this year, all you have to do is set up
        // your lights by doing the instructions Santa sent you in order.
        //
        // For example:
        //
        //   - turn on 0,0 through 999,999 would turn on (or leave on) every
        //     light.
        //   - toggle 0,0 through 999,0 would toggle the first line of 1000
        //     lights, turning off the ones that were on, and turning on the
        //     ones that were off.
        //   - turn off 499,499 through 500,500 would turn off (or leave off)
        //     the middle four lights.
        //
        // After following the instructions, how many lights are lit?
        private static int Part1(IEnumerable<(Switch, (int x, int y), (int x, int y))> input)
        {
            var finalGrid = input.Aggregate(createEmptyGrid(), (grid, instruction) => {
                var (switchDirection, startPoint, stopPoint) = instruction;
                for (var x = startPoint.x; x <= stopPoint.x; x++)
                {
                    for (var y = startPoint.y; y <= stopPoint.y; y++)
                    {
                        switch (switchDirection)
                        {
                            case Switch.On:
                                grid[(x, y)] = 1;
                                break;
                            case Switch.Off:
                                grid[(x, y)] = 0;
                                break;
                            case Switch.Toggle:
                                grid[(x, y)] = (grid[(x, y)] == 0) ? 1 : 0;
                                break;
                        }
                    }
                }
                return grid;
            });

            return finalGrid.Where(kv => kv.Value == 1).Count();
        }

        // The light grid you bought actually has individual brightness
        // controls; each light can have a brightness of zero or more. The
        // lights all start at zero.
        //
        // The phrase turn on actually means that you should increase the
        // brightness of those lights by 1.
        //
        // The phrase turn off actually means that you should decrease the
        // brightness of those lights by 1, to a minimum of zero.
        //
        // The phrase toggle actually means that you should increase the
        // brightness of those lights by 2.
        //
        // What is the total brightness of all lights combined after following
        // Santa's instructions?
        //
        // For example:
        //
        //   - turn on 0,0 through 0,0 would increase the total brightness by
        //     1.
        //   - toggle 0,0 through 999,999 would increase the total brightness
        //     by 2000000.
        private static int Part2(IEnumerable<(Switch, (int x, int y), (int x, int y))> input)
        {
            return 42;
        }
    }
}

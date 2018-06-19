using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFifteen
{
    class Day3
    {
        private static readonly string inputFile = "./TwentyFifteen/Day3.input";

        private const char NORTH = '^';
        private const char EAST = '>';
        private const char SOUTH = 'v';
        private const char WEST = '<';

        // Runs the challenge
        public static void Run()
        {
            try {
                var input = File.ReadAllText(inputFile).Trim().ToCharArray();
                Console.WriteLine($"Part1: {Part1(input)}");
                Console.WriteLine($"Part2: {Part2(input)}");
            } catch (FileNotFoundException) {
                Console.WriteLine("Missing input file for Day3");
            }
        }

        // Santa is delivering presents to an infinite two-dimensional grid of
        // houses.
        //
        // He begins by delivering a present to the house at his starting
        // location, and then an elf at the North Pole calls him via radio and
        // tells him where to move next. Moves are always exactly one house to
        // the north (^), south (v), east (>), or west (<). After each move, he
        // delivers another present to the house at his new location.
        //
        // However, the elf back at the north pole has had a little too much
        // eggnog, and so his directions are a little off, and Santa ends up
        // visiting some houses more than once. How many houses receive at
        // least one present?
        //
        // For example:
        //
        //   - > delivers presents to 2 houses: one at the starting location,
        //     and one to the east.
        //   - ^>v< delivers presents to 4 houses in a
        //     square, including twice to the house at his starting/ending
        //     location.
        //   - ^v^v^v^v^v delivers a bunch of presents to some
        //     very lucky children at only 2 houses.
        private static int Part1(IEnumerable<char> input)
        {
            var initialState = (
                visited: new HashSet<(int, int)>(),
                uniqueHouses: 0,
                currentPosition: (x: 0, y: 0)
            );

            var finalState = input.Aggregate(initialState, (state, direction) => {

                // Update the current position
                switch (direction)
                {
                  case NORTH:
                    state.currentPosition.y++;
                    break;
                  case SOUTH:
                    state.currentPosition.y--;
                    break;
                  case EAST:
                    state.currentPosition.x++;
                    break;
                  case WEST:
                    state.currentPosition.x--;
                    break;
                  default:
                    throw new ArgumentException($"Invalid direction character in input. Got `{direction}` but expected one of: '^', 'v', '<', '>'");
                }

                // If this is the first time we've visited this position then
                // tally it and add the position to our visited set so we know
                // not to count it if we see it again
                if (!state.visited.Contains(state.currentPosition)) {
                  state.uniqueHouses++;
                  state.visited.Add(state.currentPosition);
                }

                return state;
            });

            return finalState.uniqueHouses;
        }

        // The next year, to speed up the process, Santa creates a robot
        // version of himself, Robo-Santa, to deliver presents with him.
        //
        // Santa and Robo-Santa start at the same location (delivering two
        // presents to the same starting house), then take turns moving based
        // on instructions from the elf, who is eggnoggedly reading from the
        // same script as the previous year.
        //
        // This year, how many houses receive at least one present?
        //
        // For example:
        //
        //   - ^v delivers presents to 3 houses, because Santa goes north, and
        //     then Robo-Santa goes south.
        //   - ^>v< now delivers presents to 3 houses, and Santa and Robo-Santa
        //     end up back where they started.
        //   - ^v^v^v^v^v now delivers presents to 11 houses, with Santa going
        //     one direction and Robo-Santa going the other.
        private static int Part2(IEnumerable<char> input)
        {
            var initialState = (
                visited: new HashSet<(int, int)>(),
                uniqueHouses: 0,
                positions: new (int x, int y)[] { (x: 0, y: 0), (x: 0, y: 0) },
                turnsTaken: 0
            );

            var finalState = input.Aggregate(initialState, (state, direction) => {
                var whosTurn = state.turnsTaken % 2;

                // Update the current position
                switch (direction)
                {
                  case NORTH:
                    state.positions[whosTurn].y++;
                    break;
                  case SOUTH:
                    state.positions[whosTurn].y--;
                    break;
                  case EAST:
                    state.positions[whosTurn].x++;
                    break;
                  case WEST:
                    state.positions[whosTurn].x--;
                    break;
                  default:
                    throw new ArgumentException($"Invalid direction character in input. Got `{direction}` but expected one of: '^', 'v', '<', '>'");
                }

                // If this is the first time we've visited this position then
                // tally it and add the position to our visited set so we know
                // not to count it if we see it again
                if (!state.visited.Contains(state.positions[whosTurn])) {
                  state.uniqueHouses++;
                  state.visited.Add(state.positions[whosTurn]);
                }

                state.turnsTaken++;

                return state;
            });

            return finalState.uniqueHouses;
        }
    }
}

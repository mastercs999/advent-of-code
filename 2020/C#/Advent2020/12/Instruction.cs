using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._12
{
    public record Instruction
    {
        public char Direction { get; init; }
        public int Value { get; init; }


        public Position Execute(Position position)
        { 
            static char NewDirection(char direction, int degrees)
            {
                char[] directions = new char[] { 'N', 'E', 'S', 'W' };

                return directions[(Array.IndexOf(directions, direction) + degrees / 90 + 4) % 4];
            }

            return Direction switch
            {
                'N' => position with { Y = position.Y + Value },
                'S' => position with { Y = position.Y - Value },
                'E' => position with { X = position.X + Value },
                'W' => position with { X = position.X - Value },
                'L' => position with { Direction = NewDirection(position.Direction, -Value) },
                'R' => position with { Direction = NewDirection(position.Direction, Value) },
                'F' => position with
                {
                    X = position.X + (position.Direction is 'W' ? -Value : position.Direction is 'E' ? Value : 0),
                    Y = position.Y + (position.Direction is 'S' ? -Value : position.Direction is 'N' ? Value : 0)
                },
                _ => throw new InvalidOperationException()
            };
        }
        public (Position, Position) Execute2(Position waypointPosition, Position shipPosition)
        {
            static Position Rotate(Position position, char direction, int degrees)
            {
                for (int i = 0; i < degrees / 90; ++i)
                    position = position with { X = position.Y * (direction == 'L' ? -1 : 1), Y = position.X * (direction == 'L' ? 1 : -1) };
                return position;
            }

            return Direction switch
            {
                'N' => (waypointPosition with { Y = waypointPosition.Y + Value }, shipPosition),
                'S' => (waypointPosition with { Y = waypointPosition.Y - Value }, shipPosition),
                'E' => (waypointPosition with { X = waypointPosition.X + Value }, shipPosition),
                'W' => (waypointPosition with { X = waypointPosition.X - Value }, shipPosition),
                'L' => (Rotate(waypointPosition, 'L', Value), shipPosition),
                'R' => (Rotate(waypointPosition, 'R', Value), shipPosition),
                'F' => (waypointPosition, shipPosition with
                {
                    X = shipPosition.X + Value * waypointPosition.X,
                    Y = shipPosition.Y + Value * waypointPosition.Y
                }),
                _ => throw new InvalidOperationException()
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._13
{
    public static class Challenge13
    {
        public static void Run()
        {
            // Load input
            (Track[,] tracks, Cart[] carts) = LoadTracks(Path.Combine("13", "input"));

            // Task 1
            // Move until someone crashes
            Track crashTrack = RunUntilCrash(carts, tracks);

            Console.WriteLine(crashTrack.X + "," + crashTrack.Y);

            // Task 2
            (tracks, carts) = LoadTracks(Path.Combine("13", "input"));
            Cart lastCart = LastCartStanding(carts, tracks);

            Console.WriteLine(lastCart.Track.X + "," + lastCart.Track.Y);
        }

        private static (Track[,], Cart[]) LoadTracks(string path)
        {
            // Init array
            string[] lines = File.ReadAllLines(path);
            int xLength = lines.Max(x => x.Length);
            int yLength= lines.Length;

            Track[,] tracks = new Track[yLength, xLength];
            List<Cart> carts = new List<Cart>();

            // Parse track
            for (int x = 0; x < tracks.GetLength(1); ++x)
                for (int y = 0; y < tracks.GetLength(0); ++y)
                {
                    Track track = ParseTrack(lines[y][x], x, y);
                    Cart cart = ParseCart(lines[y][x], track);
                    if (cart != null)
                        carts.Add(cart);

                    tracks[y, x] = track;
                }

            return (tracks, carts.ToArray());
        }

        private static Track ParseTrack(char c, int x, int y)
        {
            switch (c)
            {
                case '|':
                case '^':
                case 'v': return new Track(x, y, TrackType.Vertical);
                case '-':
                case '<':
                case '>': return new Track(x, y, TrackType.Horizontal);
                case '/': return new Track(x, y, TrackType.CurveRight);
                case '\\': return new Track(x, y, TrackType.CurveLeft);
                case '+': return new Track(x, y, TrackType.Intersection);
            }

            return null;
        }
        private static Cart ParseCart(char c, Track track)
        {
            switch (c)
            {
                case '^': return new Cart(Direction.Top, track);
                case 'v': return new Cart(Direction.Bottom, track);
                case '<': return new Cart(Direction.Left, track);
                case '>': return new Cart(Direction.Right, track);
            }

            return null;
        }

        private static void Move(Cart cart, Track[,] tracks)
        {
            switch (cart.Track.TrackType)
            {
                case TrackType.Horizontal:
                    cart.Track = GoHorizontal(cart, tracks);
                    break;
                case TrackType.Vertical:
                    cart.Track = GoVertical(cart, tracks);
                    break;
                case TrackType.CurveRight:
                    cart.Track = cart.Direction == Direction.Left || cart.Direction == Direction.Right ? TurnLeft(cart, tracks) : TurnRight(cart, tracks);
                    break;
                case TrackType.CurveLeft:
                    cart.Track = cart.Direction == Direction.Left || cart.Direction == Direction.Right ? TurnRight(cart, tracks) : TurnLeft(cart, tracks);
                    break;
                case TrackType.Intersection:
                    switch (cart.NextTurn)
                    {
                        case Turn.Left:
                            cart.Track = TurnLeft(cart, tracks);
                            break;
                        case Turn.Straight:
                            cart.Track = cart.Direction == Direction.Left || cart.Direction == Direction.Right ? GoHorizontal(cart, tracks) : GoVertical(cart, tracks);
                            break;
                        case Turn.Right:
                            cart.Track = TurnRight(cart, tracks);
                            break;
                    }
                    cart.NextTurn = (Turn)(((int)cart.NextTurn + 1) % 3);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Track GoHorizontal(Cart cart, Track[,] tracks)
        {
            return tracks[cart.Track.Y, cart.Track.X + (cart.Direction == Direction.Left ? -1 : 1)];
        }
        private static Track GoVertical(Cart cart, Track[,] tracks)
        {
            return tracks[cart.Track.Y + (cart.Direction == Direction.Top ? -1 : 1), cart.Track.X];
        }
        private static Track TurnRight(Cart cart, Track[,] tracks)
        {
            switch (cart.Direction)
            {
                case Direction.Top:
                    cart.Direction = Direction.Right;
                    return tracks[cart.Track.Y, cart.Track.X + 1];
                case Direction.Right:
                    cart.Direction = Direction.Bottom;
                    return tracks[cart.Track.Y + 1, cart.Track.X];
                case Direction.Bottom:
                    cart.Direction = Direction.Left;
                    return tracks[cart.Track.Y, cart.Track.X - 1];
                case Direction.Left:
                    cart.Direction = Direction.Top;
                    return tracks[cart.Track.Y - 1, cart.Track.X];
            }

            throw new InvalidOperationException();
        }
        private static Track TurnLeft(Cart cart, Track[,] tracks)
        {
            switch (cart.Direction)
            {
                case Direction.Top:
                    cart.Direction = Direction.Left;
                    return tracks[cart.Track.Y, cart.Track.X - 1];
                case Direction.Right:
                    cart.Direction = Direction.Top;
                    return tracks[cart.Track.Y - 1, cart.Track.X];
                case Direction.Bottom:
                    cart.Direction = Direction.Right;
                    return tracks[cart.Track.Y, cart.Track.X + 1];
                case Direction.Left:
                    cart.Direction = Direction.Bottom;
                    return tracks[cart.Track.Y + 1, cart.Track.X];
            }

            throw new InvalidOperationException();
        }

        private static Track RunUntilCrash(Cart[] carts, Track[,] tracks)
        {
            while (true)
            {
                foreach (Cart cart in carts.OrderBy(x => x.Track.Y).ThenBy(x => x.Track.X))
                {
                    Move(cart, tracks);

                    Track crashTrack = carts.GroupBy(x => x.Track).Where(x => x.Count() > 1).SingleOrDefault()?.Key;
                    if (crashTrack != null)
                        return crashTrack;
                }
            }
        }

        private static Cart LastCartStanding(Cart[] carts, Track[,] tracks)
        {
            List<Cart> cartsLeft = carts.ToList();

            while (cartsLeft.Count != 1)
            {
                List<Cart> toRemove = new List<Cart>();

                foreach (Cart cart in cartsLeft.OrderBy(x => x.Track.Y).ThenBy(x => x.Track.X))
                {
                    Move(cart, tracks);

                    toRemove.AddRange(cartsLeft.GroupBy(x => x.Track).Where(x => x.Count() > 1).SelectMany(x => x).ToList());
                }

                foreach (Cart crashed in toRemove)
                    cartsLeft.Remove(crashed);
            }

            return cartsLeft.Single();
        }
    }
}

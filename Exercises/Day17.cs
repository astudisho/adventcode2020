using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCode.Exercises
{
    public static class Day17
    {
        private static List<Cube> InitializeCubes()
        {
            var i = -1;
            var j = 0;
            var cubes = TestData.Split("\r\n")
                                .SelectMany(x =>
                                {
                                    i++;
                                    j = 0;
                                    return x.ToCharArray();
                                }, (a, b) =>
                                 {
                                     return new Cube
                                     {
                                         PosX = i,
                                         PosY = j++,
                                         PosZ = 0,
                                         IsActive = b == '#'
                                     };
                                 });

            return cubes.ToList();

        }

        private static bool LastCycleWasActive(int i, int j, int k, List<Cube> cubes) =>
            // Find if in last iteration a cube in the same position exists and if were active, if null false.
            cubes.FirstOrDefault(x => x.PosX == i &&
                                      x.PosY == j &&
                                      x.PosZ == k)?.IsActive ?? false;
        private static bool IsAdjacentPosition(int selfPosition, int position, (int, int) cycleBounds)
        {
            if(selfPosition == position)
            {
                return true;
            }
            if(position >= selfPosition -1 && position <= selfPosition + 1)
            {
                return true;
            }
            //else if(position == cycleBounds.Item1)
            //{
            //    return selfPosition == cycleBounds.Item2;
            //}
            //else if(position == cycleBounds.Item2)
            //{
            //    return selfPosition == cycleBounds.Item1;
            //}
            else
            {
                return false;
            }
        }

        private static bool IsSamePosition(int i, int j, int k, Cube x) => (x.PosX == i && x.PosY == j && x.PosZ == k);

        private static List<Cube> GetAllNeighbors(int i, int j, int k, List<Cube> cubes)
        {
            // Fetch all the neighbors.
            var neighbors = cubes.Where(x =>
            {
                var isNeighbor =
                // At most 1 position in X
                IsAdjacentPosition(i, x.PosX, Cycle.SearchSpaceX) &&
                IsAdjacentPosition(j, x.PosY, Cycle.SearchSpaceY) &&
                IsAdjacentPosition(k, x.PosZ, Cycle.SearchSpaceZ)                
                // And not itself.
                && !IsSamePosition(i, j, k, x);

                return isNeighbor;
            }).ToList();

            return neighbors;
        }

        private static bool ShouldBeActive(bool lastCycleWasActive, int activeNeighborsCount)
        {
            var shouldBeActive = false;
            // Determine if already exists and apply logic of active or inactive.
            if (lastCycleWasActive)
            {
                shouldBeActive = activeNeighborsCount == 2 || activeNeighborsCount == 3;
            }
            else
            {
                shouldBeActive = activeNeighborsCount == 3;
            }
            return shouldBeActive;
        }

        internal static int Exercise1()
        {
            var result = 0;
            var cubes = InitializeCubes();
            do
            {
                var newCubes = new List<Cube>();

                for (int i = Cycle.SearchSpaceX.Item1; i <= Cycle.SearchSpaceX.Item2; i++)
                {
                    for (int j = Cycle.SearchSpaceY.Item1; j <= Cycle.SearchSpaceY.Item2; j++)
                    {
                        for (int k = Cycle.SearchSpaceZ.Item1; k <= Cycle.SearchSpaceZ.Item2; k++)
                        {
                            // Find if in last iteration a cube in the same position exists and if were active, if null false.
                            var lastCycleCubeWasActive = LastCycleWasActive(i, j, k, cubes);

                            // Add to next iteration cubes.
                            var cube = new Cube
                            {
                                PosX = i,
                                PosY = j,
                                PosZ = k,
                                IsActive = lastCycleCubeWasActive
                            };
                            newCubes.Add(cube);

                            // Fetch all the neighbors.
                            //var neighbors = GetAllNeighbors(i, j, k, cubes);

                            //var activeNeighborsCount = neighbors.Where(x => x.IsActive).Count();
                            
                            //var shouldBeActive = ShouldBeActive(lastCycleCubeWasActive, activeNeighborsCount);

                            
                        }
                    }
                }
                for (int i = Cycle.SearchSpaceX.Item1; i <= Cycle.SearchSpaceX.Item2; i++)
                {
                    for (int j = Cycle.SearchSpaceY.Item1; j <= Cycle.SearchSpaceY.Item2; j++)
                    {
                        for (int k = Cycle.SearchSpaceZ.Item1; k <= Cycle.SearchSpaceZ.Item2; k++)
                        {
                            // Find if in last iteration a cube in the same position exists and if were active, if null false.
                            var lastCycleCubeWasActive = LastCycleWasActive(i, j, k, newCubes);

                            var neighbors = GetAllNeighbors(i, j, k, newCubes);

                            var activeNeighborsCount = neighbors.Where(x => x.IsActive).Count();

                            var shouldBeActive = ShouldBeActive(lastCycleCubeWasActive, activeNeighborsCount);
                        }
                    }
                }
                            // Run 1 iteration.
                            cubes = newCubes;
                Cycle.Iterate();
                result = cubes.Where(x => x.IsActive).Count();
            } while (Cycle.Iteration < 6); // While Cycle iteration < 6.

            return result;

        }


        private class Cube
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public int PosZ { get; set; }
            public bool IsActive { get; set; }

            public override string ToString()
            {
                return $"x:{PosX} y:{PosY} z:{PosZ} active:{(IsActive ? 1 : 0)}";
            }
        }

        private static class Cycle
        {
            internal static (int, int) SearchSpaceX { get; set; } = (0, 2);
            internal static (int, int) SearchSpaceY { get; set; } = (0, 2);
            internal static (int, int) SearchSpaceZ { get; set; } = (-1, 1);
            internal static int Iteration { get; set; } = 0;
            internal static void Iterate()
            {
                SearchSpaceX = (SearchSpaceX.Item1 - 1, SearchSpaceX.Item2 + 1);
                SearchSpaceY = (SearchSpaceY.Item1 - 1, SearchSpaceY.Item2 + 1);
                SearchSpaceZ = (SearchSpaceZ.Item1 - 1, SearchSpaceZ.Item2 + 1);
                Iteration++;
            }
        }
        private static string TestData =
            @".#.
..#
###";
    }
}

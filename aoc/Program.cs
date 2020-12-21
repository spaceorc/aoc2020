using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    public class Program
    {
        static void Main()
        {
            var lines = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt")
                .Select(long.Parse)
                .ToArray();
            
            long res = 0;
            Console.Out.WriteLine(res);
        }
        
        static void Main_21_2()
        {
            var lines = File.ReadAllLines("day21.txt")
                .Select(x => x.Split("(contains").Select(s => s.Split(new[]{" ", ",", ")"}, StringSplitOptions.RemoveEmptyEntries)).ToArray())
                .Select(x => (ingredients: x[0], allergens: x[1]))
                .ToArray();

            var allAllergens = lines.SelectMany(x => x.allergens).Distinct().ToArray();
            var allIngredients = lines.SelectMany(x => x.ingredients).Distinct().ToArray();
            
            var matches = new Dictionary<string, string[]>();
            foreach (var allergen in allAllergens)
            {
                var ingredients = lines
                    .Where(l => l.allergens.Contains(allergen))
                    .Select(x => x.ingredients)
                    .Aggregate(allIngredients, (a, b) => a.Intersect(b).ToArray());

                matches[allergen] = ingredients;
            }

            var resultMatches = new Dictionary<string, string>();

            while (true)
            {
                var found = false;
                foreach (var (allergen, ingredients) in matches)
                {
                    if (ingredients.Length == 1)
                    {
                        found = true;
                        resultMatches[allergen] = ingredients[0];
                    }
                }

                if (!found)
                    break;

                matches = matches
                    .ToDictionary(
                        x => x.Key, 
                        x => x.Value.Except(resultMatches.Values).ToArray());
            }

            Console.Out.WriteLine(string.Join(",", resultMatches.OrderBy(x => x.Key).Select(x => x.Value)));
        }

        static void Main_21_1()
        {
            var lines = File.ReadAllLines("day21.txt")
                .Select(x => x.Split("(contains").Select(s => s.Split(new[]{" ", ",", ")"}, StringSplitOptions.RemoveEmptyEntries)).ToArray())
                .Select(x => (ingredients: x[0], allergens: x[1]))
                .ToArray();

            var allAllergens = lines.SelectMany(x => x.allergens).Distinct().ToArray();
            var allIngredients = lines.SelectMany(x => x.ingredients).Distinct().ToArray();

            var dangerousIngredients = new HashSet<string>();
            foreach (var allergen in allAllergens)
            {
                var ingredients = lines
                    .Where(l => l.allergens.Contains(allergen))
                    .Select(x => x.ingredients)
                    .Aggregate(allIngredients, (a, b) => a.Intersect(b).ToArray());
                
                dangerousIngredients.UnionWith(ingredients);
            }

            Console.Out.WriteLine(lines
                .SelectMany(x => x.ingredients)
                .Count(x => !dangerousIngredients.Contains(x)));
        }
        
        static void Main_20_2()
        {
            // output of part 1
            var image = @"
.............#....#..#.....#..####.##.#.......#.....#.#.....###..............#.....##.#...#...##
..#..#......#.###...#.#..##..#....#.##....##......#.#.#..#..##.#........#......#...#.#.#......##
...#.#...#....#.#..#.......#.#..###.###.#####.#...#..#...##...#....##...#.#..###.#............##
.##..#...#..##....####......##.#.#....#.###...#####........#.#..##.......##......#.#...#..#..#..
......#........#...#.#..#..###.#......#..#.#...............##.#.#...#.#####.##.###..#...#...#...
.#........#.......#..#..#..##..##......##....##..##.#.....##.#........##.....#......#......##...
.##.#..#..#..#...........#....###..#.##..#..###....#......##....#.....#...#..........#.##.......
.##..##.......#.#.............#..#........#.######......#....#..##...##....###.#.........#..####
.##......#.###....#..#...###.#..###........######...#...#..###....##.#.#..#..#.....#.##..#.##...
#.#.....##..##.....#....##......#........#.#.....#.#....#.#..#...#...#............##..#...#..#..
..#..#.##....###...#..#...#...##......#.###.......#........#..#.....##.#.....#....#...###.##....
...#.....#.#..##.##..........#.##...#..#..........#..#..#..##.#......##.......#..#....##..#.....
...#...##..#...#..#.........#.##.#.....#...##.#.#..#..#.#.#..#.....##.#........#...#...#........
...##.....##...###...........##......#......#...##...#..#..###...#...##...#....#...#.##..#....#.
#.#...#...........##...##...#...#..#.##....##.#...#...#.....#....#.........#....#.##.#.#...##..#
.#.#....#.##.#.#..##...###...........#..#..#........#.#..###..#.#...#..#.##.#..#.....#.##.......
....#.##...#...###.#.###.#..#.#....#....#.##..#.......#..#.###....#..#......###...#...#.#..#...#
..#.##.#.#####.#.##.#.##....#.##.....................#.....#.#.......##...####.#...##..#........
###.#....#....##...##.......#.##.#......#.##......#.......#.#........##.....##......###..#..#...
.#...#.....#.........##....#..#.#...###..#..#.#..##..##...#.......#####..#.#...#.#.........###..
#..#......#.#.#...##........#.......#....#..#..#..#..#.......#...#...#.#.#..###.#..#.....#..####
#....##...#........#.....................#.#.#.#.....#....##..#..##.##...#....#..##..###.#.##...
....##...##..#.....##...###.#.#.##.......#.............#..##..#......##....#.#.............##.#.
......##...##..#.#..........#..#.#.#..........#.#...##...#...#.#..#.###.....#..#...#.###.#..##..
#.......#.#.#......#.........#....#.#...#..#..####......##...#.........#.#..#...#.#..#..##.#.#.#
##........#....#..........#.##....#..#...##.#.#.#..#.#....####..#.....#.#.##.#..#......#..##..#.
####.........##..#......##...#..#.............##.......#...#..#.....#........##......#......#...
..#.#....#...#..##.##.#..#...#.#......#..#.........###.#.......##.........##..#.##.#.#.#..###...
..###.##.......#..#....#.##.##.........#.#........#....#..#..........##..###.##.##..####...#.#..
.......#..#..#..#..#....#.......##....#......#.#..##.#....#.#..###..#.#...###...##.###.##.#.##.#
.#............#......#..#.#.....#...#..##.###..##.......#.##.#.##..##...###.#.#.....##.##..#...#
#..##.....#...##.#..#......#..##..#....##....#..#.#..#........##.##..##....#....#...#......#.##.
.....###..####.##.#.....#.####.##.#.#..........#...#.##.#.....##.....#....##..#.......##..#.#...
.#..#...#...#.##..#.....#....#.###.#........####..#.#.....#...###......#....##.....##...#.###.##
..#..#.#.......#...#..#....#...#...#.#.#.##...####.......##.##..#...#.##...#..#.#....#.#...###..
....###.....#...#....###....#.##.#...#.......####........#.#..#.......##....#.#..##.##...##...#.
..#....#...#..#...........#.........#..#.....#.##..#.#...###...#...#....##....##....###......#..
.##.#..##....#.###..#.#....#.###..............#.#.#.#.#.##.#..###....#...#...##.....#.####.###..
#.#...#.....#.#.##....#....#.##.#..#....##.....#........#..##.##.#.......##......#....#..#.##...
##....##.......##....###.....#....#.......#..............#......##.#.##.............##..........
.#..#.######.#.##.##..#.....#..##.##.#...##....#..#...#.......#...#..#.#.......#....##.....#.#..
....#.###.##...###.#.##..##.......#.......##...##..#..#.#...#.#........#..#..........#...#....##
#..#..#.#.....#.......#####.##..#..#..#..#...#..##.#.....#...#.##.#.####.....#...####....##.#...
###.#.....##.......####..#..#.###...#.#..##......#.####..#.....#...##..##.#.#.##.........#.....#
#...#....#.#..#.#.#..#.#.....#...#...........##.......#.#.....#..#.....##......#.#..#...#.......
#.....#...#.#.#...#...#.####.#.............##...#.....#....#.....#..#.##......##....#.##....#...
...#...#.....#.##..#..#..##.#.#........##...####.....##.#.#..#.#..........#.....#.#.##.....#....
#...####....#.##..#..#..##...#..##..........#....#.#....#.....#..##..#.#...#.#####..#..###...#..
.##...#.#.#...#..##..##..........#.....#...#..#.....##.#.#.###.#...####.....#.##.##...#..#..#...
.#.##...........#....#.#.#..#.##....#.#.##.##....#...#.#.#.#...##....###..#..#.#..#..#..#####..#
#...##.......#.##.##.#.......##....##.##........#....#####.......#...####.#..##.#....#..#...###.
......#.......#.#.#.#.#....#.##.#.#...#....#.##.#....#....#.##........#.....#...#...#....##.#...
.#.#...#..#....#......#...####..#....#....###..#.#.##...#.##...#......#..#..#..#.....######...#.
.#....##.......#....##.......#.#............##.#...#.#.##....#..#....#..#.#.....#.......#..#....
..#.###.....####.#.......##.#.#.#.#.#.##...##.......#....#......#.....#....#.#..###......#.##...
......#..#............##..#.##....#..#.###..#...#.....#..#..#.....#..#.#.......##...#...###..#..
#.#....#..#.....###..###..#.##....#..###.#.......#.#.......#.#.....#...#.....#.#..#.....##......
.....##.#....###....#.#...#...#...#...#..#.#.##....#..#..#....#.##.#..#..#..##.#..........##....
#.#..###.......##.##..###.###..##....#.##...#..#.#..#.#.#......#....##.#.......#.#..#...#....#..
...#####........#.#.##.#...............#.#.###..#.###......#..#..........#.....#..#.....#.#.....
#...#.#..#........#.....#.......#.#.#.#...##..#........#.##.#.....#..##.##......#....#.###...#..
#....#.......##...#..#..............##............#.#...#...........####......#.#.##.#.#....#..#
.#.....##.###..#..#.#...#...###..#...###.#.....##..#...........#........#.....##.........##..##.
#.#....##...........#...#........##.#.#.#..#...#.#..#.######..............#.####..#.#...###..##.
......##..#.....#....#...#.##.....####..#...#...#.#...###.#.#...###........##.....#.#.......##..
#..#.###..##..#....#......#..###.#...##.#..##....##..###.#.#...#..##....##.###.#....#........#..
#..#.#........##..#..###..#.#.#.........###........####..#.#..#.#.#......#.#..#.#......##..##..#
.#......##....#.##.##...........#.#..#.........#...##......##.#.##....#...##....##.#...#.#....#.
#....###.##..#.#.####.......##...#...##.#...#.........#...###...#..##.#.#...##.#...#..#.#...#...
###..#.#.#.......##.###.....#..#....#.........#.#.#.##..#..#.##.#..#....##.....#....#....##...#.
....#..#......#.#..#....#.#.##.#.#.............#.#....#...#.##.###......#.#...#.....#....##.....
.#..#.#.#..#.#..##..#..#####....##...#.#.....#...#...###..##..#...#........#.#.#.#..#.....#.....
.#....#..#.####.#....#.#..#..#.......##...##.......#.#...#.#..#...##.##.#.##..#######...##..####
..#..##..###.##.#...#...###..#......#...#.....#.#.#...###.#.....##........#................#....
...#..##.....#...#........#....#..#...####....##.##.##.#...#...#.##.#..........##............#..
.......#.#.....#.##...#....#..#..#..#...#.......##.#.#..##.#..#........#.....#.....#........#..#
.......#....#..#...#..##.#.#..#.#..#.#.###.......#..###.#.####...#....##.##..##.#.........#.....
...#..#.#...##....#..##...##......##.....#.#...#...#.##....#..#...#..###.###....#....#.#.#....#.
##...##...#..##.....#.....#.....#..###......#........#..#.....#..##...#.##.#...#...#.#.##.###...
##.#..#....##.#..##.#.#..#....#......#.#.....#..#####...#.#......#..#.###.#.##.#.#......####...#
......#......##.#..#.#.......#...#..#.#.#..##........#.#...#..#.##..#.##......#.......#.#...#.#.
..###..###.#..#....#..#.##..##.......#..##....#....#.#..#..#..#...#....#.#.#.#.#.#....###....###
.#.#.#.#...###.....#....##..#..#..#.#..#..#.....#.#...#...#......#.#..#...#.#.#..........#.#....
.#..#..###...#.........#..#..........#.....#.#...#....##..#.......#........#.#.####.#..#...#..#.
..##..#....#..#.......#...#....#.#..#....#...###.#.##..#.#.##...##.#..........#..#......##.#....
...##....#......#.#......#........#....##.#.#......#..##..#.#.......#...##.....#...#........#.#.
#......#.....#.....#.#....##.#...#.#....#....#...#..#....###..#.......#............##.##.##..###
.....#....#.#...##.#..#....##.......##..###.#....#..##.#..#...#..##....#.#..#....#...##......#.#
#....#....#.#.#.#.#....#.##.....#..#...#.....#..###.##.##....##.##.....#..##...#....#....#......
...............#.#.##.....#....#..#......##.#..............##....#..##...#.#.#..##......#.#...##
..#.#....#..........##...#......##.#..#.......####....#..####.....###.#.......#.##.....#.#......
#...#..###..#..#.....#.##.....##...#.#.#.#..##.#..#..#..#.........#........###..##...#....#..#..
..#..#...#.#.##.....#..#.#.....#.....####.#.#...#.......##..#.#.##.......#...#.........#.#....#.
....##.#.......#.....#.#..##...#..#......#..#..#...#.#.#.....##......#.....##.#......#.##.##.##.
#.#.....#........##...#..###.##.##.....#...........#.#...#.......#..#......#.#................#.
.....#........#.....##.#....##.###.#......#####.#.#....##...#....#.........##.###..#............"
                .Trim('\n')
                .Split('\n');

            var patternImage = @"
                  # 
#    ##    ##    ###
 #  #  #  #  #  #   
".Trim('\n').Split('\n');

            var size = image.Length;
            var original = new Map<bool>(image.Length);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                original[new V(x, y)] = image[y][x] == '#';

            var pattern = new Map<bool>(patternImage[0].Length, patternImage.Length);
            for (int y = 0; y < patternImage.Length; y++)
            for (int x = 0; x < patternImage[0].Length; x++)
                pattern[new V(x, y)] = patternImage[y][x] == '#';


            for (int orientation = 0; orientation < 8; orientation++)
            {
                var map = GetMapOrientation(original, orientation);
                var count = 0;
                while (FindAndClearPattern(map))
                    count++;

                if (count > 0)
                {
                    Console.Out.WriteLine(count);
                    Console.Out.WriteLine(map.data.Count(x => x));
                    return;
                }
            }

            bool FindAndClearPattern(Map<bool> map)
            {
                for (int y = 0; y <= size - pattern.sizeY; y++)
                for (int x = 0; x <= size - pattern.sizeX; x++)
                {
                    var v = new V(x, y);
                    var found = true;
                    for (int py = 0; py < pattern.sizeY && found; py++)
                    for (int px = 0; px < pattern.sizeX; px++)
                    {
                        var pv = new V(px, py);
                        if (pattern[pv] && !map[v + pv])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        for (int py = 0; py < pattern.sizeY && found; py++)
                        for (int px = 0; px < pattern.sizeX; px++)
                        {
                            var pv = new V(px, py);
                            if (pattern[pv])
                                map[v + pv] = false;
                        }

                        return true;
                    }
                }

                return false;
            }

            Map<bool> GetMapOrientation(Map<bool> map, int variantIndex)
            {
                for (int i = 0; i < variantIndex % 4; i++)
                    map = RotateMapOnce(map);
                if (variantIndex >= 4)
                    map = FlipMapOnce(map);
                return map;
            }


            Map<bool> RotateMapOnce(Map<bool> map)
            {
                var res = new Map<bool>(size);
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    res[new V(x, y)] = map[new V(size - y - 1, x)];

                return res;
            }

            Map<bool> FlipMapOnce(Map<bool> map)
            {
                var res = new Map<bool>(size);
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    res[new V(x, y)] = map[new V(y, x)];

                return res;
            }
        }

        static void Main_20_1()
        {
            var tiles = File.ReadAllText("day20.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseTile)
                .ToDictionary(x => x.id,
                    x => x.variants.Select((variant, variantIndex) => (variant, variantIndex,
                        tileImage: GetTileImageVariant(x.tileImage, variantIndex))).ToArray());

            var complements = tiles.Values
                .SelectMany(x => x.SelectMany(t => t.variant))
                .Distinct()
                .ToDictionary(x => x, GetComplement);

            var tilesByComplement = tiles
                .SelectMany(x => x.Value.SelectMany(t => t.variant.Select(complement => (id: x.Key, complement))))
                .ToLookup(x => x.complement, x => x.id)
                .ToDictionary(x => x.Key, x => x.Distinct().ToArray());

            var size = (int) Math.Sqrt(tiles.Count);
            var image = new Map<(long id, int[] variant, int variantIndex)>(size);

            var freeTiles = tiles.Select(x => x.Key).ToHashSet();

            /*     0
             *   +---+
             * 3 |   | 1
             *   +---+
             *     2
             */
            var shifts = new[] {new V(0, -1), new V(1, 0), new V(0, 1), new V(-1, 0)};
            if (!TryPlace(new V(0, 0)))
                throw new Exception("WTF???");

            for (int y = 0; y < size; y++)
            for (int yy = 0; yy < 8; yy++)
            {
                for (int x = 0; x < size; x++)
                {
                    var tileImage = tiles[image[new V(x, y)].id][image[new V(x, y)].variantIndex].tileImage;
                    for (int xx = 0; xx < 8; xx++)
                        Console.Out.Write(tileImage[new V(xx, yy)] ? "#" : ".");
                }

                Console.Out.WriteLine();
            }


            Console.Out.WriteLine(image[new V(0, 0)].id
                                  * image[new V(size - 1, 0)].id
                                  * image[new V(0, size - 1)].id
                                  * image[new V(size - 1, size - 1)].id);

            bool TryPlace(V v)
            {
                if (v.Y >= size)
                    return true;
                foreach (var tile in GetFittingTiles(v))
                {
                    freeTiles.Remove(tile.id);
                    image[v] = tile;
                    var next = v.X < size - 1 ? v + shifts[1] : new V(0, v.Y + 1);
                    if (TryPlace(next))
                        return true;
                    freeTiles.Add(tile.id);
                }

                return false;
            }

            List<(long id, int[] variant, int variantIndex)> GetFittingTiles(V v)
            {
                var topComplements = v.Y > 0 ? tilesByComplement[image[v + shifts[0]].variant[2]] : null;
                var leftComplements = v.X > 0 ? tilesByComplement[image[v + shifts[3]].variant[1]] : null;

                var candidates = topComplements == null && leftComplements == null ? freeTiles
                    : topComplements == null ? leftComplements.Where(freeTiles.Contains)
                    : leftComplements == null ? topComplements.Where(freeTiles.Contains)
                    : leftComplements.Intersect(topComplements).Intersect(freeTiles);

                var result = new List<(long id, int[] variant, int variantIndex)>();
                foreach (var id in candidates)
                {
                    var variants = tiles[id];
                    foreach (var (variant, variantIndex, _) in variants)
                    {
                        if (v.Y > 0 && variant[0] != complements[image[v + shifts[0]].variant[2]])
                            continue;
                        if (v.X > 0 && variant[3] != complements[image[v + shifts[3]].variant[1]])
                            continue;

                        result.Add((id, variant, variantIndex));
                    }
                }

                return result;
            }

            static int GetComplement(int value)
            {
                var result = 0;
                var bit = 1;
                for (int i = 0; i < 10; i++, bit <<= 1)
                {
                    result <<= 1;
                    if ((value & bit) != 0)
                        result |= 1;
                }

                return result;
            }

            Map<bool> RotateTileImageOnce(Map<bool> tileImage)
            {
                var res = new Map<bool>(8);
                for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    res[new V(x, y)] = tileImage[new V(8 - y - 1, x)];

                return res;
            }

            Map<bool> FlipTileImageOnce(Map<bool> tileImage)
            {
                var res = new Map<bool>(8);
                for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    res[new V(x, y)] = tileImage[new V(y, x)];

                return res;
            }

            Map<bool> GetTileImageVariant(Map<bool> tileImage, int variantIndex)
            {
                for (int i = 0; i < variantIndex % 4; i++)
                    tileImage = RotateTileImageOnce(tileImage);
                if (variantIndex >= 4)
                    tileImage = FlipTileImageOnce(tileImage);
                return tileImage;
            }

            static (long id, List<int[]> variants, Map<bool> tileImage) ParseTile(string tile)
            {
                var lines = tile.Split('\n');
                var id = long.Parse(lines[0].Split(new[] {":", " "}, StringSplitOptions.RemoveEmptyEntries)[1]);
                lines = lines.Skip(1).ToArray();

                var tileImage = new Map<bool>(8);
                for (int y = 1; y <= 8; y++)
                for (int x = 1; x <= 8; x++)
                    tileImage[new V(x - 1, y - 1)] = lines[y][x] == '#';

                var variant = new[]
                {
                    Convert.ToInt32(lines[0].Replace(".", "0").Replace("#", "1"), 2),
                    Convert.ToInt32(
                        string.Join("", lines.Select(line => line.Last())).Replace(".", "0").Replace("#", "1"), 2),
                    GetComplement(Convert.ToInt32(lines.Last().Replace(".", "0").Replace("#", "1"), 2)),
                    GetComplement(Convert.ToInt32(
                        string.Join("", lines.Select(line => line.First())).Replace(".", "0").Replace("#", "1"), 2))
                };

                var variants = new List<int[]>
                {
                    variant,
                    Rotate(variant),
                    Rotate(Rotate(variant)),
                    Rotate(Rotate(Rotate(variant))),
                };

                variants.AddRange(variants.ToArray().Select(v => v.Reverse().Select(GetComplement).ToArray()));

                int[] Rotate(int[] src)
                {
                    var res = new int[src.Length];
                    for (int i = 0; i < src.Length - 1; i++)
                        res[i] = src[i + 1];
                    res[^1] = src[0];
                    return res;
                }

                return (id, variants, tileImage);
            }
        }

        static void Main_19_2()
        {
            var input = File.ReadAllText("day19.txt")
                .Split("\n\n");

            var sources = input[0].Split('\n')
                .Select(x => x.Split(new[] {": "}, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[0], x => x[1].Replace("\"", ""));

            // 0: 8 11
            // 8: 42 | 42 8
            // 11: 42 31 | 42 11 31
            sources["8"] = "42 | 42 8";
            sources["11"] = "42 31 | 42 11 31";

            var rules = new Dictionary<string, string>();
            foreach (var (key, _) in sources)
            {
                if (key == "0" || key == "8" || key == "11")
                    continue;
                Parse(key);
            }
            
            // old way - count manually
            var re42 = new Regex($"^(?<g42>{rules["42"]})+", RegexOptions.Compiled);
            var re31 = new Regex($"(?<g31>{rules["31"]})+$", RegexOptions.Compiled);
            var messages = input[1].Split('\n');
            long res = 0;
            foreach (var message in messages)
            {
                var m42 = re42.Matches(message).SingleOrDefault();
                var m31 = re31.Matches(message).SingleOrDefault();
                if (m42 == null || m31 == null)
                    continue;

                var c42 = m42.Groups["g42"].Captures.Count;
                var c31 = m31.Groups["g31"].Captures.Count;

                if (m42.Value + m31.Value == message && c42 > c31)
                    res++;
            }

            Console.Out.WriteLine(res);

            // new way - use balancing groups
            var re = new Regex($"^({rules["42"]})+(?'open'{rules["42"]})+(?'close-open'{rules["31"]})+$", RegexOptions.Compiled);
            res = 0;
            foreach (var message in messages)
            {
                if (re.IsMatch(message))
                    res++;
            }
            Console.Out.WriteLine(res);

            string Parse(string key)
            {
                if (rules.TryGetValue(key, out var rule))
                    return rule;

                var source = sources[key];
                if (char.IsLetter(source[0]))
                {
                    rules[key] = source;
                    return source;
                }

                var parts = source.Split(" | ");
                rule = string.Join("|", parts.Select(part => $"{string.Join("", part.Split(" ").Select(Parse))}"));
                if (parts.Length > 1)
                    rule = $"({rule})";

                rules[key] = rule;
                return rule;
            }
        }

        static void Main_19_1()
        {
            var input = File.ReadAllText("day19.txt")
                .Split("\n\n");

            var sources = input[0].Split('\n')
                .Select(x => x.Split(new[] {": "}, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[0], x => x[1].Replace("\"", ""));

            var rules = new Dictionary<string, string>();
            foreach (var (key, _) in sources)
                Parse(key);

            var re = new Regex($"^{rules["0"]}$", RegexOptions.Compiled);
            var messages = input[1].Split('\n');

            Console.Out.WriteLine((long) messages.Count(message => re.IsMatch(message)));

            string Parse(string key)
            {
                if (rules.TryGetValue(key, out var rule))
                    return rule;

                var source = sources[key];
                if (char.IsLetter(source[0]))
                {
                    rules[key] = source;
                    return source;
                }

                var parts = source.Split(" | ");
                rule = string.Join("|", parts.Select(part => $"{string.Join("", part.Split(" ").Select(Parse))}"));
                if (parts.Length > 1)
                    rule = $"({rule})";


                rules[key] = rule;
                return rule;
            }
        }

        static void Main_18_2()
        {
            // see alternative solution in class `N`
            var lines = File.ReadAllLines("day18.txt");
            Console.Out.WriteLine(lines.Sum(Eval));


            static long Eval(string expression)
            {
                const int mul = -1;
                const int add = -2;

                var stack = new Stack<List<long>>();
                var cur = new List<long>();
                foreach (var c in expression)
                {
                    switch (c)
                    {
                        case ' ':
                            continue;

                        case '(':
                            stack.Push(cur);
                            cur = new List<long>();
                            break;

                        case ')':
                            EvalMul();
                            var rr = cur.Single();
                            cur = stack.Pop();
                            cur.Add(rr);
                            break;

                        case '*':
                            EvalMul();
                            cur.Add(mul);
                            break;

                        case '+':
                            cur.Add(add);
                            break;

                        default:
                            if (!char.IsDigit(c))
                                throw new Exception($"Invalid char '{c}' in expression '{expression}'");

                            cur.Add(c - '0');
                            break;
                    }

                    EvalAdd();
                }

                EvalMul();
                return cur.Single();

                void EvalAdd()
                {
                    if (cur.Count >= 3)
                    {
                        if (cur[^2] == add)
                        {
                            var r = cur[^1] + cur[^3];
                            cur.RemoveAt(cur.Count - 1);
                            cur.RemoveAt(cur.Count - 1);
                            cur.RemoveAt(cur.Count - 1);
                            cur.Add(r);
                        }
                    }
                }

                void EvalMul()
                {
                    if (cur.Count >= 3)
                    {
                        if (cur[^2] == mul)
                        {
                            var r = cur[^1] * cur[^3];
                            cur.RemoveAt(cur.Count - 1);
                            cur.RemoveAt(cur.Count - 1);
                            cur.RemoveAt(cur.Count - 1);
                            cur.Add(r);
                        }
                    }
                }
            }
        }

        static void Main_18_1()
        {
            // see alternative solution in class `N`
            var lines = File.ReadAllLines("day18.txt");
            Console.Out.WriteLine(lines.Sum(Eval));

            static long Eval(string expression)
            {
                const int mul = -1;
                const int add = -2;

                var stack = new Stack<List<long>>();
                var expr = new List<long>();
                foreach (var c in expression)
                {
                    switch (c)
                    {
                        case ' ':
                            continue;

                        case '(':
                            stack.Push(expr);
                            expr = new List<long>();
                            break;

                        case ')':
                            var res = expr.Single();
                            expr = stack.Pop();
                            expr.Add(res);
                            break;

                        case '*':
                            expr.Add(mul);
                            break;

                        case '+':
                            expr.Add(add);
                            break;

                        default:
                            if (!char.IsDigit(c))
                                throw new Exception($"Invalid char '{c}' in expression '{expression}'");
                            expr.Add(c - '0');
                            break;
                    }


                    if (expr.Count == 3)
                    {
                        var r = expr[1] switch
                        {
                            mul => expr[0] * expr[2],
                            add => expr[0] + expr[2],
                            _ => throw new Exception($"Invalid operation '{expr[1]}")
                        };
                        expr.Clear();
                        expr.Add(r);
                    }
                }

                return expr.Single();
            }
        }

        static void Main_17_2()
        {
            var lines = File.ReadAllLines("day17.txt");
            var sizeX = lines[0].Length + 14;
            var sizeY = lines.Length + 14;
            var sizeZ = 1 + 14;
            var sizeW = 1 + 14;
            var dv = new V4(7, 7, 7, 7);
            var map = new Map4<bool>(sizeX, sizeY, sizeZ, sizeW);
            var map2 = new Map4<bool>(sizeX, sizeY, sizeZ, sizeW);

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[0].Length; x++)
            {
                var v = new V4(x, y, 0, 0) + dv;
                if (lines[y][x] == '#')
                    map[v] = true;
            }

            for (var i = 0; i < 6; i++)
                Sim();

            Console.Out.WriteLine(map.data.Count(x => x));

            void Sim()
            {
                map2.Clear();
                foreach (var v in map.RangeNoBorders())
                {
                    var count = v.Nears().Count(n => map[n]);
                    if (map[v])
                        map2[v] = count == 2 || count == 3;
                    else
                        map2[v] = count == 3;
                }

                var tmp = map;
                map = map2;
                map2 = tmp;
            }
        }

        static void Main_17_1()
        {
            var lines = File.ReadAllLines("day17.txt");
            var sizeX = lines[0].Length + 14;
            var sizeY = lines.Length + 14;
            var sizeZ = 1 + 14;
            var dv = new V3(7, 7, 7);

            var map = new Map3<bool>(sizeX, sizeY, sizeZ);
            var map2 = new Map3<bool>(sizeX, sizeY, sizeZ);

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[0].Length; x++)
            {
                var v = new V3(x, y, 0) + dv;
                if (lines[y][x] == '#')
                    map[v] = true;
            }

            for (var i = 0; i < 6; i++)
                Sim();
            Console.Out.WriteLine(map.data.Count(x => x));

            void Sim()
            {
                map2.Clear();
                foreach (var v in map.RangeNoBorders())
                {
                    var count = v.Nears().Count(n => map[n]);
                    if (map[v])
                        map2[v] = count == 2 || count == 3;
                    else
                        map2[v] = count == 3;
                }

                var tmp = map;
                map = map2;
                map2 = tmp;
            }
        }

        static void Main_16_2()
        {
            var groups = File.ReadAllText("day16.txt")
                .Split("\n\n")
                .ToArray();

            var rules = groups[0]
                .Split('\n')
                .Select(x =>
                {
                    var strings = x.Split(new[] {": ", " or ", "-"}, StringSplitOptions.RemoveEmptyEntries);
                    return strings
                        .Skip(1)
                        .Select(long.Parse)
                        .ToArray();
                })
                .ToArray();

            var myTickets = groups[1]
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(x => x.Split(',').Select(long.Parse).ToArray())
                .ToArray();

            var tickets = groups[2]
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(x => x.Split(',').Select(long.Parse).ToArray())
                .ToArray();

            tickets = myTickets.Concat(tickets.Where(IsGoodTicket)).ToArray();
            var ticketSize = tickets[0].Length;

            var matches = new List<int[]>();
            for (var i = 0; i < ticketSize; i++)
            {
                var matchingRuleIndices = rules
                    .Select((rule, ruleIndex) => (rule, ruleIndex))
                    .Where(x => tickets.All(t => IsMatch(t[i], x.rule)))
                    .Select(r => r.ruleIndex).ToArray();

                matches.Add(matchingRuleIndices);
            }

            var matchingRuleIndex = Enumerable.Repeat(-1, ticketSize).ToArray();

            while (true)
            {
                var found = false;
                for (var i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Length == 1)
                    {
                        found = true;
                        matchingRuleIndex[i] = matches[i][0];
                    }
                }

                if (!found)
                    break;

                matches = matches.Select(m => m.Except(matchingRuleIndex).ToArray()).ToList();
            }

            var values = Enumerable
                .Range(0, 6)
                .Select(ruleIndex => myTickets[0][Array.IndexOf(matchingRuleIndex, ruleIndex)])
                .ToArray();
            Console.Out.WriteLine(values.Aggregate(1L, (a, b) => a * b));

            bool IsMatch(long v, long[] rule) => v >= rule[0] && v <= rule[1] || v >= rule[2] && v <= rule[3];
            bool IsGoodTicket(long[] ticket) => ticket.All(v => rules.Any(rule => IsMatch(v, rule)));
        }

        static void Main_16_1()
        {
            var groups = File.ReadAllText("day16.txt")
                .Split("\n\n")
                .ToArray();

            var rules = groups[0]
                .Split('\n')
                .Select(x =>
                {
                    var strings = x.Split(new[] {": ", " or ", "-"}, StringSplitOptions.RemoveEmptyEntries);
                    return strings
                        .Skip(1)
                        .Select(long.Parse)
                        .ToArray();
                })
                .ToArray();

            var tickets = groups[2]
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(x => x.Split(',').Select(long.Parse).ToArray())
                .ToArray();

            var invalidValues = tickets.SelectMany(t => t.Where(v => !rules.Any(rule => IsMatch(v, rule)))).ToArray();
            Console.Out.WriteLine(invalidValues.Sum());

            bool IsMatch(long v, long[] rule) => v >= rule[0] && v <= rule[1] || v >= rule[2] && v <= rule[3];
        }

        static void Main_15()
        {
            var lines = "2,0,1,9,5,19".Split(',')
                .Select(long.Parse)
                .ToArray();

            var spokenAt = new Dictionary<long, (long t1, long t2)>();

            for (var i = 0; i < lines.Length; i++)
                spokenAt[lines[i]] = (-1, i);

            var last = lines[^1];

            const int count = 30000000; // or 2020 for part 1
            for (var i = lines.Length; i < count; i++)
            {
                var (t1, t2) = spokenAt[last];
                last = t1 == -1 ? 0 : t2 - t1;
                if (spokenAt.TryGetValue(last, out var tt))
                    spokenAt[last] = (tt.t2, i);
                else
                    spokenAt[last] = (-1, i);
            }

            Console.Out.WriteLine(last);
        }

        static void Main_14_2()
        {
            var lines = File.ReadAllLines("day14.txt")
                .Select(x => x.Split(new[] {" ", "mask", "=", "mem", "[", "]"}, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            var masks = new List<(long resetMask, long setMask)>();
            var mem = new Dictionary<long, long>();
            foreach (var line in lines)
            {
                if (line.Length == 1)
                {
                    masks.Clear();
                    masks.Add((0, 0));
                    long bit = 1;
                    for (var i = line[0].Length - 1; i >= 0; i--, bit <<= 1)
                    {
                        switch (line[0][i])
                        {
                            case 'X':
                                masks.AddRange(masks);
                                for (var k = 0; k < masks.Count; k++)
                                {
                                    if (k < masks.Count / 2)
                                        masks[k] = (masks[k].resetMask, masks[k].setMask | bit);
                                    else
                                        masks[k] = (masks[k].resetMask | bit, masks[k].setMask);
                                }

                                break;
                            case '1':
                                for (var k = 0; k < masks.Count; k++)
                                    masks[k] = (masks[k].resetMask, masks[k].setMask | bit);

                                break;
                        }
                    }
                }
                else
                {
                    var adr = long.Parse(line[0]);
                    var value = long.Parse(line[1]);
                    foreach (var (resetMask, setMask) in masks)
                        mem[adr & ~resetMask | setMask] = value;
                }
            }

            Console.Out.WriteLine(mem.Sum(x => x.Value));
        }

        static void Main_14_1()
        {
            var lines = File.ReadAllLines("day14.txt")
                .Select(x => x.Split(new[] {" ", "mask", "=", "mem", "[", "]"}, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            long andMask = 0;
            long orMask = 0;
            var mem = new long[100000];
            foreach (var line in lines)
            {
                if (line.Length == 1)
                {
                    andMask = Convert.ToInt64(line[0].Replace('X', '1'), 2);
                    orMask = Convert.ToInt64(line[0].Replace('X', '0'), 2);
                }
                else
                {
                    var adr = int.Parse(line[0]);
                    var value = long.Parse(line[1]);
                    mem[adr] = value & andMask | orMask;
                }
            }

            Console.Out.WriteLine(mem.Sum());
        }

        static void Main_13_2()
        {
            var lines = File.ReadAllLines("day13.txt");

            // x*N = b mod K
            var nums = lines[1].Split(',');

            var eqs = new List<(long x, long b, long K)>();

            var first = long.Parse(nums[0]);
            for (var i = 1; i < nums.Length; i++)
            {
                if (nums[i] == "x")
                    continue;

                var next = long.Parse(nums[i]);
                long delta = i;

                var b = (next - delta % next) % next;
                eqs.Add((first % next, b, next));
            }

            var results = new List<long>();
            var results2 = new List<long>();

            while (eqs.Count > 1)
            {
                var nextEqs = new List<(long x, long b, long K)>();
                for (var i = 0; i < eqs.Count; i++)
                {
                    var (x, b, K) = eqs[i];

                    // N = x^-1 * b mod K
                    var N = Inv(x, K) * b % K;

                    if (i == 0)
                    {
                        results.Add(N);
                        results2.Add(K);
                    }
                    else
                        nextEqs.Add((results2.Last() % K, (N % K + K - results.Last() % K) % K, K));
                }

                eqs = nextEqs;
            }

            var (xx, bb, KK) = eqs.Last();
            var NN = Inv(xx, KK) * bb % KK;
            for (var i = results.Count - 1; i >= 0; i--)
                NN = NN * results2[i] + results[i];

            Console.Out.WriteLine(first * NN);

            static long Bp(long a, long n, long mod)
            {
                long res = 1;
                while (n != 0)
                {
                    if ((n & 1) != 0)
                        res = res * a % mod;
                    a = a * a % mod;
                    n >>= 1;
                }

                return res;
            }

            static long Inv(long x, long mod)
            {
                return Bp(x, mod - 2, mod);
            }
        }

        static void Main_13_1()
        {
            var lines = File.ReadAllLines("day13.txt");

            var ts = long.Parse(lines[0]);
            var nums = lines[1].Split(',').Where(x => x != "x").Select(long.Parse).ToArray();

            var array = nums.Select(x => new {r = ts % x == 0 ? ts % x : x - ts % x, x}).ToArray();
            var min = array.OrderBy(x => x.r).First();
            Console.Out.WriteLine(min.r * min.x);
        }

        static void Main_12_2()
        {
            var lines = File.ReadAllLines("day12.txt")
                .Select(x => (cmd: x[0], n: int.Parse(x.Substring(1))))
                .ToArray();

            var dv = new[] {new V(0, -1), new V(1, 0), new V(0, 1), new V(-1, 0)};
            var pos = new V(0, 0);
            var vp = new V(10, -1);

            foreach (var (cmd, n) in lines)
            {
                switch (cmd)
                {
                    case 'F':
                        pos += vp * n;
                        break;
                    case 'N':
                        vp += dv[0] * n;
                        break;
                    case 'E':
                        vp += dv[1] * n;
                        break;
                    case 'S':
                        vp += dv[2] * n;
                        break;
                    case 'W':
                        vp += dv[3] * n;
                        break;
                    case 'L':
                        switch (n / 90 % 4)
                        {
                            case 0:
                                break;
                            case 2:
                                vp = -vp;
                                break;
                            case 1:
                                vp = new V(vp.Y, -vp.X);
                                break;
                            case 3:
                                vp = new V(-vp.Y, vp.X);
                                break;
                        }

                        break;
                    case 'R':
                        switch (n / 90 % 4)
                        {
                            case 0:
                                break;
                            case 2:
                                vp = -vp;
                                break;
                            case 3:
                                vp = new V(vp.Y, -vp.X);
                                break;
                            case 1:
                                vp = new V(-vp.Y, vp.X);
                                break;
                        }

                        break;
                }
            }

            Console.Out.WriteLine(pos.MLen());
        }

        static void Main_12_1()
        {
            var lines = File.ReadAllLines("day12.txt")
                .Select(x => (cmd: x[0], n: int.Parse(x.Substring(1))))
                .ToArray();

            var delta = new[] {new V(0, -1), new V(1, 0), new V(0, 1), new V(-1, 0)};
            var dir = 1;
            var pos = new V(0, 0);

            foreach (var (cmd, n) in lines)
            {
                switch (cmd)
                {
                    case 'F':
                        pos += delta[dir] * n;
                        break;
                    case 'N':
                        pos += delta[0] * n;
                        break;
                    case 'E':
                        pos += delta[1] * n;
                        break;
                    case 'S':
                        pos += delta[2] * n;
                        break;
                    case 'W':
                        pos += delta[3] * n;
                        break;
                    case 'L':
                        dir = (dir + 4 - n / 90 % 4) % 4;
                        break;
                    case 'R':
                        dir = (dir + n / 90) % 4;
                        break;
                }
            }

            Console.Out.WriteLine(pos.MLen());
        }

        static void Main_11_2()
        {
            var lines = File.ReadAllLines("day11.txt")
                .Select(x => x.ToCharArray()).ToArray();

            var nears = new List<V>[lines.Length, lines[0].Length];
            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[0].Length; x++)
                nears[y, x] = CalcNears(x, y);

            var hash = CalcHash();
            while (true)
            {
                lines = Sim();
                var nextHash = CalcHash();
                if (nextHash == hash)
                    break;

                hash = nextHash;
            }

            Console.Out.WriteLine(lines.SelectMany(x => x).Count(c => c == '#'));

            int CalcHash()
            {
                var res = 0;
                foreach (var l in lines)
                    res = HashCode.Combine(res, new string(l).GetHashCode());

                return res;
            }

            char[][] Sim()
            {
                var next = lines.Select(x => x.ToArray()).ToArray();
                for (var y = 0; y < lines.Length; y++)
                for (var x = 0; x < lines[0].Length; x++)
                    next[y][x] = Calc(x, y);

                return next;
            }

            char Calc(int x, int y)
            {
                if (lines[y][x] == '.')
                    return '.';

                var cnt = 0;
                foreach (var near in nears[y, x])
                {
                    if (lines[near.Y][near.X] == '#')
                        cnt++;
                }

                if (lines[y][x] == 'L' && cnt == 0)
                    return '#';

                if (lines[y][x] == '#' && cnt >= 5)
                    return 'L';

                return lines[y][x];
            }

            List<V> CalcNears(int x, int y)
            {
                var dvs = new[]
                {
                    new V(-1, -1), new V(-1, 0), new V(-1, 1), new V(0, 1), new V(1, 1), new V(1, 0), new V(1, -1),
                    new V(0, -1)
                };

                var res = new List<V>();

                foreach (var dv in dvs)
                {
                    var v = new V(x, y);
                    while (true)
                    {
                        v += dv;

                        if (v.X < 0)
                            break;
                        if (v.X >= lines[0].Length)
                            break;
                        if (v.Y < 0)
                            break;
                        if (v.Y >= lines.Length)
                            break;

                        if (lines[v.Y][v.X] == '.')
                            continue;

                        res.Add(v);
                        break;
                    }
                }

                return res;
            }
        }

        static void Main_11_1()
        {
            var lines = File.ReadAllLines("day11.txt")
                .Select(x => x.ToCharArray()).ToArray();

            var hash = CalcHash();
            while (true)
            {
                lines = Sim();
                var nextHash = CalcHash();
                if (nextHash == hash)
                    break;

                hash = nextHash;
            }

            Console.Out.WriteLine(lines.SelectMany(x => x).Count(c => c == '#'));

            int CalcHash()
            {
                var res = 0;
                foreach (var l in lines)
                    res = HashCode.Combine(res, new string(l).GetHashCode());

                return res;
            }

            char[][] Sim()
            {
                var next = lines.Select(x => x.ToArray()).ToArray();
                for (var y = 0; y < lines.Length; y++)
                for (var x = 0; x < lines[0].Length; x++)
                    next[y][x] = Calc(x, y);

                return next;
            }

            char Calc(int x, int y)
            {
                if (lines[y][x] == '.')
                    return '.';

                var cnt = 0;
                for (var dx = -1; dx <= 1; dx++)
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (x + dx < 0)
                        continue;
                    if (x + dx >= lines[0].Length)
                        continue;
                    if (y + dy < 0)
                        continue;
                    if (y + dy >= lines.Length)
                        continue;

                    if (lines[y + dy][x + dx] == '#')
                        cnt++;
                }

                if (lines[y][x] == 'L' && cnt == 0)
                    return '#';

                if (lines[y][x] == '#' && cnt >= 4)
                    return 'L';

                return lines[y][x];
            }
        }

        static void Main_10_2()
        {
            var nums = File.ReadAllLines("day10.txt")
                .Select(long.Parse)
                .OrderBy(x => x)
                .ToList();
            nums.Insert(0, 0);
            nums.Add(nums.Last() + 3);

            var res = new Dictionary<int, long>();

            Console.Out.WriteLine(Calc(nums.Count - 1));

            long Calc(int last)
            {
                if (last == 0)
                    return 1;

                if (res.TryGetValue(last, out var result))
                    return result;

                var count = 0L;
                for (var i = last - 1; i >= 0; i--)
                {
                    if (nums[i] < nums[last] - 3)
                        break;

                    count += Calc(i);
                }

                return res[last] = count;
            }
        }

        static void Main_10_1()
        {
            var nums = File.ReadAllLines("day10.txt")
                .Select(long.Parse)
                .OrderBy(x => x)
                .ToList();
            nums.Insert(0, 0);
            nums.Add(nums.Last() + 3);

            var d1 = 0;
            var d3 = 0;
            for (var i = 0; i < nums.Count - 1; i++)
            {
                if (nums[i + 1] - nums[i] == 1)
                    d1++;
                if (nums[i + 1] - nums[i] == 3)
                    d3++;
            }

            Console.Out.WriteLine(d1 * d3);
        }

        static void Main_9_2()
        {
            var nums = File.ReadAllLines("day9.txt")
                .Select(long.Parse)
                .ToArray();

            var res = Find();

            for (var i = 0; i < nums.Length; i++)
            {
                long sum = 0;
                for (var j = i; j < nums.Length; j++)
                {
                    sum += nums[j];
                    if (sum == res)
                    {
                        var range = nums.Skip(i).Take(j - i + 1).ToArray();
                        Console.Out.WriteLine(range.Min() + range.Max());
                        return;
                    }

                    if (sum > res)
                        break;
                }
            }

            long Find()
            {
                for (var i = 25; i < nums.Length; i++)
                {
                    var range = nums.Skip(i - 25).Take(25).ToArray();
                    var ok = false;
                    for (var j = 0; j < range.Length - 1; j++)
                    for (var k = j + 1; k < range.Length; k++)
                    {
                        if (range[j] + range[k] == nums[i])
                        {
                            ok = true;
                            break;
                        }
                    }

                    if (!ok)
                        return nums[i];
                }

                throw new Exception("WTF???");
            }
        }

        static void Main_9_1()
        {
            var nums = File.ReadAllLines("day9.txt")
                .Select(long.Parse)
                .ToArray();

            for (var i = 25; i < nums.Length; i++)
            {
                var range = nums.Skip(i - 25).Take(25).ToArray();
                var ok = false;
                for (var j = 0; j < range.Length - 1; j++)
                for (var k = j + 1; k < range.Length; k++)
                {
                    if (range[j] + range[k] == nums[i])
                    {
                        ok = true;
                        break;
                    }
                }

                if (!ok)
                    Console.Out.WriteLine(nums[i]);
            }
        }

        static void Main_8_2()
        {
            var program = Computer.Parse(File.ReadAllLines("day8.txt"));

            foreach (var line in program)
            {
                if (line.type == IT.jmp)
                {
                    line.type = IT.nop;
                    var x = Eval();
                    if (x != long.MinValue)
                    {
                        Console.Out.WriteLine(x);
                        return;
                    }

                    line.type = IT.jmp;
                }
                else if (line.type == IT.nop)
                {
                    line.type = IT.jmp;
                    var x = Eval();
                    if (x != long.MinValue)
                    {
                        Console.Out.WriteLine(x);
                        return;
                    }

                    line.type = IT.nop;
                }
            }

            long Eval()
            {
                var computer = new Computer();
                var used = new HashSet<long>();
                while (!computer.IsTerminated(program))
                {
                    if (computer.ip < 0)
                        return long.MinValue;

                    if (!used.Add(computer.ip))
                        return long.MinValue;

                    computer = computer.Next(program);
                }

                return computer.acc;
            }
        }

        static void Main_8_1()
        {
            var program = Computer.Parse(File.ReadAllLines("day8.txt"));

            var computer = new Computer();
            var used = new HashSet<long>();
            while (!computer.IsTerminated(program))
            {
                if (!used.Add(computer.ip))
                {
                    Console.Out.WriteLine(computer.acc);
                    return;
                }

                computer = computer.Next(program);
            }
        }

        static void Main_7_2()
        {
            var rules = File.ReadAllLines("day7.txt")
                .Select(r => r.Split(" bags contain "))
                .Select(r => new
                {
                    outer = r[0],
                    inner = r[1]
                        .Replace(" bags", "")
                        .Replace(" bag", "")
                        .Split(new[] {',', '.'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim())
                        .Where(v => v != "no other")
                        .Select(v => v.Split(' ', 2))
                        .Select(v => (count: int.Parse(v[0]), color: v[1]))
                        .ToList()
                })
                .ToDictionary(x => x.outer, x => x.inner);


            Console.Out.WriteLine(Calc("shiny gold"));

            long Calc(string outer)
            {
                var res = 0L;
                foreach (var (count, color) in rules[outer])
                    res += count + count * Calc(color);

                return res;
            }
        }

        static void Main_7_1()
        {
            var rules = File.ReadAllLines("day7.txt")
                .Select(r => r.Split(" bags contain "))
                .Select(r => new {outer = r[0], inner = r[1]})
                .ToArray();


            var queue = new Queue<string>();
            queue.Enqueue("shiny gold");
            var used = new HashSet<string>();
            used.Add("shiny gold");
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var next in rules.Where(x => x.inner.Contains(cur)))
                {
                    if (used.Add(next.outer))
                        queue.Enqueue(next.outer);
                }
            }


            Console.Out.WriteLine(used.Count - 1);
        }

        static void Main_6_2()
        {
            var groups = File.ReadAllText("day6.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            var res = groups.Select(Solve).Sum();
            Console.Out.WriteLine(res);

            static int Solve(string[] gr)
            {
                var r = gr[0].ToHashSet();
                foreach (var item in gr)
                    r.IntersectWith(item);

                return r.Count;
            }
        }

        static void Main_6_1()
        {
            var groups = File.ReadAllText("day6.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace("\n", "").Trim());

            var res = groups.Select(x => x.Distinct().Count()).Sum();
            Console.Out.WriteLine(res);
        }

        static void Main_5_2()
        {
            var lines = File.ReadAllLines("day5.txt");

            var all = new HashSet<int>(lines.Select(Solve));

            const int min = 8;
            const int max = (1 << 10) - 8;
            for (var i = min; i <= max; i++)
            {
                if (!all.Contains(i) && all.Contains(i + 1) && all.Contains(i - 1))
                    Console.Out.WriteLine(i);
            }

            static int Solve(string line)
            {
                return Convert.ToInt32(
                    line.Replace('B', '1')
                        .Replace('R', '1')
                        .Replace('F', '0')
                        .Replace('L', '0'),
                    2);
            }
        }

        static void Main_5_1()
        {
            var lines = File.ReadAllLines("day5.txt");

            var res = lines.Select(Solve).Max();

            Console.Out.WriteLine(res);

            static int Solve(string line)
            {
                return Convert.ToInt32(
                    line.Replace('B', '1')
                        .Replace('R', '1')
                        .Replace('F', '0')
                        .Replace('L', '0'),
                    2);
            }
        }

        static void Main_4_2()
        {
            var passports = File.ReadAllText("day4.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split(new[] {'\n', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries));

            var res = 0;
            var req = new[] {"ecl", "pid", "eyr", "hcl", "byr", "iyr", /*"cid", */"hgt"};
            foreach (var passport in passports)
            {
                var dct = new Dictionary<string, string>();
                for (var i = 0; i < passport.Length; i += 2)
                {
                    var key = passport[i];
                    var value = passport[i + 1];
                    dct[key] = value;
                }

                if (req.All(r => dct.ContainsKey(r)))
                {
                    var invalid = false;
                    foreach (var (key, value) in dct)
                    {
                        if (invalid)
                            break;
                        switch (key)
                        {
                            case "byr":
                                if (!int.TryParse(value, out var byr) || byr < 1920 || byr > 2002)
                                    invalid = true;
                                break;
                            case "iyr":
                                if (!int.TryParse(value, out var iyr) || iyr < 2010 || iyr > 2020)
                                    invalid = true;
                                break;
                            case "eyr":
                                if (!int.TryParse(value, out var eyr) || eyr < 2020 || eyr > 2030)
                                    invalid = true;
                                break;
                            case "hgt":
                                if (value.EndsWith("cm") &&
                                    int.TryParse(value.Substring(0, value.Length - 2), out var cm) && cm >= 150 &&
                                    cm <= 193)
                                {
                                }
                                else if (value.EndsWith("in") &&
                                         int.TryParse(value.Substring(0, value.Length - 2), out var @in) && @in >= 59 &&
                                         @in <= 76)
                                {
                                }
                                else
                                    invalid = true;

                                break;
                            case "ecl":
                                if (!"amb blu brn gry grn hzl oth".Split().Contains(value))
                                    invalid = true;
                                break;
                            case "pid":
                                if (value.Length != 9 || !value.All(char.IsDigit))
                                    invalid = true;
                                break;
                            case "hcl":
                                if (!value.StartsWith("#") || !value.Substring(1).All(c =>
                                    char.IsDigit(c) || "abcdef".Contains(c)))
                                    invalid = true;
                                break;
                        }
                    }

                    if (!invalid)
                        res++;
                }
            }

            Console.Out.WriteLine(res);
        }

        static void Main_4_1()
        {
            var passports = File.ReadAllText("day4.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split(new[] {'\n', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries));

            var res = 0;
            var req = new[] {"ecl", "pid", "eyr", "hcl", "byr", "iyr", /*"cid", */"hgt"};
            foreach (var passport in passports)
            {
                var dct = new Dictionary<string, string>();
                for (var i = 0; i < passport.Length; i += 2)
                {
                    var key = passport[i];
                    var value = passport[i + 1];
                    dct[key] = value;
                }

                if (req.All(r => dct.ContainsKey(r)))
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_3_2()
        {
            var lines = File.ReadAllLines("day3.txt");
            long res1 = Calc(1, 1);
            long res2 = Calc(3, 1);
            long res3 = Calc(5, 1);
            long res4 = Calc(7, 1);
            long res5 = Calc(1, 2);

            Console.Out.WriteLine(res1 * res2 * res3 * res4 * res5);

            int Calc(int dx, int dy)
            {
                var res = 0;

                var x = 0;
                var y = 0;
                while (y < lines.Length)
                {
                    if (lines[y][x] == '#')
                        res++;
                    y += dy;
                    x = (x + dx) % lines[0].Length;
                }

                return res;
            }
        }

        static void Main_3_1()
        {
            var lines = File.ReadAllLines("day3.txt");
            var res = 0;

            var x = 0;
            var y = 0;
            while (y < lines.Length)
            {
                if (lines[y][x] == '#')
                    res++;
                y++;
                x = (x + 3) % lines[0].Length;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_2_2()
        {
            var lines = File.ReadAllLines("day2.txt");
            var res = 0;
            foreach (var line in lines)
            {
                var split = line.Split(new[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(split[0]);
                var max = int.Parse(split[1]);
                var ch = split[2][0];
                var psw = split[3];
                var c1 = psw[min - 1] == ch ? 1 : 0;
                var c2 = psw[max - 1] == ch ? 1 : 0;
                if (c1 + c2 == 1)
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_2_1()
        {
            var lines = File.ReadAllLines("day2.txt");
            var res = 0;
            foreach (var line in lines)
            {
                var split = line.Split(new[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(split[0]);
                var max = int.Parse(split[1]);
                var ch = split[2][0];
                var psw = split[3];
                var count = psw.Count(x => x == ch);
                if (count >= min && count <= max)
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_1_2()
        {
            var numbers = File.ReadAllLines("day1.txt").Select(long.Parse).ToArray();

            for (var i = 0; i < numbers.Length - 2; i++)
            for (var j = i + 1; j < numbers.Length - 1; j++)
            for (var k = j + 1; k < numbers.Length; k++)
            {
                if (numbers[i] + numbers[j] + numbers[k] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j] * numbers[k]);
            }
        }

        static void Main_1_1()
        {
            var numbers = File.ReadAllLines("day1.txt").Select(long.Parse).ToArray();

            for (var i = 0; i < numbers.Length - 1; i++)
            for (var j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[i] + numbers[j] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j]);
            }
        }
    }
}
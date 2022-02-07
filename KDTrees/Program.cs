﻿// See https://aka.ms/new-console-template for more information
using KDTrees;
using KDTrees.Strategies;
using System.Diagnostics;

Console.WriteLine($"{DateTime.Now} Building map and points to check");
var map = new MapOfPoints(GenerateRandomUniquePoints(pointCount: 10_000_000));
var pointsToCheck = GenerateRandomUniquePoints(pointCount: 10_000);

Console.WriteLine($"{DateTime.Now} Building indexes");
var treeStrategy = new TreeStrategy();
treeStrategy.BuildIndex(map);

// To test if a given strategy works, out-comment the following to test it against the simple and correct, but slow strategy, DirtyStrategy
// RunStrategyAndCompareWithDirtyToCheckIfErrorsCanBeFound(strategyToTest: treeStrategy, map: map);


CheckAllPointsAgainstStrategy(strategyToTest: treeStrategy, pointsToCheck: pointsToCheck);

Console.ReadLine();

static void CheckAllPointsAgainstStrategy(IClosestPointFindStrategy strategyToTest, HashSet<Point> pointsToCheck)
{
    Console.WriteLine($"{DateTime.Now} Testing strategy {strategyToTest.GetType().Name}...");
    var stopwatch = Stopwatch.StartNew();
    foreach (var p in pointsToCheck)
    {
        strategyToTest.FindClosestPoints(p);
    }
    Console.WriteLine($"{DateTime.Now} Strategy {strategyToTest.GetType().Name} checked all {pointsToCheck.Count} points in {stopwatch.ElapsedMilliseconds}ms");

}

static void RunStrategyAndCompareWithDirtyToCheckIfErrorsCanBeFound(IClosestPointFindStrategy strategyToTest, MapOfPoints map)
{
    // VALIDATE A given strategy compared to the dirty strategy
    Console.WriteLine($"{DateTime.Now} Starting to check same value as dirty");
    while (true)
    {
        var dirtyStrategy = new DirtyStrategy();
        dirtyStrategy.BuildIndex(map);
        var checkPoint = GenerateRandomUniquePoints(pointCount: 1).Single();
        var result1 = dirtyStrategy.FindClosestPoints(checkPoint);
        var result2 = strategyToTest.FindClosestPoints(checkPoint);
        if (result1.Distance != result2.Distance)
            throw new Exception("Not the same distance!");

        if (result1.ClosestPoints.Count != result2.ClosestPoints.Count)
            throw new Exception("Not the same point count!");
        Console.Write(".");
    }
}

static HashSet<Point> GenerateRandomUniquePoints(int pointCount)
{
    int pointMaxValue = 1_000_000;
    var pointsToCheck = new HashSet<Point>();
    var ran = new Random();
    while (pointsToCheck.Count < pointCount)
    {
        pointsToCheck.Add(new Point(x: ran.Next(-pointMaxValue, pointMaxValue), y: ran.Next(-pointMaxValue, pointMaxValue)));
    }
    return pointsToCheck;
}

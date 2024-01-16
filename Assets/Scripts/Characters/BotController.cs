using System;
using System.Collections.Generic;
using Characters.View;
using Range;
using ScriptableObjects.Skills;
using Skills.View;
using World;

namespace Characters
{
    public static class BotController
    {
        // public static ((int, int), SkillObject) GenerateBotSkillWithPosition(int botId, RangeCalculator rangeCalculator, CharactersController charactersController)
        // {
        //     List<(int, int)> charactersPositions = charactersController.OtherCharactersPositions(botId);
        //     Character botCharacter = charactersController.PlayerCharacter(botId);
        //     List<SkillObject> botSkills = botCharacter.Player.equippedCharacter.skills;
        //
        //     return CalculateBestSkillTile(botSkills, rangeCalculator, charactersPositions, botCharacter.Position);
        // }
        //
        // static ((int, int), SkillObject) CalculateBestSkillTile(List<SkillObject> botSkills, RangeCalculator rangeCalculator, List<(int, int)> charactersPositions, (int, int) botPosition)
        // {
        //     List<(int, (int, int))> valuationsCross = new List<(int, (int, int))>();
        //     List<(int, (int, int))> valuationsX = new List<(int, (int, int))>();
        //     List<(int, (int, int))> valuationsSquare = new List<(int, (int, int))>();
        //     List<(int, (int, int))> valuationsHorizontalLine = new List<(int, (int, int))>();
        //     List<(int, (int, int))> valuationsVerticalLine = new List<(int, (int, int))>();
        //     List<(int, int)> validTiles;
        //
        //     foreach (var skill in botSkills)
        //     {
        //         switch (skill.skillArea)
        //         {
        //             case SkillArea.Cross:
        //                 validTiles = rangeCalculator.GetRange(SkillArea.Cross, skill.range, botPosition);
        //                 valuationsCross = CalculateBestSkillTileCross(validTiles, charactersPositions, skill.areaSize);
        //                 break;
        //             case SkillArea.X:
        //                 validTiles = rangeCalculator.GetRange(SkillArea.Cross, skill.range, botPosition);
        //                 valuationsX = CalculateBestSkillTileX(validTiles, charactersPositions, skill.areaSize);
        //                 break;
        //             case SkillArea.Square:
        //                 validTiles = rangeCalculator.GetRange(SkillArea.Cross, skill.range, botPosition);
        //                 valuationsSquare = CalculateBestSkillTileSquare(validTiles, charactersPositions, skill.areaSize);
        //                 break;
        //             case SkillArea.HorizontalLine:
        //                 validTiles = rangeCalculator.GetRange(SkillArea.Cross, skill.range, botPosition);
        //                 valuationsHorizontalLine = CalculateBestSkillTileHorizontalLine(validTiles, charactersPositions, skill.areaSize);
        //                 break;
        //             case SkillArea.VerticalLine:
        //                 validTiles = rangeCalculator.GetRange(SkillArea.Cross, skill.range, botPosition);
        //                 valuationsVerticalLine = CalculateBestSkillTileVerticalLine(validTiles, charactersPositions, skill.areaSize);
        //                 break;
        //         }
        //     }
        //
        //     (int, int) bestPosition = botPosition;
        //     SkillArea bestSkill = SkillArea.Cross;
        //     int greatestValuation = 0;
        //     for (int i=0; i<valuationsCross.Count; i++)
        //     {
        //         if (greatestValuation < valuationsCross[i].Item1)
        //         {
        //             bestPosition = valuationsCross[i].Item2;
        //             greatestValuation = valuationsCross[i].Item1;
        //             bestSkill = SkillArea.Cross;
        //         }
        //     }
        //     for (int i=0; i<valuationsX.Count; i++)
        //     {
        //         if (greatestValuation < valuationsX[i].Item1)
        //         {
        //             bestPosition = valuationsX[i].Item2;
        //             greatestValuation = valuationsX[i].Item1;
        //             bestSkill = SkillArea.X;
        //         }
        //     }
        //     for (int i=0; i<valuationsSquare.Count; i++)
        //     {
        //         if (greatestValuation < valuationsSquare[i].Item1)
        //         {
        //             bestPosition = valuationsSquare[i].Item2;
        //             greatestValuation = valuationsSquare[i].Item1;
        //             bestSkill = SkillArea.Square;
        //         }
        //     }
        //     for (int i=0; i<valuationsHorizontalLine.Count; i++)
        //     {
        //         if (greatestValuation < valuationsHorizontalLine[i].Item1)
        //         {
        //             bestPosition = valuationsHorizontalLine[i].Item2;
        //             greatestValuation = valuationsHorizontalLine[i].Item1;
        //             bestSkill = SkillArea.HorizontalLine;
        //         }
        //     }
        //     for (int i=0; i<valuationsVerticalLine.Count; i++)
        //     {
        //         if (greatestValuation < valuationsVerticalLine[i].Item1)
        //         {
        //             bestPosition = valuationsVerticalLine[i].Item2;
        //             greatestValuation = valuationsVerticalLine[i].Item1;
        //             bestSkill = SkillArea.VerticalLine;
        //         }
        //     }
        //
        //     int skillIndex = 0;
        //     foreach (var skill in botSkills)
        //     {
        //         if (skill.skillArea == bestSkill) { break; }
        //
        //         skillIndex++;
        //     }
        //
        //     return (bestPosition, botSkills[skillIndex]);
        // }
        //
        // static List<(int, (int, int))> CalculateBestSkillTileHorizontalLine(List<(int, int)> validTiles, List<(int, int)> charactersPositions, int skillAreaSize)
        // {
        //     List<(int, (int, int))> valuationsWithPosition = new List<(int, (int, int))>();
        //     foreach (var validTile in validTiles)
        //     {
        //         int valuation = 0;
        //         foreach (var characterPosition in charactersPositions)
        //         {
        //             int horizontalDistance = Math.Abs(validTile.Item1 - characterPosition.Item1);
        //             if (horizontalDistance <= skillAreaSize && validTile.Item2 == characterPosition.Item2)
        //             {
        //                 valuation++;
        //             }
        //         }
        //         valuationsWithPosition.Add((valuation, validTile));
        //     }
        //
        //     return valuationsWithPosition;
        // }
        //
        // static List<(int, (int, int))> CalculateBestSkillTileVerticalLine(List<(int, int)> validTiles, List<(int, int)> charactersPositions, int skillAreaSize)
        // {
        //     List<(int, (int, int))> valuationsWithPosition = new List<(int, (int, int))>();
        //     foreach (var validTile in validTiles)
        //     {
        //         int valuation = 0;
        //         foreach (var characterPosition in charactersPositions)
        //         {
        //             int verticalDistance = Math.Abs(validTile.Item2 - characterPosition.Item2);
        //             if (verticalDistance <= skillAreaSize && validTile.Item1 == characterPosition.Item1)
        //             {
        //                 valuation++;
        //             }
        //         }
        //         valuationsWithPosition.Add((valuation, validTile));
        //     }
        //
        //     return valuationsWithPosition;
        // }
        //
        // static List<(int, (int, int))> CalculateBestSkillTileSquare(List<(int, int)> validTiles, List<(int, int)> charactersPositions, int skillAreaSize)
        // {
        //     List<(int, (int, int))> valuationsWithPosition = new List<(int, (int, int))>();
        //     foreach (var validTile in validTiles)
        //     {
        //         int valuation = 0;
        //         foreach (var characterPosition in charactersPositions)
        //         {
        //             int horizontalDistance = Math.Abs(validTile.Item1 - characterPosition.Item1);
        //             int verticalDistance = Math.Abs(validTile.Item2 - characterPosition.Item2);
        //             if (verticalDistance <= skillAreaSize && horizontalDistance <= skillAreaSize)
        //             {
        //                 valuation++;
        //             }
        //         }
        //         valuationsWithPosition.Add((valuation, validTile));
        //     }
        //
        //     return valuationsWithPosition;
        // }
        //
        // static List<(int, (int, int))> CalculateBestSkillTileX(List<(int, int)> validTiles, List<(int, int)> charactersPositions, int skillAreaSize)
        // {
        //     List<(int, (int, int))> valuationsWithPosition = new List<(int, (int, int))>();
        //     foreach (var validTile in validTiles)
        //     {
        //         int valuation = 0;
        //         foreach (var characterPosition in charactersPositions)
        //         {
        //             int horizontalDistance = Math.Abs(validTile.Item1 - characterPosition.Item1);
        //             int verticalDistance = Math.Abs(validTile.Item2 - characterPosition.Item2);
        //             if (verticalDistance <= skillAreaSize
        //                 && horizontalDistance <= skillAreaSize
        //                 && validTile.Item1 == characterPosition.Item1
        //                 && validTile.Item2 == characterPosition.Item2)
        //             {
        //                 valuation++;
        //             }
        //         }
        //         valuationsWithPosition.Add((valuation, validTile));
        //     }
        //
        //     return valuationsWithPosition;
        // }
        //
        // static List<(int, (int, int))> CalculateBestSkillTileCross(List<(int, int)> validTiles, List<(int, int)> charactersPositions, int skillAreaSize)
        // {
        //     List<(int, (int, int))> valuationsWithPosition = new List<(int, (int, int))>();
        //     foreach (var validTile in validTiles)
        //     {
        //         int valuation = 0;
        //         foreach (var characterPosition in charactersPositions)
        //         {
        //             if (ManhattanDistance(validTile, characterPosition) <= skillAreaSize)
        //             {
        //                 valuation++;
        //             }
        //         }
        //         valuationsWithPosition.Add((valuation, validTile));
        //     }
        //
        //     return valuationsWithPosition;
        // }
        //
        // public static (int, int) GenerateBotMovement(int botId, CharactersController charactersController, MapController mapController)
        // {
        //     Character botCharacter = charactersController.PlayerCharacter(botId);
        //     List<(int, int)> validTiles = mapController.GetNonVoidTilePositions();
        //     List<(int, int)> charactersPositions = charactersController.OtherCharactersPositions(botId);
        //     BotProfile botProfile = botId % 2 == 0 ? BotProfile.Aggressive : BotProfile.Coward;
        //
        //     RemoveNonValidTiles(validTiles, botCharacter, charactersPositions);
        //
        //     int halfMapWidth = mapController.MapWidth() / 2;
        //     int halfMapHeight = mapController.MapHeight() / 2;
        //
        //     //TODO: If the new map height with border change too much the coward subtract the difference
        //     int cowardDistanceThreshold = halfMapHeight;
        //     (int, int) center = (halfMapWidth, halfMapHeight);
        //
        //     int bestTileIndex = CalculateBestTileToMove(botProfile, validTiles, charactersPositions, cowardDistanceThreshold, center);
        //
        //     return validTiles[bestTileIndex];
        // }
        //
        // static void RemoveNonValidTiles(List<(int, int)> validTiles, Character botCharacter, List<(int, int)> charactersPositions)
        // {
        //     validTiles.RemoveAll(item => item == botCharacter.Position);
        //     validTiles.RemoveAll(item => ManhattanDistance(item, botCharacter.Position) > botCharacter.Stamina);
        //     foreach (var otherCharacterPosition in charactersPositions)
        //     {
        //         validTiles.Remove(otherCharacterPosition);
        //     }
        // }
        //
        // static int CalculateBestTileToMove(BotProfile profile, List<(int, int)> validTiles, List<(int, int)> charactersPositions, int cowardDistanceThreshold, (int, int) center)
        // {
        //     List<int> valuations = new List<int>();
        //     foreach (var validTile in validTiles)
        //     {
        //         valuations.Add(CalculateValidTileValuation(profile, charactersPositions, validTile, cowardDistanceThreshold, center));
        //     }
        //
        //     int bestIndex = 0;
        //     int greatestValuation = 0;
        //     for (int i=0; i<valuations.Count; i++)
        //     {
        //         if (greatestValuation < valuations[i])
        //         {
        //             bestIndex = i;
        //             greatestValuation = valuations[i];
        //         }
        //     }
        //
        //     return bestIndex;
        // }
        //
        // static int CalculateValidTileValuation(BotProfile profile, List<(int, int)> charactersPositions, (int, int) validTile, int cowardDistanceThreshold, (int, int) center)
        // {
        //     int valuation = 0;
        //     foreach (var characterPosition in charactersPositions)
        //     {
        //         int dist = ManhattanDistance(validTile, characterPosition);
        //         int centerDist = ManhattanDistance(validTile, center);
        //         if (profile == BotProfile.Aggressive)
        //         {
        //             valuation = CalculateAgressiveValuation(dist, centerDist);
        //         }
        //         else if (profile == BotProfile.Coward)
        //         {
        //             valuation = CalculateCowardValuation(dist, centerDist, cowardDistanceThreshold);
        //         }
        //     }
        //
        //     return valuation;
        // }
        //
        // private static int CalculateCowardValuation(int dist, int centerDist, int cowardDistanceThreshold)
        // {
        //     int valuation = 0;
        //
        //     if (dist < cowardDistanceThreshold)
        //     {
        //         valuation = dist;
        //     }
        //
        //     valuation -= centerDist / 2;
        //     return valuation;
        // }
        //
        // private static int CalculateAgressiveValuation(int dist, int centerDist)
        // {
        //     switch (dist)
        //     {
        //         case 1:
        //         case 7:
        //             return 100;
        //         case 2:
        //         case 6:
        //             return 200;
        //         case 3:
        //         case 5:
        //             return 300;
        //         case 4:
        //             return 400;
        //         default:
        //             return 100 - centerDist;
        //     }
        // }
        //
        // public static int ManhattanDistance((int, int) v1, (int, int) v2)
        // {
        //     return Math.Abs(v1.Item1 - v2.Item1) + Math.Abs(v1.Item2 - v2.Item2);
        // }
    }
}

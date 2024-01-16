using System.Collections.Generic;
using System.Linq;
using Characters.View;
using Range;
using ScriptableObjects.Skills;
using Skills.View;
using UnityEngine;
using World;
using Random = System.Random;

namespace Characters
{
    public class RandomoBotController
    {
        private RangeCalculator rangeCalculator;
        private CharactersController charactersController;
        MapController mapController;

        public RandomoBotController(CharactersController charactersController, MapController mapController)
        {
            this.charactersController = charactersController;
            this.mapController = mapController;
            rangeCalculator = new RangeCalculator(mapController);
        }

        public (int, int) GetBotRandomMovePosition(int botId)
        {
            List<(int, int)> playerCurrentRange = GetTilesInBotRange(botId);
            List<(int, int)> tilesInPlayerRange = GetValidTilesToMove(playerCurrentRange);
            List<(int, int)> borderTiles = mapController.GetBorderTiles();

            List<(int, int)> validTiles = new List<(int, int)>();
            foreach (var tileInRange in tilesInPlayerRange)
            {
                if (!borderTiles.Contains(tileInRange))
                {
                    validTiles.Add(tileInRange);
                }
            }

            if (validTiles.Count == 0)
            {
                Character character = charactersController.PlayerCharacter(botId);
                return (character.Position.Item1 + 2, character.Position.Item2 + 2);
            }

            return validTiles.LastOrDefault();
        }

        public ((int, int), SkillObject) GenerateBotSkillWithPosition(Character castingBot)
        {
            List<SkillObject> botSkills = castingBot.Player.equippedCharacter.skills;
            SkillObject randomSkill = botSkills[2];
            List<(int, int)> skillRange = rangeCalculator.GetRange(SkillArea.Cross, randomSkill.range, castingBot.Position);
            List<(int, int)> charactersInSkillRange = CharactersInSkillRange(skillRange, castingBot);

            if (charactersInSkillRange.Count == 1 || (charactersInSkillRange.Count > 1 && castingBot.Name.Contains("Angelica") ) )
            {
                return (charactersInSkillRange[0], randomSkill);
            }

            randomSkill = botSkills[1];
            skillRange = rangeCalculator.GetRange(SkillArea.Cross, randomSkill.range, castingBot.Position);
            charactersInSkillRange = CharactersInSkillRange(skillRange, castingBot);

            if (charactersInSkillRange.Count >= 1)
            {
                return (charactersController.GetCharacterWithHighestHealth(charactersInSkillRange).Position, randomSkill);
            }

            randomSkill = botSkills[0];
            return (castingBot.Position, randomSkill);
        }

        private List<(int, int)> CharactersInSkillRange(List<(int, int)> skillRange, Character castingBot)
        {
            List<(int, int)> charactersInSkillRange = new List<(int, int)>();
            foreach ((int, int) position in skillRange)
            {
                if (charactersController.IsCharacterOccupyingPosition(position) && position != castingBot.Position)
                {
                    Character character = charactersController.CharacterWithPosition(position);
                    if (!charactersController.GetCharacterGameObject(character).activeSelf ||character.CurrentLife <= 0)
                    {
                        continue;
                    }
                    charactersInSkillRange.Add(position);
                }
            }

            return charactersInSkillRange;
        }

        private List<(int, int)> GetTilesInBotRange(int botId)
        {
            Character character = charactersController.PlayerCharacter(botId);
            int stamina = character.Stamina;

            return rangeCalculator.GetRange(SkillArea.Cross, stamina, character.Position);
        }

        private List<(int, int)> GetValidTilesToMove(List<(int, int)> tilesPosition)
        {
            List<(int, int)> validTiles = new List<(int, int)>();
            foreach ((int, int) position in tilesPosition)
            {
                if (!charactersController.IsCharacterOccupyingPosition(position))
                {
                    validTiles.Add(position);
                }
            }

            return validTiles;
        }
    }
}

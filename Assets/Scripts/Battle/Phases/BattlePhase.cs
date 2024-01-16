using System.Collections.Generic;
using Battle.View;
using Characters;
using Characters.Input;
using Characters.View;
using Highlight;
using Highlight.View;
using Players;
using Range;
using ScriptableObjects.Skills;
using Skills;
using Skills.View;
using UnityEngine;
using World;

namespace Battle.Phases
{
    public class BattlePhase : MonoBehaviour
    {
        private RangeCalculator rangeCalculator;
        private HighlightController highlightController;
        private CharactersController charactersController;
        private SkillsController skillsController;
        private InputController inputController;
        private bool battlePhase;
        private int playerId;
        private (int, int) selectedPosition;
        private SkillExecution playerSkillExecution;
        private PlayerController playerController;
        private RandomoBotController randomoBotController;

        public void Initialize(PlayerController playerController, MapController mapController, HighlightController highlightController,
            CharactersController charactersController, InputController inputController,
            SkillsInitializer skillsInitializer, int playerId)
        {
            rangeCalculator = new RangeCalculator(mapController);
            this.highlightController = highlightController;
            this.charactersController = charactersController;
            this.inputController = inputController;
            this.playerId = playerId;
            this.playerController = playerController;
            randomoBotController = new RandomoBotController(charactersController, mapController);
            skillsController = new SkillsController(skillsInitializer, GetMainCharacterSkill(playerId));
            skillsController.CreateSkillCards();
            battlePhase = false;
            SkillsInitializer.onSkillSelect = new SkillsInitializer.OnSkillSelect(HighlightSkillOnSelect);
        }

        void Update()
        {
            if (!battlePhase)
            {
                return;
            }

            SkillObject selectedSkill = skillsController.SelectedSkill();
            Character character = charactersController.PlayerCharacter(playerId);
            List<(int, int)> areaTilesPositions = rangeCalculator.GetRange(SkillArea.Cross, selectedSkill.range, character.Position);
            HighlightSkillAreaOnSelectedTile(areaTilesPositions, character, selectedSkill);
        }

        public void SetupBattle()
        {
            battlePhase = true;
            playerSkillExecution = null;
            skillsController.ShowSkillCards();
            HighlightSkillRange(charactersController.PlayerCharacter(playerId), skillsController.SelectedSkill());
        }

        public List<SkillExecution> TierDownBattle()
        {
            battlePhase = false;
            skillsController.ClearSkillCards();
            highlightController.ClearHighlightedTiles();

            List<SkillExecution> battleSkills = new List<SkillExecution>();

            if (playerSkillExecution != null) { battleSkills.Add(playerSkillExecution); }

            foreach (var botId in playerController.PlayerIds())
            {
                if (botId == playerController.MainPlayer().id) { continue; }

                if (charactersController.PlayerCharacter(playerId).CurrentLife <= 0 || !charactersController.GetCharacterGameObject(charactersController.PlayerCharacter(playerId)).activeSelf)
                {
                    continue;
                }

                Character castingCharacter = charactersController.PlayerCharacter(botId);
                GameObject go = charactersController.GetCharacterGameObject(castingCharacter);
                if (go is null)
                {
                    continue;
                }

                if (go.activeSelf == false)
                {
                    continue;
                }

                ((int, int), SkillObject) positionWithSkill = randomoBotController.GenerateBotSkillWithPosition(castingCharacter);
                battleSkills.Add(CreateSkillExecution(castingCharacter, positionWithSkill.Item2, positionWithSkill.Item1));
            }

            return battleSkills;
        }

        private List<SkillObject> GetMainCharacterSkill(int playerId)
        {
            Character character = charactersController.PlayerCharacter(playerId);
            return charactersController.GetCharacterSkills(character);
        }

        private void HighlightSkillRange(Character character, SkillObject skill)
        {
            highlightController.ClearHighlightedTiles();
            List<(int, int)> areaTilesPositions = rangeCalculator.GetRange(SkillArea.Cross, skill.range, character.Position);
            highlightController.HighlightTiles(areaTilesPositions, HighlightColor.Black);
        }

        private void HighlightSkillAreaOnSelectedTile(List<(int, int)> skillRangeArea, Character character, SkillObject skill)
        {
            if (!NewValidPlayerInputAvailable(skillRangeArea)) { return; }

            highlightController.ClearHighlightedTiles();
            highlightController.HighlightTiles(skillRangeArea, HighlightColor.Black);

            List<(int, int)> selectedSkillArea = rangeCalculator.GetRange(skill.skillArea, skill.areaSize, selectedPosition);
            highlightController.HighlightTiles(selectedSkillArea, HighlightColor.White);
            playerSkillExecution = CreateSkillExecution(character, skill, selectedPosition);
        }

        private SkillExecution CreateSkillExecution(Character character, SkillObject skill, (int, int) position)
        {
            List<(int, int)> selectedSkillArea = rangeCalculator.GetRange(skill.skillArea, skill.areaSize, position);
            return new SkillExecution(character, skill, charactersController.GetCharactersInRange(selectedSkillArea));
        }

        private bool NewValidPlayerInputAvailable(List<(int, int)> availableArea)
        {
            GameObject playerInput = inputController.SelectedTileGo();
            if (playerInput == null || !NewTileSelected(playerInput))
            {
                return false;
            }

            selectedPosition = charactersController.GameObjectCurrentPosition(playerInput);
            return availableArea.Contains(selectedPosition);
        }

        private bool NewTileSelected(GameObject playerInput)
        {
            (int, int) currentPosition = charactersController.GameObjectCurrentPosition(playerInput);
            return selectedPosition != currentPosition;
        }

        private void HighlightSkillOnSelect(SkillObject skillObject)
        {
            HighlightSkillRange(charactersController.PlayerCharacter(playerId), skillObject);
        }
    }
}

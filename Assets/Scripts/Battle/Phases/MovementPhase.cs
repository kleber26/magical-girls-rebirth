using System;
using System.Collections.Generic;
using Characters;
using Characters.Input;
using Characters.View;
using Highlight;
using Highlight.View;
using Players;
using Range;
using Skills.View;
using UnityEngine;
using World;

namespace Battle.Phases
{
    public class MovementPhase : MonoBehaviour
    {
        private CharactersController charactersController;
        private RangeCalculator rangeCalculator;
        private PlayerController playerController;
        private HighlightController highlightController;
        private InputController inputController;

        private bool movementPhase;
        private GameObject selectedTileGo;
        private List<(int, int)> playerCurrentRange;
        public bool SelectedTileIsNull => selectedTileGo == null;

        public void Initialize(CharactersController charactersController, MapController mapController,
            PlayerController playerController, HighlightController highlightController, InputController inputController)
        {
            this.charactersController = charactersController;
            this.playerController = playerController;
            this.highlightController = highlightController;
            this.inputController = inputController;

            rangeCalculator = new RangeCalculator(mapController);
            movementPhase = false;
        }

        void Update()
        {
            if (!movementPhase)
            {
                return;
            }

            HighlightPlayerMovementPhase();
        }

        public void HighlightTilesFromCharacterRange()
        {
            movementPhase = true;
            selectedTileGo = null;
            int mainPlayerId = playerController.MainPlayer().id;
            Character character = charactersController.PlayerCharacter(mainPlayerId);
            int stamina = character.Stamina;

            playerCurrentRange = rangeCalculator.GetRange(SkillArea.Cross, stamina, character.Position);
            List<(int, int)> validMovementRange = GetValidTilesToMove(playerCurrentRange);
            highlightController.HighlightTiles(validMovementRange, HighlightColor.Black);
        }

        public void ClearHighlightedTiles()
        {
            movementPhase = false;
            playerCurrentRange.Clear();
            highlightController.ClearHighlightedTiles();
        }

        public (int, int) SelectedTilePosition()
        {
            if (selectedTileGo == null)
            {
                throw new NullReferenceException("The selected tile Go is null. You should call this method after assuring its presence.");
            }

            return charactersController.GameObjectCurrentPosition(selectedTileGo);
        }

        private void HighlightPlayerMovementPhase()
        {
            GameObject playerInput = inputController.SelectedTileGo();

            if (playerInput == null) { return; }
            if (!NewTileSelected(playerInput)) { return; }

            (int, int) selectedTilePosition = charactersController.GameObjectCurrentPosition(playerInput);
            if (!playerCurrentRange.Contains(selectedTilePosition)) { return; }

            selectedTileGo = playerInput;
            List<(int, int)> validMovementRange = GetValidTilesToMove(playerCurrentRange);
            HighlightPlayerTiles(validMovementRange, selectedTilePosition);
        }

        private bool NewTileSelected(GameObject playerInput)
        {
            return playerInput?.GetInstanceID() != selectedTileGo?.GetInstanceID();
        }

        private void HighlightPlayerTiles(List<(int, int)> playerRange, (int, int) selectedTilePosition)
        {
            highlightController.HighlightTiles(playerRange, HighlightColor.Black);

            List<(int, int)> selectedPosition = new List<(int, int)> { selectedTilePosition };
            if (GetValidTilesToMove(selectedPosition).Count == 0)
            {
                return;
            }

            highlightController.HighlightTiles(selectedPosition, HighlightColor.White);
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

using System.Collections.Generic;
using Characters;
using Characters.View;
using Highlight.View;
using Players;
using Range;
using ScriptableObjects.Skills;
using UnityEngine;
using World;

namespace Highlight
{
    public class HighlightController
    {
        private TileHighlight tileHighlight;
        private PlayerController playerController;
        MapController mapController;

        public HighlightController(MapController mapController, TileHighlight tileHighlight)
        {
            this.tileHighlight = tileHighlight;
            this.mapController = mapController;
        }

        public void HighlightTiles(List<(int, int)> tilesPosition, HighlightColor highlightColor)
        {
            List<GameObject> tilesGo = mapController.GetTilesGameObjects(tilesPosition);
            tileHighlight.HighlightTiles(tilesGo, highlightColor);
        }

        public void ClearHighlightedTiles()
        {
            tileHighlight.ClearHighlightedTiles();
        }
    }
}

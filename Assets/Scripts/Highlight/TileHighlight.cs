using System.Collections.Generic;
using cakeslice;
using Highlight.View;
using UnityEngine;

namespace Highlight
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;

        private List<GameObject> highlightedTiles;

        private void Awake()
        {
            highlightedTiles = new List<GameObject>();
        }

        public void HighlightTiles(List<GameObject> tilesGo, HighlightColor highlightColor)
        {
            // ClearHighlightedTiles();

            foreach (GameObject tileGo in tilesGo)
            {
                tileGo.GetComponent<Outline>().enabled = true;
                tileGo.GetComponent<Outline>().color = (int) highlightColor;
            }

            highlightedTiles.AddRange(tilesGo);
        }

        public void InstantiatePathFound((int, int) position)
        {
            GameObject tileGO = Instantiate(tilePrefab, new Vector3(position.Item1, 1, position.Item2), Quaternion.identity);
            tileGO.name = $"PATH {position}";
            Destroy(tileGO, 1.5f);
        }

        public void ClearHighlightedTiles()
        {
            foreach (GameObject tileGo in highlightedTiles)
            {
                tileGo.GetComponent<Outline>().enabled = false;
            }

            highlightedTiles.Clear();
        }
    }
}

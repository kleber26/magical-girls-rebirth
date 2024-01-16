using System.Collections.Generic;
using System.Linq;
using Battle;
using Characters;
using Characters.Input;
using Characters.View;
using Highlight;
using Players;
using ScriptableObjects;
using Skills;
using World;
using UnityEngine;
using UnityEngine.SceneManagement;
using World.View;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Texture2D mapImg;
        [SerializeField] private MapForge mapForge;
        [SerializeField] private ColorTerrain colorTerrain;
        [SerializeField] private TerrainObject terrainObject;
        [SerializeField] private CharactersPosition charactersPosition;
        [SerializeField] private InputController inputController;
        [SerializeField] private TileHighlight tileHighlight;
        [SerializeField] private SkillsInitializer skillsInitializer;
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private CharactersCanvasUpdater charactersCanvasUpdater;

        private MapController mapController;
        private CharactersController charactersController;
        private PlayerController playerController;
        private CharactersInitializer charactersInitializer;
        private GameObject selectedTileGo;

        private EventController eventController;
        private HighlightController highlightController;

        void Start()
        {
            Dictionary<Color32, TerrainType> terrainColors = colorTerrain.terrainColors
                .ToList()
                .ToDictionary(ct => ct.color, ct => ct.terrain);

            Dictionary<TerrainType, GameObject[]> terrainGOs = terrainObject.terrainPrefabs
                .ToList()
                .ToDictionary(ct => ct.terrain, ct => ct.terrainGOs);

            mapController = new MapController(mapImg, terrainColors, terrainGOs, mapForge);

            // FIXME: We will use an instance of PlayerController
            playerController = FindObjectOfType<DDOL>().PlayerController;
            charactersInitializer = new CharactersInitializer();
            charactersController = new CharactersController(charactersInitializer, mapController, charactersPosition, playerController.Players, playerController);
            charactersCanvasUpdater.Initialize(charactersController);

            highlightController = new HighlightController(mapController, tileHighlight);

            eventController = new EventController(mapController);
            battleManager.Initialize(mapController, playerController, highlightController, charactersController, inputController, eventController, skillsInitializer);
        }

        // FIXME: This is for testing only.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                List<(int, int)> borderTiles = mapController.GetBorderTiles();
                mapController.DropTiles(borderTiles);
            }
        }

        void OnDrawGizmos()
       {
           if (mapController is null) { return; }

           List<(int, int)> nonVoidMapPositions = mapController.GetNonVoidTilePositions();

           foreach ((int, int) position in nonVoidMapPositions)
           {
               Gizmos.color = charactersController.IsCharacterOccupyingPosition(position) ? Color.red : Color.white;
               Vector3 worldPosition = new Vector3(position.Item1, 0, position.Item2);
               Gizmos.DrawCube(worldPosition, Vector3.one);
           }
       }
    }
}

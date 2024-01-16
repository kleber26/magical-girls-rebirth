using System.Collections.Generic;
using System.Linq;
using Players;
using ScriptableObjects.MapEventObjects;
using UnityEngine;
using UnityEngine.UI;
using World;
using World.View;
using Random = System.Random;

namespace Battle.Phases
{
    public class EventVotingPhase : MonoBehaviour
    {
        [SerializeField] private GameObject eventCardPrefab;
        [SerializeField] private GameObject eventVotingPhaseGameObject;
        [SerializeField] private GameObject eventVotingLayoutGroup;
        [SerializeField] private GameObject eventWinnerLayoutGroup;

        [SerializeField] private List<MapEventObject> mapEventObjects;
        [SerializeField] private MapEventObject lastMapEvent;

        [SerializeField] private int totalEventCards;

        private MapEventObject selectedMapEvent;
        private MapEventObject[] possibleMapEvents;

        private PlayerController playerController;
        private EventController eventController;

        public MapEventObject WinnerEvent { get; private set; }

        public void Initialize(PlayerController playerController, EventController eventController)
        {
            this.playerController = playerController;
            this.eventController = eventController;
        }

        public void SelectMapEvent(MapEventObject mapEvent)
        {
            selectedMapEvent = mapEvent;
        }

        public int GetNumberOfAvailableEvents()
        {
            return mapEventObjects.Count;
        }

        public void SetupEventVotingLayout()
        {
            eventVotingLayoutGroup.SetActive(true);
            eventWinnerLayoutGroup.SetActive(false);

            SetupEventVotingCards();
        }

        public void SetupWinnerEventLayout()
        {
            eventVotingLayoutGroup.SetActive(false);
            eventWinnerLayoutGroup.SetActive(true);

            if (mapEventObjects.Count == 0)
            {
                SetupEventWinnerCard(lastMapEvent);
            }
            else
            {
                SetupEventWinnerCard(WinnerEvent);
                mapEventObjects.Remove(WinnerEvent);
            }
        }

        public void ShowEventVotingPhaseLayout()
        {
            eventVotingPhaseGameObject.SetActive(true);
        }

        public void HideEventVotingPhaseLayout()
        {
            eventVotingPhaseGameObject.SetActive(false);
        }

        public void CalculateVotes()
        {
            if (mapEventObjects.Count == 0)
            {
                WinnerEvent = lastMapEvent;
                return;
            }

            Random random = new Random();
            Dictionary<MapEventObject, int> votes = SetupVoteCards();

            if (selectedMapEvent && votes.ContainsKey(selectedMapEvent))
            {
                votes[selectedMapEvent]++;
            }

            for (int i = 0; i < playerController.Players.Count - 1; i++)
            {
                MapEventObject votedEvent = possibleMapEvents[random.Next(TotalEventCards())];
                votes[votedEvent]++;
            }

            WinnerEvent = votes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        public void DestroyEventCards()
        {
            for (int i = 0; i < eventVotingLayoutGroup.transform.childCount; i++)
            {
                Destroy(eventVotingLayoutGroup.transform.GetChild(i).gameObject);
            }

            if (eventWinnerLayoutGroup.transform.childCount > 0)
            {
                Destroy(eventWinnerLayoutGroup.transform.GetChild(0).gameObject);
            }
        }

        public void CallWinnerEvent()
        {
            eventController.CallEvent(WinnerEvent.mapEvent);
        }

        private Dictionary<MapEventObject, int> SetupVoteCards()
        {
            Dictionary<MapEventObject, int> mapEventDictionary = new Dictionary<MapEventObject, int>();

            foreach (MapEventObject mapEvent in possibleMapEvents)
            {
                mapEventDictionary.Add(mapEvent, 0);
            }

            return mapEventDictionary;
        }

        private void SetupEventVotingCards()
        {
            possibleMapEvents = SetupMapEventArray();

            for (int i = 0; i < possibleMapEvents.Length; i++)
            {
                MapEventObject mapEvent = possibleMapEvents[i];

                GameObject buttonObject = Instantiate(eventCardPrefab, Vector3.zero, Quaternion.identity);
                Button button = buttonObject.GetComponentInChildren<Button>();

                buttonObject.transform.SetParent(eventVotingLayoutGroup.transform, false);
                button.image.sprite = mapEvent.mapEventSprite;

                Text buttonText = button.transform.Find("Text").GetComponent<Text>();
                buttonText.text = mapEvent.mapEvent.ToString();

                button.onClick.AddListener(() => SelectMapEvent(mapEvent));
            }
        }

        private void SetupEventWinnerCard(MapEventObject winnerEvent)
        {
            GameObject winnerEventCard = Instantiate(eventCardPrefab, Vector3.zero, Quaternion.identity);
            Button button = winnerEventCard.GetComponentInChildren<Button>();

            winnerEventCard.transform.SetParent(eventWinnerLayoutGroup.transform, false);
            button.image.sprite = winnerEvent.mapEventSprite;
            button.interactable = false;

            Text buttonText = button.transform.Find("Text").GetComponent<Text>();
            buttonText.text = winnerEvent.mapEvent.ToString();
        }

        private MapEventObject[] SetupMapEventArray()
        {
            Random random = new Random();
            int totalNumberOfCards = TotalEventCards();
            int[] mapEventOrder = Enumerable.Range(0, mapEventObjects.Count).OrderBy(n => random.Next()).Take(totalNumberOfCards).ToArray();

            MapEventObject[] mapEventCards = new MapEventObject[totalNumberOfCards];
            for (int i = 0; i < totalNumberOfCards; i++)
            {
                mapEventCards[i] = mapEventObjects[mapEventOrder[i]];
            }

            return mapEventCards;
        }

        private int TotalEventCards()
        {
            return totalEventCards <= mapEventObjects.Count ? totalEventCards : mapEventObjects.Count;
        }
    }
}

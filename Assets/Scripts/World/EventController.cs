using System;
using System.Collections.Generic;
using System.Linq;
using World.View;
using Randomizer;

namespace World
{
    public class EventController
    {
        private MapController mapController;
        private Dictionary<MapEvent, Action> eventDictionary;
        private Random random;

        public EventController(MapController mapController)
        {
            this.mapController = mapController;
            eventDictionary = SetupStates();
            random = new Random();
        }

        public void CallEvent(MapEvent chosenEvent)
        {
            eventDictionary[chosenEvent].Invoke();
        }

        private Dictionary<MapEvent, Action> SetupStates()
        {
            return new Dictionary<MapEvent, Action>
            {
                [MapEvent.Shift] = Shift,
                [MapEvent.Carousel] = Carousel,
                [MapEvent.LavaFloor] = LavaFloor,
                [MapEvent.FloodGate] = FloodGate,
            };
        }

        private void Shift()
        {
            int minLine = 11;
            int maxLine = 28;

            List<Action<int>> shifts = CreateListOfShifts();
            int numberOfShifts = 1;
            Action<int> randomShift = shifts[random.Next(shifts.Count)];

            int shiftedPlace = random.Next(minLine, maxLine);
            randomShift.Invoke(shiftedPlace);
        }

        private List<Action<int>> CreateListOfShifts()
        {
            return new List<Action<int>>()
            {
                mapController.ShiftColumnDown,
                mapController.ShiftColumnUp,
                mapController.ShiftLineLeft,
                mapController.ShiftLineRight
            };
        }

        private void Carousel()
        {
            int minCarouselSize = 2;
            int maxCarouselSize = 5;

            int numberOfCarousels = 1;

            List <(int, int)> availablePositions = mapController.GetNonVoidTilePositions().
                Where(a => a.Item1 > 10 && a.Item1 < 30 && a.Item2 > 10 && a.Item2 < 30).
                ToList();

            (int, int) position = availablePositions[random.Next(availablePositions.Count)];
            int carouselSize = random.Next(minCarouselSize, maxCarouselSize);
            mapController.Carousel(position, carouselSize);
        }

        private void LavaFloor()
        {
            int minPercentage = 5;
            int maxPercentage = 10;
            int numberOfLavaCells = GetRandomNumberOfCells(minPercentage, maxPercentage);

            mapController.LavaFloorEvent(numberOfLavaCells);
        }

        private void FloodGate()
        {
            int minPercentage = 5;
            int maxPercentage = 15;
            int numberOfWaterCells = GetRandomNumberOfCells(minPercentage, maxPercentage);

            mapController.FloodGateEvent(numberOfWaterCells);
        }

        private int GetRandomNumberOfCells(int minPercentage, int maxPercentage)
        {
            int numberOfCells = mapController.GetNonVoidTilePositions().Count;
            int minCells = (numberOfCells * minPercentage) / 100;
            int maxCells = (numberOfCells * maxPercentage) / 100;

            return random.Next(minCells, maxCells);
        }
    }
}

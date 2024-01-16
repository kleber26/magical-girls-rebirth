using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Phases;
using Battle.View;
using Characters;
using Characters.Input;
using Characters.View;
using Game;
using Highlight;
using Players;
using Skills;
using tetryds.Tools;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private Text phase;
        [SerializeField] private Text timer;
        [SerializeField] private MovementPhase movementPhase;
        [SerializeField] private MovementStartPhase movementStartPhase;
        [SerializeField] private EventVotingPhase eventVotingPhase;
        [SerializeField] private BattlePhase battlePhase;
        [SerializeField] private GameOverPhase gameOverPhase;
        [SerializeField] private BattleStartPhase battleStartPhase;
        [SerializeField] private CameraManager cameraManager;

        private Dictionary<BattleState, int> statesDurantion;
        StateMachine<BattleState, string, Action> stateMachine;

        private BattleState CurrentState => stateMachine.Current;
        private int currentTurnNumber;
        private int turnsBetweenEvents = 3;

        MapController mapController;
        CharactersController charactersController;
        PlayerController playerController;
        void Start()
        {
            statesDurantion = SetupStatesDuration();
            stateMachine = CreateStateMachine();
            phase.text = "";
            stateMachine.Behavior.Invoke();

            currentTurnNumber = 0;
        }

        public void Initialize(MapController mapController, PlayerController playerController, HighlightController highlightController,
            CharactersController charactersController, InputController inputController, EventController eventController,
            SkillsInitializer skillsInitializer)
        {
            movementPhase.Initialize(charactersController, mapController, playerController,
                highlightController, inputController);
            movementStartPhase.Initialize(mapController, charactersController, playerController);
            eventVotingPhase.Initialize(playerController, eventController);
            battlePhase.Initialize(playerController, mapController, highlightController, charactersController, inputController,
                skillsInitializer, playerController.MainPlayer().id);
            battleStartPhase.Initialize(charactersController);
            gameOverPhase.Initialize(charactersController, playerController);

            Character mainCharacter = charactersController.PlayerCharacter(playerController.MainPlayer().id);
            cameraManager.Initialize(charactersController.GetCharacterGameObject(mainCharacter), mapController.MiddleMapPosition());
            charactersController.RemovelayerFlag();
            this.mapController = mapController;
            this.charactersController = charactersController;
            this.playerController = playerController;
        }

        private Dictionary<BattleState, int> SetupStatesDuration()
        {
            return new Dictionary<BattleState, int>
            {
                [BattleState.GameStart] = 1,
                [BattleState.Movement] = 5,
                [BattleState.MovementStart] = 0,
                [BattleState.Battle] = 5,
                [BattleState.BattleStart] = 2,
                [BattleState.DamageCalculation] = 2,
                [BattleState.EventVote] = 5,
                [BattleState.EventWinner] = 3,
                [BattleState.EventStart] = 3,
                [BattleState.GameOver] = 0
            };
        }

        StateMachine<BattleState, string, Action> CreateStateMachine()
        {
            stateMachine = new StateMachine<BattleState, string, Action>(BattleState.GameStart, () => GameStart());

            stateMachine
                .AddState(BattleState.Movement)
                .AddState(BattleState.MovementStart)
                .AddState(BattleState.Battle)
                .AddState(BattleState.BattleStart)
                .AddState(BattleState.DamageCalculation)
                .AddState(BattleState.EventVote)
                .AddState(BattleState.EventWinner)
                .AddState(BattleState.EventStart)
                .AddState(BattleState.GameOver)
                .AddTransition("start", BattleState.GameStart, BattleState.Movement, () => Movement())
                .AddTransition("next", BattleState.Movement, BattleState.MovementStart, () => MovementStart())
                .AddTransition("next", BattleState.MovementStart, BattleState.Battle, () => Battle())
                .AddTransition("next", BattleState.Battle, BattleState.BattleStart, () => BattleStart())
                .AddTransition("next", BattleState.BattleStart, BattleState.DamageCalculation, () => DamageCalculation())
                .AddTransition("next", BattleState.DamageCalculation, BattleState.EventVote, () => EventVote())
                .AddTransition("next", BattleState.EventVote, BattleState.EventWinner, () => EventWinner())
                .AddTransition("skip", BattleState.EventVote, BattleState.Movement, () => Movement())
                .AddTransition("next", BattleState.EventWinner, BattleState.EventStart, () => EventStart())
                .AddTransition("next", BattleState.EventStart, BattleState.Movement, () => Movement())
                .AddTransition("gameOver", BattleState.EventStart, BattleState.GameOver, () => GameOver())
                .AddTransition("gameOver", BattleState.MovementStart, BattleState.GameOver, () => GameOver())
                .AddTransition("gameOver", BattleState.DamageCalculation, BattleState.GameOver, () => GameOver());

            return stateMachine;
        }

        void GameStart()
        {
            Debug.Log("---- Game Start");
            gameOverPhase.HideGameOverScreen();
            cameraManager.SetMapView();
            StartCoroutine(Countdown("start"));
        }

        void Movement()
        {
            Debug.Log("---- Game Movement");
            phase.text = "MOVEMENT PHASE";
            eventVotingPhase.HideEventVotingPhaseLayout();
            eventVotingPhase.DestroyEventCards();
            cameraManager.SetInteractiveView();

            movementPhase.HighlightTilesFromCharacterRange();
            StartCoroutine(Countdown("next"));
        }

        void MovementStart()
        {
            Debug.Log("---- Game MovementStart");
            cameraManager.SetCharacterView();
            phase.text = "";
            movementPhase.ClearHighlightedTiles();

            if (!movementPhase.SelectedTileIsNull)
            {
                (int, int) selectedPosition = movementPhase.SelectedTilePosition();
                movementStartPhase.MoveCharacters(selectedPosition);
            }

            movementStartPhase.MoveBots();
            StartCoroutine(WaitPlayersMovement("next"));
        }

        void Battle()
        {
            Debug.Log("---- Battle");
            phase.text = "ATTACK!";
            cameraManager.SetInteractiveView();
            battlePhase.SetupBattle();
            StartCoroutine(Countdown("next"));
        }

        void BattleStart()
        {
            Debug.Log("---- BattleStart");

            cameraManager.SetCharacterView();
            phase.text = "";
            List<SkillExecution> battleSkills = battlePhase.TierDownBattle();
            battleStartPhase.TriggerCharacterAnimation(battleSkills);
            StartCoroutine(WaitBattleStartAnimations("next"));
        }

        void DamageCalculation()
        {
            Debug.Log("---- DamageCalculation");
            if (currentTurnNumber <= 10)
            {
                List<(int, int)> borderTiles = mapController.GetBorderTiles();
                mapController.DropTiles(borderTiles);

                List<Character> charsOnBorder = new List<Character>();
                foreach (var character in charactersController.GetAliveCharacters())
                {
                    if (borderTiles.Contains(character.Position))
                    {
                        character.CurrentLife = 0;
                        charsOnBorder.Add(character);
                        GameObject charGo = charactersController.GetCharacterGameObject(character);
                        charGo.AddComponent<Rigidbody>();
                        battleStartPhase.CheckCharactersDeath(new List<Character> { character } );
                    }
                }
            }

            Debug.Log(string.Join(charactersController.GetAliveCharacters().Count + " alive: " + ", ", charactersController.GetAliveCharacters()));
            StartCoroutine(WaitDamageCalculation());
        }

        void EventVote()
        {
            Debug.Log("---- EventVote");
            charactersController.RemoveGamePlayers();
            phase.text = "NEXT EVENT";
            cameraManager.SetMapView();
            currentTurnNumber++;

            eventVotingPhase.ShowEventVotingPhaseLayout();
            eventVotingPhase.SetupEventVotingLayout();

            if (eventVotingPhase.GetNumberOfAvailableEvents() <= 1)
            {
                stateMachine?.RaiseEvent("next");
                return;
            }

            StartCoroutine(Countdown("next"));
        }

        void EventWinner()
        {
            Debug.Log("---- Game Event Winner");
            phase.text = "";
            eventVotingPhase.CalculateVotes();
            eventVotingPhase.SetupWinnerEventLayout();
            StartCoroutine(Countdown("next"));
        }

        void EventStart()
        {
            Debug.Log("---- EventStart");
            eventVotingPhase.CallWinnerEvent();
            charactersController.CheckCharactersOnVoidTiles(mapController, battleStartPhase);
            StartCoroutine(WaitEventAnimation());
        }

        void GameOver()
        {
            Debug.Log("---- GameOver");
            eventVotingPhase.HideEventVotingPhaseLayout();

            gameOverPhase.SetupGameOverPopup();
            gameOverPhase.ShowGameOverScreen();
            playerController.AddGamePlayers();
        }

        private IEnumerator Countdown(string eventName)
        {
            int seconds = statesDurantion[stateMachine.Current];
            while (seconds > 0)
            {
                if(stateMachine.Current == BattleState.Movement || stateMachine.Current == BattleState.Battle || stateMachine.Current == BattleState.EventVote)
                {
                    timer.text = seconds.ToString();
                }
                else
                {
                    timer.text = "";
                }

                yield return new WaitForSeconds(1);
                seconds--;
            }
            timer.text = "";

            stateMachine?.RaiseEvent(eventName);
        }

        private IEnumerator WaitPlayersMovement(string eventName)
        {
            while (!movementStartPhase.CharactersFinishedMoving())
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);
            stateMachine?.RaiseEvent(eventName);
        }

        private IEnumerator WaitBattleStartAnimations(string eventName)
        {
            // Bad way of waiting all animations to start
            yield return new WaitForSeconds(1f);

            battleStartPhase.InflictDamageAfterSkillExecution();
            while (!battleStartPhase.CharactersFinishedAttacking())
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            while (!battleStartPhase.CharactersFinishedGettingHit())
            {
                yield return null;
            }

            stateMachine?.RaiseEvent(eventName);
        }

        private IEnumerator WaitDamageCalculation()
        {
            yield return null;
            stateMachine?.RaiseEvent(gameOverPhase.IsGameOver() ? "gameOver" : "next");
        }

        private IEnumerator WaitEventAnimation()
        {
            yield return null;
            stateMachine?.RaiseEvent(gameOverPhase.IsGameOver() ? "gameOver" : "next");
        }
    }
}

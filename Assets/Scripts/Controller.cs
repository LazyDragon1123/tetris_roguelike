using UnityEngine;

public class Controller : MonoBehaviour {
    public Board board;
    public Ghost ghost;
    public Ready ready;
    public GameModifier gameModifier;

    private void Start() {
        ready.onReadyComplete += GameStart;  // Subscribe to the event
        ready.Initialize();
    }

    private void GameStart() {
        board.Initialize();
        ghost.Initialize();
        gameModifier.Initialize(board, ghost);

        board.SpawnPiece();
        board.StartBossPhase();
    }

    private void OnDestroy() {
        ready.onReadyComplete -= GameStart;  // Unsubscribe to avoid memory leaks
    }
}

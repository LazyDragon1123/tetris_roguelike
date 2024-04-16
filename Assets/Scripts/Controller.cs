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
        gameModifier.Initialize();
        board.Initialize(gameModifier);
        ghost.Initialize();
        board.SpawnPiece();
    }

    private void OnDestroy() {
        ready.onReadyComplete -= GameStart;  // Unsubscribe to avoid memory leaks
    }
}

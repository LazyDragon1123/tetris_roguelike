using UnityEngine;
using UnityEngine.Tilemaps;

public class FreeFallPiece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public TetrominoBossData data { get; private set; }
    public TileProperty[] tileProperties;
    public Vector3Int position { get; private set; }

    private float stepDelay = 0.2f;
    private float lockDelay = 0.1f;
    public float probAttackagle = 1.0f;
    public Tile[] tiles { get; private set; }
    public Tile[] lockedTiles { get; private set; }
    private float stepTime;
    private float lockTime;
    private bool isInitialized = false;
    // private GameManager gameManager;


    

    public void Initialize(Board board, Vector3Int position, TetrominoBossData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;

        stepTime = Time.time + stepDelay;
        lockTime = 0f;
        isInitialized = true;

        if (cells == null) {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }

        SetTileProperties();
        SetTiles();
    }
    private void SetTiles()
    {
        tiles = new Tile[cells.Length];
        lockedTiles = new Tile[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            tiles[i] = data.tile;
            lockedTiles[i] = data.lockedTile;
        }
    }
    private void SetTileProperties()
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        tileProperties = InitTilePropertiesArray(cells.Length);
        if (randomValue <= probAttackagle)
        {
            int index = Random.Range(0, cells.Length);
            tileProperties[index].isAttackable = true;
        }
    }

    private TileProperty[] InitTilePropertiesArray(int length)
        {
        TileProperty[] initTileProperties = new TileProperty[length];
        
        for (int i = 0; i < length; i++)
        {
            initTileProperties[i] = new TileProperty {};
        }
        return initTileProperties;
    }    
    private void Update()
    {
        if (GameManager.isGamePaused) return;
        if (!isInitialized) return;
        // board.SoftClearBoss(this);

        // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        lockTime += Time.deltaTime;


        // Advance the piece to the next row every x seconds
        if (Time.time > stepTime) {
            board.SoftClearBoss(this);
            Step();
            board.SoftSetBoss(this);
        }

        // board.SoftSetBoss(this);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        // Step down to the next row
        Move(Vector2Int.down);

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay) {
            Lock();
        }
    }

    private void Lock()
    {
        board.SetBoss(this);
        board.ClearLines();
        board.SpawnBossCell();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPositionBoss(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            position = newPosition;
            lockTime = 0f; // reset
        }

        return valid;
    }
}


using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TilePropertyMap propertyMap { get; private set; }
    public Piece activePiece;
    

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public float minStepDelay = 0.1f;
    public float speedIncreaseFactor = 0.1f;
    public TetrominoHolder tetrominoHolder;

    public UpcomingTetrominos upcomingTetrominos;
    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    public ScoreBoard scoreBoard;
    public SpecialCellsCounter specialCellsCounter;
    public int Score = 0;

    public Tile bossTile;    // Assign this in the Unity Editor with a specific tile for the boss phase
    private Vector3Int currentBossCellPosition;

    public void Initialize()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        propertyMap = GetComponentInChildren<TilePropertyMap>();
        activePiece = GetComponentInChildren<Piece>();
        scoreBoard = GetComponentInChildren<ScoreBoard>();
        scoreBoard.UpdateScore(Score);
        scoreBoard.UpdateSpeed(activePiece.stepDelay);

        tetrominoHolder = GetComponentInChildren<TetrominoHolder>();
        tetrominoHolder.Initialize(this);
        upcomingTetrominos = GetComponentInChildren<UpcomingTetrominos>();
        upcomingTetrominos.Initialize(this);

        specialCellsCounter = GetComponentInChildren<SpecialCellsCounter>();
        

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
        InitializeUpcomingTetrominos();
    }
    private void InitializeUpcomingTetrominos()
    {
        // Preload the upcoming Tetrominos at the start of the game
        for (int i = 0; i < upcomingTetrominos.previewCount; i++)
        {
            upcomingTetrominos.AddNewTetromino(tetrominoes[Random.Range(0, tetrominoes.Length)]);
        }
    }

    public void SpawnPiece()
    {
        SetPiece(upcomingTetrominos.GetNextTetromino());
        upcomingTetrominos.AddNewTetromino(tetrominoes[Random.Range(0, tetrominoes.Length)]);
    }

    public void SetPiece(TetrominoData data)
    {
        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition)) {
            Set(activePiece);
        } else {
            GameOver();
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        propertyMap.ClearAllTiles();

        // Do anything else you want on game over here..
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.tiles[i]);
            propertyMap.SetTile(tilePosition, piece.tileProperties[i]);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;
        // int specialCellCleared = 0;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (IsSpecialCell(position))
        {
            specialCellsCounter.AddSpecialCell();
        }
            tilemap.SetTile(position, null);
            propertyMap.SetTile(position, new TileProperty { isSpecial = true });
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
        Score += 100;
        scoreBoard.UpdateScore(Score);
    }

    private bool IsSpecialCell(Vector3Int position)
    {
        return propertyMap.IsTileSpecial(position);
    }

    // Method to increase the falling speed of the active piece
    public void IncreaseFallingSpeed()
    {
        // Increase the speed by reducing the delay
        activePiece.stepDelay = Mathf.Max(minStepDelay, activePiece.stepDelay - speedIncreaseFactor);
        // Update the UI if necessary
        scoreBoard.UpdateSpeed(activePiece.stepDelay); // Assuming you have this method in scoreBoard
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            tetrominoHolder.SwapOrHoldTetromino();
        }
    }
    public void StartBossPhase()
    {
        StartCoroutine(BossPhaseRoutine());
    }
    IEnumerator BossPhaseRoutine()
    {
        while (true)  // Loop to continuously spawn boss cells
        {
            SpawnBossCell();
            yield return new WaitUntil(() => MoveBossCellDown());  // Wait until the cell cannot move down anymore
        }
    }

    void SpawnBossCell()
    {
        int randomColumn = Random.Range(0, tilemap.size.x);
        currentBossCellPosition = new Vector3Int(randomColumn, tilemap.cellBounds.max.y - 1, 0);
        tilemap.SetTile(currentBossCellPosition, bossTile);
    }

    bool MoveBossCellDown()
    {
        Vector3Int newPosition = new Vector3Int(currentBossCellPosition.x, currentBossCellPosition.y - 1, 0);

        if (!tilemap.HasTile(newPosition) && IsWithinBounds(newPosition))
        {
            tilemap.SetTile(currentBossCellPosition, null);  // Clear the current position
            tilemap.SetTile(newPosition, bossTile);           // Set tile at the new position
            currentBossCellPosition = newPosition;            // Update the current boss cell position
            return true;
        }
        return false;  // Return false when the boss cell can't move down anymore
    }

    bool IsWithinBounds(Vector3Int position)
    {
        return position.y >= tilemap.cellBounds.min.y;
    }
}


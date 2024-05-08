using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TilePropertyMap propertyMap { get; private set; }
    public Piece activePiece;
    public FreeFallPiece bossPiece;
    

    public TetrominoData[] tetrominoes;
    public TetrominoBossData[] tetrominoBosses;
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
    public BossHpCounter bossHpCounter;
    public int Score = 0;

    private bool isBossPhase = false;
    private Vector3Int currentBossCellPosition;
    public BossBoard bossBoard;
    public StateTilemap stateTilemap;

    public void Initialize()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        propertyMap = GetComponentInChildren<TilePropertyMap>();
        activePiece = GetComponentInChildren<Piece>();
        bossPiece = GetComponentInChildren<FreeFallPiece>();
        scoreBoard = GetComponentInChildren<ScoreBoard>();
        scoreBoard.UpdateScore(Score);
        scoreBoard.UpdateSpeed(activePiece.stepDelay);
        bossBoard = GetComponentInChildren<BossBoard>();
        stateTilemap = GetComponentInChildren<StateTilemap>();

        tetrominoHolder = GetComponentInChildren<TetrominoHolder>();
        tetrominoHolder.Initialize(this);
        upcomingTetrominos = GetComponentInChildren<UpcomingTetrominos>();
        upcomingTetrominos.Initialize(this);

        specialCellsCounter = GetComponentInChildren<SpecialCellsCounter>();
        bossHpCounter = GetComponentInChildren<BossHpCounter>();
        

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
        for (int i = 0; i < tetrominoBosses.Length; i++) {
            tetrominoBosses[i].Initialize();
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
        stateTilemap.ClearAllTiles();
        propertyMap.ClearAllTiles();
        Score = 0;
        scoreBoard.UpdateScore(Score);
        tetrominoHolder.Initialize(this);
        upcomingTetrominos.Initialize(this);
        InitializeUpcomingTetrominos();
        // Do anything else you want on game over here..
    }

    public void Set(Piece piece, TileState tileState = TileState.Moving)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.tiles[i]);
            stateTilemap.SetTile(tilePosition, tileState);
            propertyMap.SetTile(tilePosition, piece.tileProperties[i]);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
            stateTilemap.SetTile(tilePosition, TileState.NotOccupied);
            propertyMap.SetTile(tilePosition, new TileProperty { isSpecial = false });
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
                StartLineClear(row);
            } else {
                row++;
            }
        }
        specialCellsCounter.UpdateCount();

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

    public void StartLineClear(int row)
    {
        StartCoroutine(LineClearCoroutine(row));
    }

    private IEnumerator LineClearCoroutine(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (IsSpecialCell(position))
            {
                specialCellsCounter.AddSpecialCell();
            }
            if (IsAttackableCell(position))
            {
                bossHpCounter.AddCount();
            }

            tilemap.SetTile(position, null);
            stateTilemap.SetTile(position, TileState.NotOccupied);
            propertyMap.SetTile(position, new TileProperty { isSpecial = false });
        }

        // Add a yield statement to allow other operations to run in the meantime
        yield return null;

        // Shift rows down
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int abovePos = new Vector3Int(col, row + 1, 0);
                if (stateTilemap.HasTile(abovePos, TileState.Locked))
                {
                    TileBase aboveTile = tilemap.GetTile(abovePos);
                    Vector3Int newPos = new Vector3Int(col, row, 0);
                    tilemap.SetTile(newPos, aboveTile);
                    stateTilemap.SetTile(newPos, TileState.Locked);
                    propertyMap.SetTile(newPos, propertyMap.GetTileProperty(abovePos));
                }
            }

            row++;
            yield return null; // Yield control to allow other tasks to run between rows
        }

        // Update score
        Score += 100;
        scoreBoard.UpdateScore(Score);
    }


    private bool IsSpecialCell(Vector3Int position)
    {
        return propertyMap.IsTileSpecial(position);
    }

    private bool IsAttackableCell(Vector3Int position)
    {
        return propertyMap.IsTileAttackable(position);
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
    public void IncreaseHoldCapacity()
    {
        tetrominoHolder.IncreaseCapacity();
    }
    public void StartBossPhase()
    {
        isBossPhase = true;
        SpawnBossCell();
    }
    public void EndBossPhase()
    {
        isBossPhase = false;
    }

    public void SpawnBossCell()
    {
        int randomColumn = Random.Range(-boardSize.x / 2, boardSize.x / 2);
        currentBossCellPosition = new Vector3Int(randomColumn, tilemap.cellBounds.max.y - 1, 0);
        SetBossPiece(tetrominoBosses[Random.Range(0, tetrominoBosses.Length)]);
    }
    private void SetBossPiece(TetrominoBossData data)
    {
        bossPiece.Initialize(this, currentBossCellPosition, data);
        SoftSetBoss(bossPiece);
    }
    public void ClearBoss(FreeFallPiece freeFallPiece)
    {
        for (int i = 0; i < freeFallPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = freeFallPiece.cells[i] + freeFallPiece.position;
            tilemap.SetTile(tilePosition, null);
            stateTilemap.SetTile(tilePosition, TileState.NotOccupied);
            propertyMap.SetTile(tilePosition, new TileProperty { isSpecial = false });
        }
    }
    public void SetBoss(FreeFallPiece freeFallPiece, TileState tileState = TileState.Locked)
    {
        for (int i = 0; i < freeFallPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = freeFallPiece.cells[i] + freeFallPiece.position;
            tilemap.SetTile(tilePosition, freeFallPiece.lockedTiles[i]);
            stateTilemap.SetTile(tilePosition, tileState);
            propertyMap.SetTile(tilePosition, new TileProperty { isSpecial = false });
        }
    }
    public void SoftSetBoss(FreeFallPiece freeFallPiece)
    {
        for (int i = 0; i < freeFallPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = freeFallPiece.cells[i] + freeFallPiece.position;
            bossBoard.SetTile(tilePosition, freeFallPiece.tiles[i]);
            stateTilemap.SetTile(tilePosition, TileState.BossMoving);
            propertyMap.SetTile(tilePosition, new TileProperty { isSpecial = false });
        }
    }
    public void SoftClearBoss(FreeFallPiece freeFallPiece)
    {
        for (int i = 0; i < freeFallPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = freeFallPiece.cells[i] + freeFallPiece.position;
            bossBoard.SetTile(tilePosition, null);
            stateTilemap.SetTile(tilePosition, TileState.NotOccupied);
            propertyMap.SetTile(tilePosition, new TileProperty { isSpecial = false });
        }
    }
    public bool IsValidPositionBoss(FreeFallPiece freeFallPiece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < freeFallPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = freeFallPiece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (stateTilemap.HasTile(tilePosition, TileState.Locked)) {
                return false;
            }
            
        }

        return true;
    }
}


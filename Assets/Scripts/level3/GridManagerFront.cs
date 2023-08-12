using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerFront : MonoBehaviour
{
    public GameObject GameOverMenu; // 2
    // public TMPro.TextMeshProUGUI MovesText;
    public TMPro.TextMeshProUGUI ScoreText;
    public TMPro.TextMeshProUGUI mainTimerDisplay;

    // public int StartingMoves = 50;
    public float StartingTime = 15.0f; // 2
    private float _timeLeft; // 3
    public float TimeLeft
    {
        get
        {
            return _timeLeft;
        }

        set
        {
            _timeLeft = value;
            mainTimerDisplay.text = _timeLeft.ToString("0.00");
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            ScoreText.text = _score.ToString();
        }
    }
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;
    private GameObject[, ,] Grid;
    // Start is called before the first frame update
    public static GridManagerFront Instance { get; private set; } 
    void Awake() 
    { 
        Instance = this;
        Score = 0;
        // NumMoves = StartingMoves;
        TimeLeft=StartingTime;
        GameOverMenu.SetActive(false); 
    }
    void Start()
    {
      Grid = new GameObject[GridDimension+1, GridDimension, GridDimension+1];
      InitGrid();  
    }

    // Update is called once per frame
    void Update()
    {
        FillHoles();
        TimeLeft -= Time.deltaTime;
		
			// don't let it go negative
			if (TimeLeft <= 0.0f)
            {
				TimeLeft = 0.0f;
                GameOver();
            }

			// update the text UI
    }
    void InitGrid()
{
    Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f); // 1
    for (int row = 0; row < GridDimension; row++)
        for (int column = GridDimension; column >0; column--) // 2
        {
            GameObject newTile = Instantiate(TilePrefab); // 3
            SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); // 4
            renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            TileFront tile = newTile.AddComponent<TileFront>();
            tile.Position = new Vector3Int(column, row, 8);
            newTile.transform.parent = transform; // 6
            newTile.transform.position = new Vector3(column * Distance-1, row * Distance, 8) + positionOffset; // 7
                
            Grid[column, row,8] = newTile; // 8
        }
        // Vector3 positionOffsetright = transform.position - new Vector3(0, GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f); // 1
    for (int row = 0; row < GridDimension; row++)
        for (int column = 0; column < GridDimension; column++) // 2
        {
            GameObject newTile = Instantiate(TilePrefab); // 3
            SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); // 4
            renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            TileFront tile = newTile.AddComponent<TileFront>();
            tile.Position = new Vector3Int(8, row, column);
            // tile.transform.Rotate(0,90,0);
            newTile.transform.parent = transform; // 6
            newTile.transform.Rotate(0,-90,0);
            newTile.transform.position = new Vector3(8, row * Distance,column * Distance) + positionOffset; // 7
                
            Grid[8, row, column] = newTile; // 8
        }
        // Vector3 positionOffsetfront = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 7); // 1
    for (int row = 0; row < GridDimension; row++)
        for (int column = 0; column < GridDimension; column++) // 2
        {
            GameObject newTile = Instantiate(TilePrefab); // 3
            SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); // 4
            renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            TileFront tile = newTile.AddComponent<TileFront>();
            tile.Position = new Vector3Int(column, row, 0);
            newTile.transform.parent = transform; // 6
            newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset; // 7
                
            Grid[column, row,0] = newTile; // 8
        }
    for (int row = 0; row < GridDimension; row++)
        for (int column = GridDimension; column > 0; column--) // 2
        {
            GameObject newTile = Instantiate(TilePrefab); // 3
            SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); // 4
            renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            TileFront tile = newTile.AddComponent<TileFront>();
            tile.Position = new Vector3Int(0, row, column);
            // tile.transform.Rotate(0,90,0);
            newTile.transform.parent = transform; // 6
            newTile.transform.Rotate(0,-90,0);
            newTile.transform.position = new Vector3(0, row * Distance,column * Distance-1) + positionOffset; // 7
                
            Grid[0, row, column] = newTile; // 8
        }
}
// public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position) // 1
// {

//     // 2
//     GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
//     SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

//     GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
//     SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

//     // 3
//     Sprite temp = renderer1.sprite;
//     renderer1.sprite = renderer2.sprite;
//     renderer2.sprite = temp;
// }
public SpriteRenderer GetSpriteRendererAt(int x,int y,int z)
{
    GameObject tile1 = Grid[x, y, z];
    SpriteRenderer renderer = tile1.GetComponent<SpriteRenderer>();
    return renderer;
}
void FillHoles()
{
    for (int column = GridDimension; column >0; column--)
    {
        for (int row = 0; row < GridDimension; row++) // 1
        {
            while (GetSpriteRendererAt(column, row,8).sprite == null) // 2
            {
                for (int filler = row; filler < GridDimension - 1; filler++) // 3
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, filler,8); // 4
                    SpriteRenderer next = GetSpriteRendererAt(column, filler + 1,8);
                    current.sprite = next.sprite;
                }
                SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1,8);
                last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            }
        }
    }
    for (int column = 0; column < GridDimension; column++)
    {
        for (int row = 0; row < GridDimension; row++) // 1
        {
            while (GetSpriteRendererAt(8,row,column).sprite == null) // 2
            {
                for (int filler = row; filler < GridDimension - 1; filler++) // 3
                {
                    SpriteRenderer current = GetSpriteRendererAt(8, filler,column); // 4
                    SpriteRenderer next = GetSpriteRendererAt(8, filler + 1,column);
                    current.sprite = next.sprite;
                }
                SpriteRenderer last = GetSpriteRendererAt(8, GridDimension - 1,column);
                last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            }
        }
    }
    for (int column = 0; column < GridDimension; column++)
    {
        for (int row = 0; row < GridDimension; row++) // 1
        {
            while (GetSpriteRendererAt(column, row,0).sprite == null) // 2
            {
                for (int filler = row; filler < GridDimension - 1; filler++) // 3
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, filler,0); // 4
                    SpriteRenderer next = GetSpriteRendererAt(column, filler + 1,0);
                    current.sprite = next.sprite;
                }
                SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1,0);
                last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            }
        }
    }
    for (int column = GridDimension; column > 0; column--)
    {
        for (int row = 0; row < GridDimension; row++) // 1
        {
            while (GetSpriteRendererAt(0,row,column).sprite == null) // 2
            {
                for (int filler = row; filler < GridDimension - 1; filler++) // 3
                {
                    SpriteRenderer current = GetSpriteRendererAt(0, filler,column); // 4
                    SpriteRenderer next = GetSpriteRendererAt(0, filler + 1,column);
                    current.sprite = next.sprite;
                }
                SpriteRenderer last = GetSpriteRendererAt(0, GridDimension - 1,column);
                last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            }
        }
    }
}
void GameOver()
{
    PlayerPrefs.SetInt ("score", Score);
    GameOverMenu.SetActive (true);
    Destroy(gameObject);
    SoundManager.Instance.PlaySound(SoundType.TypeGameOver);
}
}

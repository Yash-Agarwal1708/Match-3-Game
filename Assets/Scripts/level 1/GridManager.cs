using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;
    private GameObject[,] Grid;
    // Start is called before the first frame update
    public static GridManager Instance { get; private set; } 
    void Awake() 
    { 
        Instance = this; 
    }
    void Start()
    {
      Grid = new GameObject[GridDimension, GridDimension];
      InitGrid();  
    }

    // Update is called once per frame
    void Update()
    {
        FillHoles();
    }
    void InitGrid()
{
    Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0); // 1
    for (int row = 0; row < GridDimension; row++)
        for (int column = 0; column < GridDimension; column++) // 2
        {
            GameObject newTile = Instantiate(TilePrefab); // 3
            SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); // 4
            renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            Tile tile = newTile.AddComponent<Tile>();
            tile.Position = new Vector2Int(column, row);
            newTile.transform.parent = transform; // 6
            newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset; // 7
                
            Grid[column, row] = newTile; // 8
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
public SpriteRenderer GetSpriteRendererAt(int x,int y)
{
    GameObject tile1 = Grid[x, y];
    SpriteRenderer renderer = tile1.GetComponent<SpriteRenderer>();
    return renderer;
}
void FillHoles()
{
    for (int column = 0; column < GridDimension; column++)
    {
        for (int row = 0; row < GridDimension; row++) // 1
        {
            while (GetSpriteRendererAt(column, row).sprite == null) // 2
            {
                for (int filler = row; filler < GridDimension - 1; filler++) // 3
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, filler); // 4
                    SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);
                    current.sprite = next.sprite;
                }
                SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1);
                last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
            }
        }
    }
}
}

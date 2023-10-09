using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    // How big the gird is  (how many rows and columns)
    public int rows;
    public int columns;
    public Tile[,] tiles; /*don't get this I know it's accessing the Tile script though*/

    // Tile prefab were going to use to make the grid
    public GameObject tilePrefab; /* making the tile into a gameobject? forgot what prefab was*/

    //Origin tile position , All subsequent tiles will be positioned based on this one
    //Origin tile is [0,0];
    public Vector3 originPos = new Vector3(-3, -3, 0); /*Forgot what Vector2 and Vecotr3 is... if origin is at 0,0 why is it -3,-3 */

    [Range(0, 5)] public int holeCount; /*the max for is 5 tiles? each random map can contain either 1-5 holes/traps? But everytime I restart it the hole is always 5 tiles while the trap is 3 tiles every map not randomized amounts of tiles*/
    [Range(0, 5)] public int trapTileCount;
    [Range(0, 2)] public int teleportCount;

    public List<Tile> trapTiles = new List<Tile>(); /* making seperate lists of all the trap and tiles */
    private List<Tile> inaccessibleTiles = new List<Tile>(); /*why is one list private but one public*/ 
    public List<Tile> teleportTiles = new List<Tile>();


    void Awake()     /* why is this function never called? */
    {
        //Initilize 2D array
        tiles = new Tile[rows, columns]; /* is this making a tile object? */

        //Call code that makes the grid
        MakeGrid();
        
    }

    private void MakeGrid()
    {
        //Nested for loops to create the rows and columns
        for (int c = 0; c < columns; c++) /* what is the maxiumum column and row num if it was never decalred a maximum number? */
        {
            for (int r = 0; r < rows; r++)
            {
                //Here we want to get the size of the Tile sprite so that he can place them side by side
                float sizeX = tilePrefab.GetComponent<SpriteRenderer>().size.x;
                float sizeY = tilePrefab.GetComponent<SpriteRenderer>().size.y;
                Vector2 pos = new Vector3(originPos.x + sizeX * r, originPos.y + sizeY * c,0);

                //Here we Instantiate the GameObject and then immediately get a reference to it's Tile script.
                GameObject o = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                Tile t = o.GetComponent<Tile>();

                //We make sure to set the newly created tile in the appropriate slot in the 2D array and then name it accordingly
                tiles[r, c] = t;
                tiles[r, c].name = "[" + r.ToString() + "," + c.ToString() + "]";

            }   /*this whole for loop is confusing. does it make the tiles in sequence such as 1,1 to 2,2 or?*/

        }

        // We run some for loops after making the Grid to set any specific tiles. adds traps and holes until the for loop ends
        for (int i = 0; i < holeCount; i++)
        {
            AddHoles();
        }
        for (int i = 0; i < trapTileCount; i++)
        {
            AddTraps();
        }
        for (int i = 0; i < teleportCount; i++)
        {
            AddTeleport();
        }
    }

    //If we ever need the position for a tile, we can get it from one of these two functions.
    //The first one is for getting a position using the row anf column index
    public Vector3 GetTilePosition(int r, int c)
    {
        return tiles[r, c].transform.position;

    }
    //The second one is for getting a position using the tile itself
    public Vector3 GetTilePosition(Tile t)
    {
        return t.transform.position;

    }


    private void AddTraps()
    {
        //We get a random tile 
        Tile t = GetRandomTile();

        //We check that it isnt already been inluded as either a trap or Hole and that it doesnt set the player's start position 
        //as a trap. We do this by checking that, while the tile is either the origin tile, a hole or a trap, we keep getting a new tile

        while (t == tiles[0,0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || teleportTiles.Contains(t))
        {
            t = GetRandomTile();
        }
        
        /* Shouldn't the while be false instead of true? If the random tile is equal to [0,0] isn't that the players initial position? */

        //...when we break out of the while loop, it means what the random tile selected fulfills the above criteria
        //So we add it to the appropriate list, color it and set the appropriate bool to true
        trapTiles.Add(t); /*adds the tile to the trapTiles list */
        t.AdjustColor(Color.red);
        t.isTrap = true;

    }

    private void AddHoles()
    {
        //We get a random tile 
        Tile t = GetRandomTile();

        //We check that it isnt already been inluded as either a trap or Hole and that it doesnt set the player's start position 
        //as a trap. We do this by checking that, while the tile is either the origin tile, a hole or a trap, we keep getting a new tile

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || teleportTiles.Contains(t))
        {
            t = GetRandomTile();
        }

        //...when we break out of the while loop, it means what the random tile selected fulfills the above criteria
        //So we add it to the appropriate list, color it and set the appropriate bool to true
        inaccessibleTiles.Add(t);
        t.AdjustColor(Color.black);
        t.isInaccessible = true;
    }

    private void AddTeleport()
    {
        //We get a random tile 
        Tile t = GetRandomTile();

        //We check that it isnt already been inluded as either a trap or Hole and that it doesnt set the player's start position 
        //as a trap. We do this by checking that, while the tile is either the origin tile, a hole or a trap, we keep getting a new tile

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || teleportTiles.Contains(t))
        {
            t = GetRandomTile();
        }

        //...when we break out of the while loop, it means what the random tile selected fulfills the above criteria
        //So we add it to the appropriate list, color it and set the appropriate bool to true
        teleportTiles.Add(t);
        t.AdjustColor(Color.blue);
        t.isTeleport = true;
    }

    public Tile GetRandomTile()
    {
        //This just returns a random tile from the 2D area but using a random row and random column index
        //return tiles[Random.Range(0, rows), Random.Range(0, columns)];

        Tile t = tiles[Random.Range(0, rows), Random.Range(0, columns)];

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || teleportTiles.Contains(t))
        {
            t = GetRandomTile();
        }

        return t; 

    }

}

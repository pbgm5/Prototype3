using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Tile script that is on the prefab object, we can set hese variables insided the nested for loop in the Gridgenerator
public class Tile : MonoBehaviour
{
    //for storing the tile color
    public Color tileColor;

    //For storing the row and column of the tile (for easy identification)
    public int row;
    public int column;

    //bools for setting whether the tile is a trap or inaccessible 
    public bool isTrap;
    public bool isInaccessible;


    //This is a method for adjusting the color of the tile, we call it in the GridGenerator to color the tile once we've 
    //randomly decided with type of tile it will be 
    public void AdjustColor(Color col)
    {
        tileColor = col;
        GetComponent<SpriteRenderer>().color = tileColor;
    }

}

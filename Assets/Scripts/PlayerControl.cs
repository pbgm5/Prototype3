using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public CameraShake cam;
    public float moveSpeed;
    public GridGenerator grid;
    private bool isMoving;
    private Color playerColor;
   
    private int rIndex = 0;
    private int lastRIndex = 0;
    private int lastCIndex = 0;
    private int cIndex = 0;
    private Tile currentTile;
    private Tile lastTile;
    private Tile targetTile;
    public Tile randomTile;


    void Start()
    {      

        //This sets the player to grid place 0,0 at start
        transform.position = grid.GetTilePosition(rIndex, cIndex); //will always equal 0,0
        //transform.position = grid.GetTilePosition(randomr, randomc);
        //We make sure that the current tile and target tile are set to our current 0,0 tile at start
        currentTile = grid.tiles[rIndex, cIndex]; //currently is 0,0 changes whenever the player moves
        targetTile = currentTile; //currently is 0,0 but changes

        playerColor = GetComponentInChildren<SpriteRenderer>().color;

        
    }


    void Update()
    {
        //If the player is not moving, WASD can be used to set the direction the player needs to move.
        //Player will then move a single space in that direction
        if (!isMoving) /* why do you need the condition of not moving? why can't you just code whenever the player pressed a key? */
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SetTargetTile(Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                SetTargetTile(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                SetTargetTile(Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.S)) 
            {
                SetTargetTile(Vector3.down);
            }
        }
        
        //here is what triggers the player to move, once the targettile and currentle are not the samw
        //We run the coroutine that moves the player from their current position to their target position
        if (targetTile != currentTile)
        {
            StartCoroutine(MovePlayer(grid.GetTilePosition(currentTile), grid.GetTilePosition(targetTile))); /*moves the player from current tile to target tile*/

            //We then make a referenc to the last tile in case we need it again later -See: Traps
            lastTile = currentTile;
            //And we make sure to set currentTile to the targettile so that we dont run the corourine endlessly
            currentTile = targetTile; 
        }   
    }

    //These two functions do the same thing but take in different arguments.
    //The first sets the target tile based on WASD direction  
    public void SetTargetTile(Vector3 dir)  /*confusing*/
    {
        if (rIndex + dir.x >= 0 && rIndex + dir.x < grid.rows && cIndex + dir.y >= 0 && cIndex + dir.y < grid.columns)
        {
            var t = grid.tiles[rIndex + (int)dir.x, cIndex + (int)dir.y];

            if (!t.isInaccessible)
            {
                targetTile = t;
                lastRIndex = rIndex;
                lastCIndex = cIndex;
                rIndex += (int)dir.x;
                cIndex += (int)dir.y;
            }
        }      
    }

    //The second let's us direct pass a tile as target tile.
    //In this case we also need to remeber to update the row and column indices since we'll need them 
    //to keep track of where we are in the grid
    public void SetTargetTile(Tile t) /*think this function makes the player get pushed back to the recent tile when they go try to go to inaccessible tile*/ 
    {
        if (!t.isInaccessible)
        {
            targetTile = t;
            rIndex = lastRIndex;
            cIndex = lastCIndex;
        }

    }

    //We move the player in a coroutine by sending a start and an end pos and just lerping between then
    //for the duration set. The duration is currently 1/ moveSpeed 
    private IEnumerator MovePlayer(Vector3 startPos, Vector3 endPos)
    {
        isMoving = true;

        float timeElapsed = 0;
        float duration = 1 / moveSpeed;

        while (timeElapsed < duration)
        {
            //The lerping happens in the while loop
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Once the player has completely shifted to the target location, we're gonna runa  function that process tile effects
        //We also make sure to set the position of the object to the exact endPos in case it's off by a very tiny amount
        //This tiny amounts can compound over time so this is necessary
        transform.position = endPos;
        ProcessTileEvents();
        TeleportEvent();

    }

    public void ProcessTileEvents()
    {
        //Currently the only tile effect we have is Traps
        //this function checks if the current tile is a trap tile
        if (currentTile.isTrap)
        {
            //If it is, move the player back to the last tile they were on
            StartCoroutine(FlashPlayer());
            //And shake the camera for effect
            cam.shakeDuration = 0.25f;

            //This step is also necessary to make sure out place on the grid gets properly updated
            SetTargetTile(lastTile);
        }
        else
        {
            isMoving = false;
        }

    }

    public void TeleportEvent()
    {
        randomTile = grid.GetRandomTile();
        if (currentTile.isTeleport)
        {

            transform.position = grid.GetTilePosition(randomTile);
            currentTile = randomTile;
        }
    }

        //This coroutine just flashs the player red when they are hit by a trap
        private IEnumerator FlashPlayer()
    {
        WaitForSeconds blinkDuration = new WaitForSeconds(0.04f);

        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = Color.red;
        yield return blinkDuration;
        sr.color = playerColor;
        yield return blinkDuration;
        sr.color = Color.red;
        yield return blinkDuration;
        sr.color = playerColor;
        yield return blinkDuration;
        sr.color = Color.red;
        yield return blinkDuration;
        sr.color = playerColor;

    }
}

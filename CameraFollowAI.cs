using UnityEngine;
using System.Collections;

public class CameraFollowAI : MonoBehaviour {

    //the AI object
    public MoveAI player;
    //the camera object for the AI
    public GameObject camera;
    //the position of the jail
    public Transform inJail;
    //the previous position
    Vector3 outdatedPlayerPos;
    float Tparam;
    //the desired location of the camera after the user has displaced.
    Vector3 desiredLocationCamera;
    public float speedCam;

    // Use this for initialization
    void Start()
    {

        //desired location of the camera at the start of the game
        desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);
        //the current position of the player, that will be considered its previous position during the next iteration
        outdatedPlayerPos = player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        //to notice any change in the player's transform position. If the player did move, a change will be made to 
        //the position of the camera.
        if (player.transform.position.x > outdatedPlayerPos.x + 1 || player.transform.position.x < outdatedPlayerPos.x - 1
            || player.transform.position.z > outdatedPlayerPos.z + 1 || player.transform.position.z < outdatedPlayerPos.z - 1)
        {
            //depending on the index, the camera will need to move differently, because no matter where the AI will be on the 
            //board, the camera always has to face the center of the board
            if (player.index <= 9 || player.index == 39)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);

            else if ((player.index > 9 && player.index < 19) || player.lockedInJail)
                desiredLocationCamera = new Vector3(player.transform.position.x + 350, player.transform.position.y + 112, player.transform.position.z);
            else if (player.index >= 19 && player.index < 29)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z - 350);
            else if (player.index >= 29 && player.index < 39)
                desiredLocationCamera = new Vector3(player.transform.position.x - 350, player.transform.position.y + 112, player.transform.position.z);

            //These are the positions on the board that are at the corners of the board
            if ((player.index == 10 || player.index == 19 || player.index == 29 || player.index == 39) && !player.justGotOut) // for each corner to turn camera
            {
                camera.transform.Rotate(new Vector3(20, 83, -20));
            }
            //if the player is going to jail, this breaks the pattern of setting the rotation
            if (player.lockedInJail)
            {
                if (player.index <= 9)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
                else if (player.index >= 19 && player.index < 29)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
                else if (player.index >= 29 && player.index < 39)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
            }

            //animation for moving the camera
            StartCoroutine(MoveFct());
            //now outdated player position for next iteration
            outdatedPlayerPos = player.transform.position;
        }
    }

    IEnumerator MoveFct()
    {
        //animation to move the camera slowly instead of moving it directly to a new position
        float timeSinceStart = 0f;
        while (true)
        {
            timeSinceStart += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, desiredLocationCamera, timeSinceStart);

            //if object has arrived, stop coroutine
            if (transform.position == desiredLocationCamera)
                yield break;

            yield return null; //otherwise, continue next frame
        }
    }
}

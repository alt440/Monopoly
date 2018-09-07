using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//the object player one
    public MovePlayer player;
	//the camera object
    public GameObject camera;
	//the position indicating the player is in jail
    public Transform inJail;
	//the previous position
    Vector3 outdatedPlayerPos;
    float Tparam;
	//the location that we want the camera to move to
    Vector3 desiredLocationCamera;
	//the speed of movement of the camera
    public float speedCam;

	// Use this for initialization
	void Start () {

		//different location than the player's because we want to see the player object from a distace at an angle
        desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);
		//the current position of the player (that is going to be outdated soon)
        outdatedPlayerPos = player.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	
        if (player.transform.position.x > outdatedPlayerPos.x+1 || player.transform.position.x < outdatedPlayerPos.x - 1
            || player.transform.position.z > outdatedPlayerPos.z + 1 || player.transform.position.z < outdatedPlayerPos.z - 1)
        {
		//displacing the camera depending on the lane the player is at.
		//the camera needs to be displaced with different positions once we enter a new row of the monopoly game, because
		//I want the camera to look towards the inside of the board. However, to do so, everytime the player goes in a
		//different direction, the camera has to move differently.
            if (player.index <= 9 || player.index == 39)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);

            else if ((player.index > 9 && player.index < 19) || player.lockedInJail)
                desiredLocationCamera = new Vector3(player.transform.position.x + 350, player.transform.position.y + 112, player.transform.position.z);
            else if (player.index >= 19 && player.index < 29)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z - 350);
            else if (player.index >= 29 && player.index < 39)
                desiredLocationCamera = new Vector3(player.transform.position.x - 350, player.transform.position.y + 112, player.transform.position.z);

		//I rotate the camera once when I reach corners of the board
            if ((player.index == 10 || player.index == 19 || player.index == 29 || player.index == 39) && !player.justGotOut) // for each corner to turn camera
            {
                camera.transform.Rotate(new Vector3(20, 83, -20));
            }
		//adjust the angle once the player goes to jail, because he can be anywhere on the board when he gets the
		//get in jail card
            if (player.lockedInJail)
            {
                Debug.Log("CameraInJail");
                if (player.index <= 9)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
                else if (player.index>=19 && player.index < 29)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
                else if (player.index>=29 && player.index < 39)
                {
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                    camera.transform.Rotate(new Vector3(20, 83, -20));
                }
            }

		//Start coroutine (in tandem) to move the camera towards the new location/ new rotation I want it to have.
            StartCoroutine(MoveFct());
		//considered the previous location for the next iteration
            outdatedPlayerPos = player.transform.position;
        }
    }

    IEnumerator MoveFct()
    {
	    //the MoveFct displaces the camera slowly. This is an animation function to not suddenly displace the
	    //camera to another location
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

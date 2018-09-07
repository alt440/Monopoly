using UnityEngine;
using System.Collections;

public class CameraToDice : MonoBehaviour {

    //the dice object
    public GameObject dice;
    //the camera object that follows the dice
    public GameObject camera;
    //the previous player position
    Vector3 outdatedPlayerPos;
    float Tparam;
    //the desired location of the camera after the dice move
    Vector3 desiredLocationCamera;
    public float speedCam;

    // Use this for initialization
    void Start () {
        //initial position of the dice camera
        desiredLocationCamera = new Vector3(dice.transform.position.x, dice.transform.position.y + 300, dice.transform.position.z);
        outdatedPlayerPos = dice.transform.position;

    }

    // Update is called once per frame
    void Update () {

        //this is for the dice camera to be constantly above of the dice.
        if (dice.transform.position.x > outdatedPlayerPos.x + 1 || dice.transform.position.x < outdatedPlayerPos.x - 1
            || dice.transform.position.z > outdatedPlayerPos.z + 1 || dice.transform.position.z < outdatedPlayerPos.z - 1)
        {
            desiredLocationCamera = new Vector3(dice.transform.position.x, dice.transform.position.y + 300, dice.transform.position.z);

            StartCoroutine(MoveFct());
            outdatedPlayerPos = dice.transform.position;
        }
    }

    IEnumerator MoveFct()
    {
        //this is the animation that moves the camera so that it follows the dice.
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

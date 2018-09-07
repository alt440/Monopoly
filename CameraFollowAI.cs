using UnityEngine;
using System.Collections;

public class CameraFollowAI : MonoBehaviour {

    public MoveAI player;
    public GameObject camera;
    public Transform inJail;
    Vector3 outdatedPlayerPos;
    float Tparam;
    Vector3 desiredLocationCamera;
    public float speedCam;

    // Use this for initialization
    void Start()
    {

        desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);
        outdatedPlayerPos = player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.x > outdatedPlayerPos.x + 1 || player.transform.position.x < outdatedPlayerPos.x - 1
            || player.transform.position.z > outdatedPlayerPos.z + 1 || player.transform.position.z < outdatedPlayerPos.z - 1)
        {
            if (player.index <= 9 || player.index == 39)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z + 350);

            else if ((player.index > 9 && player.index < 19) || player.lockedInJail)
                desiredLocationCamera = new Vector3(player.transform.position.x + 350, player.transform.position.y + 112, player.transform.position.z);
            else if (player.index >= 19 && player.index < 29)
                desiredLocationCamera = new Vector3(player.transform.position.x, player.transform.position.y + 112, player.transform.position.z - 350);
            else if (player.index >= 29 && player.index < 39)
                desiredLocationCamera = new Vector3(player.transform.position.x - 350, player.transform.position.y + 112, player.transform.position.z);

            if ((player.index == 10 || player.index == 19 || player.index == 29 || player.index == 39) && !player.justGotOut) // for each corner to turn camera
            {
                camera.transform.Rotate(new Vector3(20, 83, -20));
            }
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

            StartCoroutine(MoveFct());
            outdatedPlayerPos = player.transform.position;
        }
    }

    IEnumerator MoveFct()
    {
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

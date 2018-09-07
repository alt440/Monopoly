using UnityEngine;
using System.Collections;

public class CameraToDice : MonoBehaviour {

    public GameObject dice;
    public GameObject camera;
    Vector3 outdatedPlayerPos;
    float Tparam;
    Vector3 desiredLocationCamera;
    public float speedCam;

    // Use this for initialization
    void Start () {
        desiredLocationCamera = new Vector3(dice.transform.position.x, dice.transform.position.y + 300, dice.transform.position.z);
        outdatedPlayerPos = dice.transform.position;

    }

    // Update is called once per frame
    void Update () {

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

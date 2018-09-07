using UnityEngine;
using System.Collections;

public class DicesRoll : MonoBehaviour {

    public int valueOfDice;
    public bool readyForMove = false;
    Vector3 speed;
    public GameObject camera;
    public MovePlayer player;

	// Use this for initialization
	void Start () {

        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
        }

        Physics.gravity= new Vector3(0, -300);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0,Random.Range(-300,-500));
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1,10); 
	}
	
	// Update is called once per frame
	void Update () {

        if (GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            speed = GetComponent<Rigidbody>().velocity;
        }
        if (GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            
            if (speed != Vector3.zero)
            {

                //rotate 90 degrees towards camera
                //moveTowards camera
                MovePlayerValue();
                speed = Vector3.zero;
                if ( gameObject.activeInHierarchy == true)
                readyForMove = true;
                
            }
        }
	}

    void MovePlayerValue()
    {

        if (transform.localEulerAngles.z >89 &&  transform.localEulerAngles.z <91)
        {
            valueOfDice = 5;
            Debug.Log("5");
        }
        else if (transform.localEulerAngles.x > 88 &&  transform.localEulerAngles.x<91)
        {
            valueOfDice = 3;
            Debug.Log("3");
        }
        else if ( transform.localEulerAngles.z > 179 &&  transform.localEulerAngles.z <181)
        {
            valueOfDice = 6;
            Debug.Log("6");
        }
        else if (transform.localEulerAngles.z > 269 &&  transform.localEulerAngles.z < 271)
        {
            valueOfDice = 2;
            Debug.Log("2");
        }
        else if ( transform.localEulerAngles.x > 267 &&  transform.localEulerAngles.x < 274)
        {
            valueOfDice = 4;
            Debug.Log("4");
        }
        else 
        {
            valueOfDice = 1;
            Debug.Log("1");
        }
    }

    public void RestartRolling(MovePlayer player)
    {
        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
        }

        Physics.gravity = new Vector3(0, -300);
        if (player.index < 9 || player.index == 39)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, -100, Random.Range(-300, -500));
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if ((player.index >= 9 && player.index < 19) || player.lockedInJail)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-300, -500), -100, 0);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if (player.index >= 19 && player.index < 29)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, -100, Random.Range(300, 500));
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if (player.index >= 29 && player.index < 39)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(300, 500), -100, 0);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
       
    }

    public void RestartRolling(MoveAI player)
    {
        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
        }

        Physics.gravity = new Vector3(0, -300);
        if (player.index < 9 || player.index == 39)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, -100, Random.Range(-300, -500));
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if ((player.index >= 9 && player.index < 19) || player.lockedInJail)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-300, -500), -100, 0);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if (player.index >= 19 && player.index < 29)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, -100, Random.Range(300, 500));
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }
        else if (player.index >= 29 && player.index < 39)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(300, 500), -100, 0);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(1, 10);
        }

    }
}

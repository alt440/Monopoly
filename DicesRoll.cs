using UnityEngine;
using System.Collections;

public class DicesRoll : MonoBehaviour {

	//this class is set to a dice object
	//the value indicated by the dice
    public int valueOfDice;
	//set when it is the beginning of a player's turn
    public bool readyForMove = false;
	//speed of dice thrown
    Vector3 speed;
	//camera object on the dice
    public GameObject camera;
	//main player of the game
    public MovePlayer player;

	// Use this for initialization
	void Start () {

		//if the dice are not currently used
        if (gameObject.activeInHierarchy == false)
        {
		//they are activated
            gameObject.SetActive(true);
        }

		//to let the dice stay on the board
        Physics.gravity= new Vector3(0, -300);
		//sets velocity and angular velocity of the dice throw
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

	    //this method finds the value the dice indicate
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
	    //if the dice are not doing anything right now
        if (gameObject.activeInHierarchy == false)
        {
		//see the dice
            gameObject.SetActive(true);
        }

	    //sets the gravity so the players do not jump everytime they are moved
        Physics.gravity = new Vector3(0, -300);
	    //sets the direction of the dice, their velocity and angular velocity when they are thrown
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
	    //if the dice is not doing anything right now
        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
        }

	    //setss the gravity so the object stays on the board and does not jump everytime it moves
        Physics.gravity = new Vector3(0, -300);
	    //this below gives the direction of the throw of dice and both the velocity and angular velocity of the dice
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

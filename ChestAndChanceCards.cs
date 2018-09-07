using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChestAndChanceCards : MonoBehaviour { //SET CARDS FOR P2 ALSO

	//The information on the card
    public Text infoCard;
	//The type of card (Chest, Chance)
    public Text typeOfCard;
	//White image on background of the card
    public RawImage backgroundCard;
	//the player (user)
    public MovePlayer player;
	//the AI
    public MoveAI player2;
	//the pause panel that activates once user presses a key.
    public GameObject pausePanel;

	//the camera that follows player one
    public GameObject cameraP1;
	//the camera that follows player two
    public GameObject cameraP2;

    public GameObject getOutOfJailCard;
	//the position of the jail
    public Transform inJail;

	//when the player presses the key to pause (P), the isPaused bool becomes true
    bool isPaused = false;

    string[] chanceCards = new string[17];
    string[] communityChestCards = new string[17];

	//activated when the player/AI has finished his/her/its turn
    bool hasPlayed = false;

	// Use this for initialization
	void Start () {

		//setting all the community chest cards and chance cards' text
        chanceCards[0] = "Advance to Go -- Collect $200";
        chanceCards[1] = "Advance to Illinois Ave.";
        chanceCards[2] = "Advance to the nearest Utility. If\nunowned, you may buy it from the Bank. If\nowned, throw the dice and pay the owner a\ntotal of ten times the amount thrown.";
        chanceCards[3] = "Advance to the nearest Railroad and\npay the owner twice the rental to which\nhe or she is otherwise entitled. If it is\nunowned, you may buy it from the Bank.";
        chanceCards[4] = "Advance to the nearest Railroad and\npay the owner twice the rental to which\nhe or she is otherwise entitled. If it is\nunowned, you may buy it from the Bank.";
        chanceCards[5] = "Advance to St-Charles Place. If you\npass Go, collect $200";
        chanceCards[6] = "The Bank pays you a dividend of $50";
        chanceCards[7] = "Get out of Jail free card -- This\ncard may be kept until needed, or sold.";
        chanceCards[8] = "Go back three spaces.";
        chanceCards[9] = "Go directly to Jail -- Do not pass\n Go, do not collect $200.";
        chanceCards[10] = "Make general repairs on all your\nproperty -- for each house pay $25,\nfor each hotel, $100.";
        chanceCards[11] = "Pay the poor tax of $15";
        chanceCards[12] = "Take a trip to Reading Railroad.\nIf you pass Go, collect $200.";
        chanceCards[13] = "Take a walk on the Boardwalk.\nAdvance to Boardwalk.";
        chanceCards[14] = "You have been elected chairman\nof the board. Pay each player $50.";
        chanceCards[15] = "Your building loan matures.\nCollect $150.";
        chanceCards[16] = "You have won a crossword\ncompetition. Collect $100.";

        communityChestCards[0] = "Advance to Go -- Collect $200.";
        communityChestCards[1] = "The Bank made a mistake in your\nfavor -- Collect $75.";
        communityChestCards[2] = "Doctor's fees -- Pay $50.";
        communityChestCards[3] = "Get out of jail free card. This\ncard may be kept until needed or sold.";
        communityChestCards[4] = "Go to Jail. Do not pass Go. Do\nnot collect $200.";
        communityChestCards[5] = "It is your birthday. Collect $10\nfrom each player.";
        communityChestCards[6] = "Grand Opera Night. Collect $50\nfrom each player for opening night seats.";
        communityChestCards[7] = "Income Tax refund. Collect $20.";
        communityChestCards[8] = "Life Insurance Matures. Collect $100.";
        communityChestCards[9] = "Pay Hospital Fees of $100.";
        communityChestCards[10] = "Pay School Fees of $50.";
        communityChestCards[11] = "Receive $25 Consultancy Fee.";
        communityChestCards[12] = "You are assessed for street repairs. Pay $40\nfor each house you own and $115 for each hotel.";
        communityChestCards[13] = "You have won the second prize in a beauty\ncontest. Collect $10.";
        communityChestCards[14] = "You inherit $100. Collect $100.";
        communityChestCards[15] = "From the sale of some of your stocks, you\nearn $50.";
        communityChestCards[16] = "Your holiday fund matures. You receive $100.";

    }

    // Update is called once per frame
    void Update () {

        /*if ((player.nextTurn || player2.nextTurn) && (player.index==7 || player.index==22 || player.index == 36 || player2.index == 7 || player2.index==22 || player2.index==36) && !hasPlayed){
            //chanceCard
            

            if (player.nextTurn && (player.index==7||player.index==22 || player.index == 36))
            {
                typeOfCard.text = "Chance";
                int cardChosen = Random.Range(0,16);
                infoCard.text = "" + chanceCards[cardChosen];
                backgroundCard.gameObject.SetActive(true);
                hasPlayed = true;
                StartCoroutine(seenCardChance(cardChosen, player));
            }
                
            else if (player2.nextTurn && (player2.index==7||player2.index==22 || player.index == 36))
            {
                typeOfCard.text = "Chance";
                int cardChosen = Random.Range(0, 16);
                infoCard.text = "" + chanceCards[cardChosen];
                backgroundCard.gameObject.SetActive(true);
                hasPlayed = true;
                StartCoroutine(seenCardChance(cardChosen, player2));
            }
                

        }
        if ((player.nextTurn || player2.nextTurn) && (player.index==2 || player.index==17 || player.index == 33 || player2.index==2 || player2.index==17 || player2.index==33) && !hasPlayed)
        {
            //communityChestCard
            
            if (player.nextTurn && (player.index == 2 || player.index == 17 || player.index == 33))
            {
                typeOfCard.text = "Community Chest";
                int cardChosen = Random.Range(0, 16);
                infoCard.text = "" + communityChestCards[cardChosen];
                backgroundCard.gameObject.SetActive(true);
                hasPlayed = true;
                StartCoroutine(seenCardComm(cardChosen, player));
            }
                
            else if (player2.nextTurn && (player2.index == 2 || player2.index == 17 || player2.index == 33))
            {
                typeOfCard.text = "Community Chest";
                int cardChosen = Random.Range(0, 16);
                infoCard.text = "" + communityChestCards[cardChosen];
                backgroundCard.gameObject.SetActive(true);
                hasPlayed = true;
                StartCoroutine(seenCardComm(cardChosen, player2));
            }
                
        }
        if (hasPlayed && Input.GetKeyUp(KeyCode.Return))
        {
            hasPlayed = false;
            backgroundCard.gameObject.SetActive(false);
        }*/

	    //get key up used because key down would probably be equivalent to pressing P multiple times
        if (Input.GetKeyUp(KeyCode.P))
        {
		//if the game was already paused
            if (isPaused)
            {
		    //this makes the game run anew.
                Time.timeScale = 1;
		    //the panel is removed from the user's view
                pausePanel.SetActive(false);
                isPaused = false;
            }
                
            else
            {
		    //this stops all activity of the game in the background of the pause panel
                Time.timeScale = 0;
		    //this sets the pause panel on the foreground.
                pausePanel.SetActive(true);
                isPaused = true;
            }
                
        }
	}

    IEnumerator seenCardChance(int chosenCard, MovePlayer P1)// must be more flexible since we wont know which player fell here.
    {
        if (P1.nextTurn)
        {
            yield return new WaitForSeconds(2);
        }
        
        backgroundCard.gameObject.SetActive(false);

	    //depending on the card, the user has to move to another location. This also means setting
	    //the camera correctly. When any card is chosen, different rotations and translations are made because the
	    //player jumps to a different location.
        if (chosenCard == 0)
        {
            //rotation
            if (player.index <= 9)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 19 && player.index < 29)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index > 9 && player.index < 19)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }

            player.index = 39;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            string[] valueMoney = player.money.text.Split('$');
		//action that the card does
            int moneyNow = int.Parse(valueMoney[1]) + 200;
            player.money.text = "$" + moneyNow; 

        }
        else if (chosenCard == 1)
        {
            //rotation
            if (player.index <= 9)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 29 && player.index < 39)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index > 9 && player.index < 19)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            player.index = 23;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            //action that the card does
	    player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 2)
        {
            //Advance to nearest utility and pay *10
            if (player.index >= 20)
                player.index = 27;
            else
                player.index = 11;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.hasMovedFromCard = true;
            player.utilityCard = true;
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 3 || chosenCard == 4)
        {
            //Advance to nearest Railroad and pay double the rent
            int currentIndex = player.index;
            if (currentIndex <= 10)
                player.index = 4;
            else if (currentIndex > 10 && currentIndex < 19)
                player.index = 14;
            else if (currentIndex >= 19 && currentIndex < 29)
                player.index = 24;
            else if (currentIndex >= 29)
                player.index = 34;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.hasMovedFromCard = true;
            player.trainCard = true;
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 5)
        {
            
            if (player.index > 10)
            {
                string[] valueMoney = player.money.text.Split('$');
                int moneyNow = int.Parse(valueMoney[1]) + 200;
                player.money.text = "$" + moneyNow;
            }
            //rotation
            if (player.index <= 9)
            {
                //cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 29 && player.index < 39)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 19 && player.index < 29)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(0, -90, 0));
            }
            player.index = 10;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 6)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 50;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 7)
        {
            //Adds choice when in jail to use card
            //Shows it in the properties overview
            getOutOfJailCard.SetActive(true);
        }
        else if (chosenCard == 8)
        {
            int toSeeIfChangedLane = player.index / 10;
            player.index -= 4;
            int didItChangeLane = player.index / 10;
            if (toSeeIfChangedLane != didItChangeLane)
            {
                cameraP1.transform.Rotate(new Vector3(20, -83, 20));
            }
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.hasMovedFromCard = true;
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 9)
        {
            //create another transform spot to be in jail
            player.transform.Translate(new Vector3(inJail.position.x - player.transform.position.x, player.transform.position.z - inJail.position.z, 0));
            StartCoroutine(player.lockedUp());
            //Activate inJail action
        }
        else if (chosenCard == 10)
        {
            //count houses and hotels player has. + knowing to whom properties belong will help.
            //25$ / house, 100$ / hotel
            int cost = 0;
            for (int i = 0; i < player.nextPositions.Length; i++)
            {
                if (player.nextPositions[i].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (player.nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                    {
                        cost += 25;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                    {
                        cost += 50;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                    {
                        cost += 75;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                    {
                        cost += 100;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().hotel)
                    {
                        cost += 100;
                    }
                }
            }
            string[] values = player.money.text.Split('$');
            if (int.Parse(values[1]) > cost)
            {
                player.money.text = "$" + (int.Parse(values[1]) - cost);
            }
            else
            {
                player.MortgageOrBankrupt(values, cost);
            }
        }
        else if (chosenCard == 11)
        {
            string[] valueMoney = player.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 15)
                player.money.text = "$" + (int.Parse(valueMoney[1]) - 15);
            else
                player.MortgageOrBankrupt(valueMoney, 15);
        }
        else if (chosenCard == 12)
        {
            if (player.index > 4)
            {
                string[] valueMoney = player.money.text.Split('$');
                int moneyNow = int.Parse(valueMoney[1]) + 200;
                player.money.text = "$" + moneyNow;
            }
            if (player.index > 9 && player.index < 19)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 29 && player.index < 39)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 19 && player.index < 29)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            player.index = 4;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 13)
        {
            if (player.index > 9 && player.index < 19)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index <= 9)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 19 && player.index < 29)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            player.index = 38;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            player.Properties_Mgmt(player.index+1);
        }
        else if (chosenCard == 14)
        {
            //need an array of players bc makes it indefinite amount of players.
            //for (int i=0; i<players.Length;i++){
            //(playerX)money -50
            //(player[i] -> money +50
            string[] values = player.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 50;
            player.money.text = "$" + moneyAfter;
            string[] valuesP2 = player2.money.text.Split('$');
            if (int.Parse(valuesP2[1]) > 50)
            {
                player2.money.text = "$" + (int.Parse(valuesP2[1]) - 50);
            }
            else
                player2.MortgageOrBankrupt(valuesP2, 50);
        }
        else if (chosenCard == 15)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 150;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 16)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player.money.text = "$" + moneyNow;
        }
    }

    IEnumerator seenCardChance(int chosenCard, MoveAI P2)// must be more flexible since we wont know which player fell here.
    {
        if (P2.nextTurn)
        {
            yield return new WaitForSeconds(2);
        }
        
        backgroundCard.gameObject.SetActive(false);

        if (chosenCard == 0)
        {
            //rotation
            if (player2.index <= 9)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 19 && player2.index < 29)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index > 9 && player2.index < 19)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 39;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.hasMovedFromCard = true;
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 200;
            player2.money.text = "$" + moneyNow;

        }
        else if (chosenCard == 1)
        {
            //rotation
            if (player2.index <= 9)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 29 && player2.index < 39)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index > 9 && player2.index < 19)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 23;
            player2.hasMovedFromCard = true;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.Properties_Mgmt(player2.index+1);
        }
        else if (chosenCard == 2)
        {
            //Advance to nearest utility and pay *10
            if (player2.index >= 20)
                player2.index = 27;
            else
                player2.index = 11;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.hasMovedFromCard = true;
            player2.utilityCard = true;
            player2.Properties_Mgmt(player2.index + 1);
        }
        else if (chosenCard == 3 || chosenCard == 4)
        {
            //Advance to nearest Railroad and pay double the rent
            int currentIndex = player2.index;
            if (currentIndex <= 10)
                player2.index = 4;
            else if (currentIndex > 10 && currentIndex < 19)
                player2.index = 14;
            else if (currentIndex >= 19 && currentIndex < 29)
                player2.index = 24;
            else if (currentIndex >= 29)
                player2.index = 34;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.hasMovedFromCard = true;
            player2.trainCard = true;
            player2.Properties_Mgmt(player2.index + 1);
        }
        else if (chosenCard == 5)
        {
            if (player2.index > 10)
            {
                string[] valueMoney = player2.money.text.Split('$');
                int moneyNow = int.Parse(valueMoney[1]) + 200;
                player2.money.text = "$" + moneyNow;
            }
            //rotation
            if (player2.index <= 9)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 29 && player2.index < 39)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 19 && player2.index < 29)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 10;
            player2.hasMovedFromCard = true;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.Properties_Mgmt(player2.index+1);
        }
        else if (chosenCard == 6)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 50;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 7)
        {
            //Adds choice when in jail to use card
            //Shows it in the properties overview
        }
        else if (chosenCard == 8)
        {
            int toSeeIfChangedLane = player2.index / 10;
            player2.index -= 4;
            int didItChangeLane = player2.index / 10;
            if (toSeeIfChangedLane != didItChangeLane)
            {
                cameraP2.transform.Rotate(new Vector3(20, -83, 20));
            }
            player2.hasMovedFromCard = true;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.Properties_Mgmt(player2.index+1);
        }
        else if (chosenCard == 9)
        {
            //create another transform spot to be in jail
            player.transform.Translate(new Vector3(inJail.position.x - player.transform.position.x, player.transform.position.z - inJail.position.z, 0));
            StartCoroutine(player.lockedUp());
        }
        else if (chosenCard == 10)
        {
            //count houses and hotels player2 has. + knowing to whom properties belong will help.
            int cost = 0;
            for (int i = 0; i < player2.nextPositions.Length; i++)
            {
                if (player2.nextPositions[i].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (player2.nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                    {
                        cost += 25;
                    }
                    else if (player2.nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                    {
                        cost += 50;
                    }
                    else if (player2.nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                    {
                        cost += 75;
                    }
                    else if (player2.nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                    {
                        cost += 100;
                    }
                    else if (player2.nextPositions[i].gameObject.GetComponent<Properties>().hotel)
                    {
                        cost += 100;
                    }
                }
            }
            string[] values = player2.money.text.Split('$');
            if (int.Parse(values[1]) > cost)
            {
                player2.money.text = "$" + (int.Parse(values[1]) - cost);
            }
            else
            {
                player2.MortgageOrBankrupt(values, cost);
            }
        }
        else if (chosenCard == 11)
        {
            string[] valueMoney = player2.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 15)
                player2.money.text = "$" + (int.Parse(valueMoney[1]) - 15);
            else
                player2.MortgageOrBankrupt(valueMoney, 15);
        }
        else if (chosenCard == 12)
        {
            if (player2.index > 4)
            {
                string[] valueMoney = player2.money.text.Split('$');
                int moneyNow = int.Parse(valueMoney[1]) + 200;
                player2.money.text = "$" + moneyNow;
            }
            if (player2.index > 9 && player2.index < 19)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 29 && player2.index < 39)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 19 && player2.index < 29)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 4;
            player2.hasMovedFromCard = true;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.Properties_Mgmt(player2.index+1);
        }
        else if (chosenCard == 13)
        {
            if (player2.index > 9 && player2.index < 19)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index <= 9)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 19 && player2.index < 29)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 38;
            player2.hasMovedFromCard = true;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            player2.Properties_Mgmt(player2.index+1);
        }
        else if (chosenCard == 14)
        {
            //need an array of player2s bc makes it indefinite amount of player2s.
            //for (int i=0; i<player2s.Length;i++){
            //(player2X)money -50
            //(player2[i] -> money +50
            string[] values = player2.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 50;
            player2.money.text = "$" + moneyAfter;
            string[] valuesP1 = player.money.text.Split('$');
            if (int.Parse(valuesP1[1]) > 50)
            {
                player.money.text = "$" + (int.Parse(valuesP1[1]) - 50);
            }
            else
                player.MortgageOrBankrupt(valuesP1, 50);
        }
        else if (chosenCard == 15)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 150;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 16)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player2.money.text = "$" + moneyNow;
        }
    }

    IEnumerator seenCardComm(int chosenCard, MovePlayer P1) // ----------------------------------------------------------------------------------------------------------------
    {
        if (P1.nextTurn)
        {
            yield return new WaitForSeconds(2);
        }
        
        backgroundCard.gameObject.SetActive(false);

        if (chosenCard == 0)
        {
            //rotation
            if (player.index <= 9)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index >= 19 && player.index < 29)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player.index > 9 && player.index < 19)
            {
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
                cameraP1.transform.Rotate(new Vector3(20, 83, -20));
            }
            player.index = 39;
            player.hasMovedFromCard = true;
            player.transform.Translate(new Vector3(player.nextPositions[player.index].position.x - player.transform.position.x, player.transform.position.z - player.nextPositions[player.index].position.z, 0));
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 200;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 1)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 75;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 2)
        {
            string[] valueMoney = player.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 50)
                player.money.text = "$" + (int.Parse(valueMoney[1]) - 50);
            else
                player.MortgageOrBankrupt(valueMoney, 50);
        }
        else if (chosenCard == 3)
        {
            //Adds choice when in jail to use card
            //Shows it in the properties overview
            getOutOfJailCard.SetActive(true);
        }
        else if (chosenCard == 4)
        {
            //create another transform spot to be in jail
            player2.transform.Translate(new Vector3(inJail.position.x - player2.transform.position.x, player2.transform.position.z - inJail.position.z, 0));
            StartCoroutine(player.lockedUp());
        }
        else if (chosenCard == 5)
        {
            // money +10 to one that got card from each player
            string[] values = player.money.text.Split('$');
            string[] valuesP2 = player2.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 10;
            player.money.text = "$" + moneyAfter;
            if (int.Parse(valuesP2[1]) > 10)
            {
                player2.money.text = "$" + (int.Parse(valuesP2[1]) - 10);
            }
            else
                player2.MortgageOrBankrupt(valuesP2, 10);
        }
        else if (chosenCard == 6)
        {
            // money +50 to one that got card from each player
            string[] values = player.money.text.Split('$');
            string[] valuesP2 = player2.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 50;
            player.money.text = "$" + moneyAfter;
            if (int.Parse(valuesP2[1]) > 50)
            {
                player2.money.text = "$" + (int.Parse(valuesP2[1]) - 50);
            }
            else
                player2.MortgageOrBankrupt(valuesP2, 50);
        }
        else if (chosenCard == 7)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 20;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 8)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 9)
        {
            string[] valueMoney = player.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 100)
                player.money.text = "$" + (int.Parse(valueMoney[1]) - 100);
            else
                player.MortgageOrBankrupt(valueMoney, 100);
        }
        else if (chosenCard == 10)
        {
            string[] valueMoney = player.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 50)
                player.money.text = "$" + (int.Parse(valueMoney[1]) - 50);
            else
                player.MortgageOrBankrupt(valueMoney, 50);
        }
        else if (chosenCard == 11)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 25;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 12)
        {
            //count houses and hotels from properties player owns.
            // 40$ / house, 115$ / hotel
            int cost = 0;
            for (int i = 0; i < player.nextPositions.Length; i++)
            {
                if (player.nextPositions[i].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (player.nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                    {
                        cost += 40;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                    {
                        cost += 80;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                    {
                        cost += 120;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                    {
                        cost += 160;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().hotel)
                    {
                        cost += 115;
                    }
                }
            }
            string[] values = player.money.text.Split('$');
            if (int.Parse(values[1]) > cost)
            {
                player.money.text = "$" + (int.Parse(values[1]) - cost);
            }
            else
            {
                player.MortgageOrBankrupt(values, cost);
            }
        }
        else if (chosenCard == 13)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 10;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 14)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 15)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 50;
            player.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 16)
        {
            string[] valueMoney = player.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player.money.text = "$" + moneyNow;
        }
    }

    IEnumerator seenCardComm(int chosenCard, MoveAI P2)
    {
        if (P2.nextTurn)
        {
            yield return new WaitForSeconds(2);
        }
        
        backgroundCard.gameObject.SetActive(false);

        if (chosenCard == 0)
        {
            //rotation
            if (player2.index <= 9)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index >= 19 && player2.index < 29)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            else if (player2.index > 9 && player2.index < 19)
            {
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
                cameraP2.transform.Rotate(new Vector3(20, 83, -20));
            }
            player2.index = 39;
            player2.transform.Translate(new Vector3(player2.nextPositions[player2.index].position.x - player2.transform.position.x, player2.transform.position.z - player2.nextPositions[player2.index].position.z, 0));
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 200;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 1)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 75;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 2)
        {
            string[] valueMoney = player2.money.text.Split('$');
            player2.money.text = "$" + (int.Parse(valueMoney[1]) - 50);
        }
        else if (chosenCard == 3)
        {
            //Adds choice when in jail to use card
            //Shows it in the properties overview
        }
        else if (chosenCard == 4)
        {
            //create another transform spot to be in jail
            player.transform.Translate(new Vector3(inJail.position.x - player.transform.position.x, player.transform.position.z - inJail.position.z, 0));
            StartCoroutine(player.lockedUp());
        }
        else if (chosenCard == 5)
        {
            // money +10 to one that got card from each player2
            string[] values = player2.money.text.Split('$');
            string[] valuesP2 = player.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 10;
            player2.money.text = "$" + moneyAfter;
            if (int.Parse(valuesP2[1]) > 10)
            {
                player.money.text = "$" + (int.Parse(valuesP2[1]) - 10);
            }
            else
                player.MortgageOrBankrupt(valuesP2, 10);
        }
        else if (chosenCard == 6)
        {
            // money +50 to one that got card from each player2
            string[] values = player2.money.text.Split('$');
            string[] valuesP2 = player.money.text.Split('$');
            int moneyAfter = int.Parse(values[1]) + 50;
            player2.money.text = "$" + moneyAfter;
            if (int.Parse(valuesP2[1]) > 50)
            {
                player.money.text = "$" + (int.Parse(valuesP2[1]) - 50);
            }
            else
                player.MortgageOrBankrupt(valuesP2, 50);
        }
        else if (chosenCard == 7)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 20;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 8)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 9)
        {
            string[] valueMoney = player2.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 100)
                player2.money.text = "$" + (int.Parse(valueMoney[1]) - 100);
            else
                player2.MortgageOrBankrupt(valueMoney, 100);
        }
        else if (chosenCard == 10)
        {
            string[] valueMoney = player2.money.text.Split('$');
            if (int.Parse(valueMoney[1]) > 50)
                player2.money.text = "$" + (int.Parse(valueMoney[1]) - 50);
            else
                player2.MortgageOrBankrupt(valueMoney, 50);
        }
        else if (chosenCard == 11)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 25;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 12)
        {
            //count houses and hotels from properties player2 owns.
            // 40$ / house, 115$ / hotel
            int cost = 0;
            for (int i = 0; i < player.nextPositions.Length; i++)
            {
                if (player.nextPositions[i].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (player.nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                    {
                        cost += 40;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                    {
                        cost += 80;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                    {
                        cost += 120;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                    {
                        cost += 160;
                    }
                    else if (player.nextPositions[i].gameObject.GetComponent<Properties>().hotel)
                    {
                        cost += 115;
                    }
                }
            }
            string[] values = player2.money.text.Split('$');
            if (int.Parse(values[1]) > cost)
            {
                player2.money.text = "$" + (int.Parse(values[1]) - cost);
            }
            else
            {
                player2.MortgageOrBankrupt(values, cost);
            }
        }
        else if (chosenCard == 13)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 10;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 14)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 15)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 50;
            player2.money.text = "$" + moneyNow;
        }
        else if (chosenCard == 16)
        {
            string[] valueMoney = player2.money.text.Split('$');
            int moneyNow = int.Parse(valueMoney[1]) + 100;
            player2.money.text = "$" + (int.Parse(valueMoney[1]) + 100);
        }
    }

    public void onClickPlay()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

}

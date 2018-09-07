using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MovePlayer : DicesRoll {

    public Text rent;
    public Text house1;
    public Text house2;
    public Text house3;
    public Text house4;
    public Text hotel;
    public Text mortgage;
    public Text houseCost;
    public Text hotelCost;
    public Text exception;
    public RawImage white;
    public RawImage section; //need to change color of it.
    public Text titleOfProperty;

    public Text player1Index;

    public RawImage railroad;
    public RawImage waterWorks;
    public RawImage electricalCompany;

    public Button buy;
    public Button auction;
    public Text cannotBuy;
    //public GameObject auctionPanel;

    public Text money;

    public static bool isFirstTurn = true;

    public DicesRoll diceOne;
    public DicesRoll diceTwo;
    public MovePlayer player1;
    public MoveAI player2;
    public GameObject dices;
    public Camera cameraPlayer;
    public Camera cameraDice1;
    public Camera cameraDice2;
    public Text toContinue;
    public Text upgrades;
    public GameObject panelUpgrades;
    public int movePlayer =0;

    public Button[] buttons4PropertiesOwned;


    public Transform inJail;
    public Button payJail;
    int rolls = 0;
    public Transform[] nextPositions;
    public int index = 0;

    public bool nextTurn=false;
    public bool lackOfMoney = false;
    public int targetMoney;
    public Text targetMoneyToAccumulate;
    public bool utilityCard = false;
    public bool trainCard = false;
    bool turnCameraAtZero = false;

    bool hasPlayed = false; //so it doesnt loop Property_Mgmt func
    bool notOnUtility = true;
    bool pressed4Upgrades = false;
    bool onlyOnce4Utilities = false;
    bool wasNotOnUtility = true;
    public bool lockedInJail = false;
    public bool justGotLocked = false;
    public bool justGotOut = false;
    public GameObject jailCard;
    bool onlyOnceInJail = false;
    int valueToPay = 0;

    public bool hasMovedFromCard = false;

    UpgradingMortgage upgradeMenu;

    public Sprite violet;
    public Sprite violet1;
    public Sprite cyan;
    public Sprite pink;
    public Sprite orange;
    public Sprite red;
    public Sprite yellow;
    public Sprite green;
    public Sprite blue;
    public Sprite black;
    public Sprite white0;

    Vector3 fallingPosDice1;
    Vector3 fallingPosDice2;

	// Use this for initialization
	void Start () {

        toContinue.text = "";
        upgrades.text = "";
        money.text = "$1500";

        payJail.GetComponentInChildren<Text>().text = "Pay $50";
        payJail.gameObject.SetActive(false);
}
	
	// Update is called once per frame
	void Update () {

        if (nextTurn && !hasPlayed)
        {
            Properties_Mgmt(index);
            
        }
        if ((Input.GetKeyUp(KeyCode.U) && nextTurn))
        {
            // Check for possible houses to put and mortgage
            pressed4Upgrades = !pressed4Upgrades;
            panelUpgrades.SetActive(pressed4Upgrades);
        }

        if (lackOfMoney)
        {
            panelUpgrades.SetActive(true);
            string[] values = money.text.Split('$');
            if (int.Parse(values[1]) > targetMoney)
            {
                money.text = "$" + (int.Parse(values[1]) - targetMoney);
                string[] valuesP2 = player2.money.text.Split('$');
                int moneyP2 = int.Parse(valuesP2[1]) + targetMoney;
                player2.money.text = "$" + moneyP2;
                targetMoneyToAccumulate.text = "";
                lackOfMoney = false;
            }
        }

        if (diceOne.readyForMove && diceTwo.readyForMove && notOnUtility)
            {
                movePlayer = diceOne.valueOfDice + diceTwo.valueOfDice;
            Debug.Log("MP " + movePlayer);
                StartCoroutine(isEnabled());
            if (wasNotOnUtility && !lockedInJail)
                StartCoroutine(MovePlayerTo(movePlayer, player1));
            else if (lockedInJail)
            {
                Debug.Log("READY 4 MOVE");
                rolls++;
                if (diceOne.valueOfDice == diceTwo.valueOfDice)
                {
                    lockedInJail = false;
                    justGotOut = true;
                    player.index = 10;
                    
                    StartCoroutine(MovePlayerTo(movePlayer, player1));
                }
                nextTurn = true;
            }
            else
                valueToPay = movePlayer;
            diceOne.readyForMove = false;
            diceTwo.readyForMove = false;
            }

            if (!diceOne.gameObject.activeInHierarchy && !diceTwo.gameObject.activeInHierarchy) //to position dice b4 reengaging
        {
            if (index < 9 || index == 39)
            {
                diceOne.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x - 200f, player2.dices.transform.position.y, player2.dices.transform.position.z), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x - 200f, player2.dices.transform.position.y + 100f, player2.dices.transform.position.z), 20 * Time.deltaTime);
            }
            else if (index >=9 && index < 19)
            {
                diceOne.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x, player2.dices.transform.position.y, player2.dices.transform.position.z+200), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x, player2.dices.transform.position.y + 100f, player2.dices.transform.position.z+200), 20 * Time.deltaTime);
            }
            else if (index >=19 && index < 29)
            {
                diceOne.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x + 200f, player2.dices.transform.position.y, player2.dices.transform.position.z), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x + 200f, player2.dices.transform.position.y + 100f, player2.dices.transform.position.z), 20 * Time.deltaTime);
            }
            else if (index >=29 && index < 39)
            {
                diceOne.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x, player2.dices.transform.position.y, player2.dices.transform.position.z-200), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player2.dices.transform.position, new Vector3(player2.dices.transform.position.x, player2.dices.transform.position.y + 100f, player2.dices.transform.position.z-200), 20 * Time.deltaTime);
            }
            
        }

        if (lockedInJail && !onlyOnceInJail && !justGotLocked)
        {

            onlyOnceInJail = true;

            Debug.Log("INJAIL");
            diceOne.RestartRolling(player1);
            diceTwo.RestartRolling(player1);
            
        }

        if (lockedInJail)
        {
            if (!justGotLocked && nextTurn)
            {
                if (jailCard.gameObject.activeInHierarchy)
                {
                    payJail.GetComponentInChildren<Text>().text = "Get Out Free";
                }
                payJail.gameObject.SetActive(true);
            }
                
            if (rolls == 3)
            {
                rolls = 0;
                lockedInJail = false;
                player.index = 10;
                justGotOut = true;
                StartCoroutine(MovePlayerTo(movePlayer, player1));
            }
        }

        if (valueToPay != 0 && !onlyOnce4Utilities)
        {
            onlyOnce4Utilities = true;
            string[] emptyPockets = money.text.Split('$');
            Debug.Log(index);
            if (player1.index == 11)
            {
                //if player owns only one utility or two
                if (nextPositions[27].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || utilityCard)
                {
                    valueToPay *= 10;
                    utilityCard = false;
                    Debug.Log("HO");
                }
                else if (!nextPositions[27].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    valueToPay *= 4;
                    Debug.Log("HA");
                }
                if (int.Parse(emptyPockets[1]) > valueToPay)
                    money.text = "$" + (int.Parse(emptyPockets[1]) - valueToPay);
                else
                {
                    MortgageOrBankrupt(emptyPockets, valueToPay);
                }
            }
            else if (player1.index == 27)
            {

                //if player owns only one utility or two
                if (nextPositions[11].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || utilityCard)
                {
                    valueToPay *= 10;
                    utilityCard = false;
                    Debug.Log("HO");
                }
                else if (!nextPositions[11].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    valueToPay *= 4;
                    Debug.Log("HA");
                }
                string[] valueMoney = player1.money.text.Split('$');
                if (int.Parse(valueMoney[1]) > valueToPay)
                    money.text = "$" + (int.Parse(valueMoney[1]) - valueToPay);
                else
                {
                    MortgageOrBankrupt(valueMoney, valueToPay);
                }
            }
        }

        if (nextTurn && Input.GetKeyUp(KeyCode.Return) && !pressed4Upgrades)
        {
            toContinue.text = "";
            upgrades.text = "";
            cannotBuy.text = "";
            white.gameObject.SetActive(false);
            section.gameObject.SetActive(false);
            railroad.gameObject.SetActive(false);
            waterWorks.gameObject.SetActive(false);
            electricalCompany.gameObject.SetActive(false);
            buy.gameObject.SetActive(false);
            auction.gameObject.SetActive(false);
            panelUpgrades.SetActive(false);
            payJail.gameObject.SetActive(false);
            isFirstTurn = false;
            diceOne.RestartRolling(player2);
            diceTwo.RestartRolling(player2);
            nextTurn = false;
            hasPlayed = false;
            onlyOnce4Utilities = false;
            valueToPay = 0;
            wasNotOnUtility = true;
            onlyOnceInJail = false;
            justGotLocked = false;
            justGotOut = false;

            //so we dont see player when its AI's turn
            player1.gameObject.SetActive(false);
            player2.cameraPlayer.enabled = true;
            player1.cameraPlayer.enabled = false;
            player2.gameObject.SetActive(true);
            player2.money.gameObject.SetActive(true);
            player1.money.gameObject.SetActive(false);
            player2.cameraPlayer.GetComponentInChildren<Light>().enabled = true;
            player1.cameraPlayer.GetComponentInChildren<Light>().enabled = false;
        }
        
	}

    public IEnumerator MovePlayerTo(int moveValue, MovePlayer player)
    {
        yield return new WaitForSeconds(2f);
        while (moveValue > 0)
        {
            if (hasMovedFromCard)
                ++index;
            hasMovedFromCard = false;
            if (index == nextPositions.Length)
                index = 0;
            player.transform.Translate(new Vector3(nextPositions[index].position.x - player.transform.position.x, player.transform.position.z - nextPositions[index].position.z, 0 ));
            yield return new WaitForSeconds(.7f);
            moveValue--;
            justGotOut = false;
            if (index == nextPositions.Length - 1)
            {
                index = 0;
                string[] moneyValue = money.text.Split('$');
                money.text = "$"+(int.Parse(moneyValue[1])+200);
            }
                
            else
                index++;
        }
        nextTurn = true;
        toContinue.text = "Press Enter to Continue";
        upgrades.text = "Press U to Upgrade or Mortgage";
        turnCameraAtZero = false;
        
    }
    IEnumerator isEnabled()
    {
        
        cameraDice1.enabled = true;
        cameraPlayer.enabled = false;
        yield return new WaitForSeconds(.5f);
        cameraDice2.enabled = true;
        cameraDice1.enabled = false;
        yield return new WaitForSeconds(.5f);
        cameraPlayer.enabled = true;
        cameraDice2.enabled = false;
        diceOne.gameObject.SetActive(false);
        diceTwo.gameObject.SetActive(false);

        //setting rotation back to zero so you can translate correctly, depending on parent rotation
        diceOne.gameObject.transform.localEulerAngles = new Vector3(0,0);
        diceTwo.gameObject.transform.localEulerAngles = new Vector3(0,0);
    }

    public void Properties_Mgmt(int position)
    {
        if (position == 1) //Mediterranean Avenue
        {
            section.color = Color.HSVToRGB(0.755f, 0.258f, .147f);
            titleOfProperty.text = "Mediterranean Avenue";
            rent.text = "RENT $2";
            house1.text = "With 1 House                 $10";
            house2.text = "With 2 Houses                $30";
            house3.text = "With 3 Houses                $90";
            house4.text = "With 4 Houses                $160";
            hotel.text = "With Hotel $250";
            mortgage.text = "Mortgage Value $30";
            houseCost.text = "Houses cost $50 Each";
            hotelCost.text = "Hotels $50 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $60"; // Instead of repeating code, you could just change text and authorize activity at end of method.

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) //OFFICIAL CODE HERE WHEN THERE IS ONE, TWO,... HOUSES
            {
                Payment(position-1);

            } //ENDS HERE               ENDS HERE               ENDS HERE               ENDS HERE               ENDS HERE
            Debug.Log(isOccupied);
        }

        else if (position == 3) //Baltic Avenue
        {
            section.color = Color.HSVToRGB(.755f, .258f, .147f);
            titleOfProperty.text = "Baltic Avenue";
            rent.text = "RENT $4";
            house1.text = "With 1 House                 $20";
            house2.text = "With 2 Houses                $60";
            house3.text = "With 3 Houses                $180";
            house4.text = "With 4 Houses                $320";
            hotel.text = "With Hotel $450";
            mortgage.text = "Mortgage Value $30";
            houseCost.text = "Houses cost $50 Each";
            hotelCost.text = "Hotels $50 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $60";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
        }

        else if (position == 4)
        {
            string[] values = money.text.Split('$');
            if (200 > int.Parse(values[1]) * .1f)
            {
                if (int.Parse(values[1]) > 200)
                {
                    int moneyLeft = int.Parse(values[1]) - 200;
                    money.text = "$" + moneyLeft;
                }
                else
                {
                    MortgageOrBankrupt(values, 200);
                }
            }
            else
            {
                int moneyLeft = int.Parse(values[1]) - (int)((int.Parse(values[1])) * .1f);
                money.text = "$" + moneyLeft;
            }
        }

        else if (position == 6) // Oriental Avenue
        {
            section.color = Color.cyan;
            titleOfProperty.text = "Oriental Avenue";
            rent.text = "RENT $6";
            house1.text = "With 1 House                 $30";
            house2.text = "With 2 Houses                $90";
            house3.text = "With 3 Houses                $270";
            house4.text = "With 4 Houses                $400";
            hotel.text = "With Hotel $550";
            mortgage.text = "Mortgage Value $50";
            houseCost.text = "Houses cost $50 Each";
            hotelCost.text = "Hotels $50 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $100";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false )
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 8) // Vermont Avenue
        {
            section.color = Color.cyan;
            titleOfProperty.text = "Vermont Avenue";
            rent.text = "RENT $6";
            house1.text = "With 1 House                 $30";
            house2.text = "With 2 Houses                $90";
            house3.text = "With 3 Houses                $270";
            house4.text = "With 4 Houses                $400";
            hotel.text = "With Hotel $550";
            mortgage.text = "Mortgage Value $50";
            houseCost.text = "Houses cost $50 Each";
            hotelCost.text = "Hotels $50 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $100";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 9) // Connecticut Avenue
        {
            section.color = Color.cyan;
            titleOfProperty.text = "Connecticut Avenue";
            rent.text = "RENT $8";
            house1.text = "With 1 House                 $40";
            house2.text = "With 2 Houses                $100";
            house3.text = "With 3 Houses                $300";
            house4.text = "With 4 Houses                $450";
            hotel.text = "With Hotel $600";
            mortgage.text = "Mortgage Value $60";
            houseCost.text = "Houses cost $50 Each";
            hotelCost.text = "Hotels $50 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $120";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 11) //St. Charles Place
        {
            section.color = Color.HSVToRGB(.0814f, .278f, .278f);
            titleOfProperty.text = "St. Charles Place";
            rent.text = "RENT $10";
            house1.text = "With 1 House                 $50";
            house2.text = "With 2 Houses                $150";
            house3.text = "With 3 Houses                $450";
            house4.text = "With 4 Houses                $625";
            hotel.text = "With Hotel $750";
            mortgage.text = "Mortgage Value $70";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $140";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 13) // States Avenue
        {
            section.color = Color.HSVToRGB(.0814f, .278f, .278f);
            titleOfProperty.text = "States Avenue";
            rent.text = "RENT $10";
            house1.text = "With 1 House                 $50";
            house2.text = "With 2 Houses                $150";
            house3.text = "With 3 Houses                $450";
            house4.text = "With 4 Houses                $625";
            hotel.text = "With Hotel $750";
            mortgage.text = "Mortgage Value $70";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $140";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 14) // Virginia Avenue
        {
            section.color = Color.HSVToRGB(.0814f, .278f, .278f);
            titleOfProperty.text = "Virginia Avenue";
            rent.text = "RENT $12";
            house1.text = "With 1 House                 $60";
            house2.text = "With 2 Houses                $180";
            house3.text = "With 3 Houses                $500";
            house4.text = "With 4 Houses                $700";
            hotel.text = "With Hotel $900";
            mortgage.text = "Mortgage Value $80";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $160";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 16) // St. James Place
        {
            Material newMat = Resources.Load("Materials/Material_014", typeof(Material)) as Material; //this is orange. to code, should assign materials instead of colors.
            section.color = Color.HSVToRGB(.108f, .278f, .278f);
            titleOfProperty.text = "St. James Place";
            rent.text = "RENT $14";
            house1.text = "With 1 House                 $70";
            house2.text = "With 2 Houses                $200";
            house3.text = "With 3 Houses                $550";
            house4.text = "With 4 Houses                $750";
            hotel.text = "With Hotel $950";
            mortgage.text = "Mortgage Value $90";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $180";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 18) // Tennessee Avenue
        {
            section.color = Color.HSVToRGB(.108f, .278f, .278f);
            titleOfProperty.text = "Tennessee Avenue";
            rent.text = "RENT $14";
            house1.text = "With 1 House                 $70";
            house2.text = "With 2 Houses                $200";
            house3.text = "With 3 Houses                $550";
            house4.text = "With 4 Houses                $750";
            hotel.text = "With Hotel $950";
            mortgage.text = "Mortgage Value $90";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $180";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 19) // New York Avenue
        {
            section.color = Color.HSVToRGB(.108f, .278f, .278f);
            titleOfProperty.text = "New York Avenue";
            rent.text = "RENT $16";
            house1.text = "With 1 House                 $80";
            house2.text = "With 2 Houses                $220";
            house3.text = "With 3 Houses                $600";
            house4.text = "With 4 Houses                $800";
            hotel.text = "With Hotel $1000";
            mortgage.text = "Mortgage Value $100";
            houseCost.text = "Houses cost $100 Each";
            hotelCost.text = "Hotels $100 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $200";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 21) // Kentucky Avenue
        {
            section.color = Color.red;
            titleOfProperty.text = "Kentucky Avenue";
            rent.text = "RENT $18";
            house1.text = "With 1 House                 $90";
            house2.text = "With 2 Houses                $250";
            house3.text = "With 3 Houses                $700";
            house4.text = "With 4 Houses                $875";
            hotel.text = "With Hotel $1050";
            mortgage.text = "Mortgage Value $110";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $220";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 23) // Indiana Avenue
        {
            section.color = Color.red;
            titleOfProperty.text = "Indiana Avenue";
            rent.text = "RENT $18";
            house1.text = "With 1 House                 $90";
            house2.text = "With 2 Houses                $250";
            house3.text = "With 3 Houses                $700";
            house4.text = "With 4 Houses                $875";
            hotel.text = "With Hotel $1050";
            mortgage.text = "Mortgage Value $110";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $220";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 24) // Illinois Avenue
        {
            section.color = Color.red;
            titleOfProperty.text = "Illinois Avenue";
            rent.text = "RENT $20";
            house1.text = "With 1 House                 $100";
            house2.text = "With 2 Houses                $300";
            house3.text = "With 3 Houses                $750";
            house4.text = "With 4 Houses                $925";
            hotel.text = "With Hotel $1100";
            mortgage.text = "Mortgage Value $120";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $240";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 26) // Atlantic Avenue
        {
            section.color = Color.yellow;
            titleOfProperty.text = "Atlantic Avenue";
            rent.text = "RENT $22";
            house1.text = "With 1 House                 $110";
            house2.text = "With 2 Houses                $330";
            house3.text = "With 3 Houses                $800";
            house4.text = "With 4 Houses                $975";
            hotel.text = "With Hotel $1150";
            mortgage.text = "Mortgage Value $130";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $260";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 27) // Ventnor Avenue
        {
            section.color = Color.yellow;
            titleOfProperty.text = "Ventnor Avenue";
            rent.text = "RENT $22";
            house1.text = "With 1 House                 $110";
            house2.text = "With 2 Houses                $330";
            house3.text = "With 3 Houses                $800";
            house4.text = "With 4 Houses                $975";
            hotel.text = "With Hotel $1150";
            mortgage.text = "Mortgage Value $130";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $260";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 29) // Marvin Gardens
        {
            section.color = Color.yellow;
            titleOfProperty.text = "Marvin Gardens";
            rent.text = "RENT $24";
            house1.text = "With 1 House                 $120";
            house2.text = "With 2 Houses                $360";
            house3.text = "With 3 Houses                $850";
            house4.text = "With 4 Houses                $1025";
            hotel.text = "With Hotel $1200";
            mortgage.text = "Mortgage Value $140";
            houseCost.text = "Houses cost $150 Each";
            hotelCost.text = "Hotels $150 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $280";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 31) // Pacific Avenue
        {
            section.color = Color.green;
            titleOfProperty.text = "Pacific Avenue";
            rent.text = "RENT $26";
            house1.text = "With 1 House                 $130";
            house2.text = "With 2 Houses                $390";
            house3.text = "With 3 Houses                $900";
            house4.text = "With 4 Houses                $1100";
            hotel.text = "With Hotel $1275";
            mortgage.text = "Mortgage Value $150";
            houseCost.text = "Houses cost $200 Each";
            hotelCost.text = "Hotels $200 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $300";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 32) // North Carolina Avenue
        {
            section.color = Color.green;
            titleOfProperty.text = "North Carolina Avenue";
            rent.text = "RENT $26";
            house1.text = "With 1 House                 $130";
            house2.text = "With 2 Houses                $390";
            house3.text = "With 3 Houses                $900";
            house4.text = "With 4 Houses                $1100";
            hotel.text = "With Hotel $1275";
            mortgage.text = "Mortgage Value $150";
            houseCost.text = "Houses cost $200 Each";
            hotelCost.text = "Hotels $200 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $300";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 34) // Pennsylvania Avenue
        {
            section.color = Color.green;
            titleOfProperty.text = "Pennsylvania Avenue";
            rent.text = "RENT $28";
            house1.text = "With 1 House                 $150";
            house2.text = "With 2 Houses                $450";
            house3.text = "With 3 Houses                $1000";
            house4.text = "With 4 Houses                $1200";
            hotel.text = "With Hotel $1400";
            mortgage.text = "Mortgage Value $160";
            houseCost.text = "Houses cost $200 Each";
            hotelCost.text = "Hotels $200 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $320";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 37) // Park Place
        {
            section.color = Color.blue;
            titleOfProperty.text = "Park Place";
            rent.text = "RENT $35";
            house1.text = "With 1 House                 $175";
            house2.text = "With 2 Houses                $500";
            house3.text = "With 3 Houses                $1100";
            house4.text = "With 4 Houses                $1300";
            hotel.text = "With Hotel $1500";
            mortgage.text = "Mortgage Value $175";
            houseCost.text = "Houses cost $200 Each";
            hotelCost.text = "Hotels $200 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $350";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 38)
        {
            string[] values = money.text.Split('$');
            if (75 < int.Parse(values[1]))
            {
                int moneyLeft = int.Parse(values[1]) - 75;
                money.text = "$" + moneyLeft;
            }
            else
            {
                MortgageOrBankrupt(values, 75);
            }
            
        }

        else if (position == 39) // Boardwalk
        {
            section.color = Color.blue;
            titleOfProperty.text = "Boardwalk";
            rent.text = "RENT $50";
            house1.text = "With 1 House                 $200";
            house2.text = "With 2 Houses                $600";
            house3.text = "With 3 Houses                $1400";
            house4.text = "With 4 Houses                $1700";
            hotel.text = "With Hotel $2000";
            mortgage.text = "Mortgage Value $200";
            houseCost.text = "Houses cost $200 Each";
            hotelCost.text = "Hotels $200 Plus 4 Houses";

            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $400";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                Payment(position-1);
            }
            Debug.Log(isOccupied);
        }

        //Railroads                         Railroads                           Railroads

        else if (position == 5) //Reading Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "Reading Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $200";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 50);
                    }
                }
                else
                {
                    if (trainCard && moneyLeft > 50)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 25);
                    }
                }


            }
            Debug.Log(isOccupied);
        }

        else if (position == 15) // Pennsylvania Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "Pennsylvania Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $200";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2))
                {
                    
                    if (trainCard && moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        trainCard = false;
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        trainCard = false;
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }

                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 50);
                    }
                }
                else
                {
                    if (trainCard && moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        trainCard = false;
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 25);
                    }
                }


            }
            Debug.Log(isOccupied);
        }

        else if (position == 25) // B & O Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "B & O Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $200";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        money.text = "$" + (moneyLeft - 400);
                        trainCard = false;
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 50);
                    }
                }
                else
                {
                    if (trainCard && moneyLeft > 50)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player2.money.text = "$" + moneyAft;
                    }
                   
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 25);
                    }
                }


            }
            Debug.Log(isOccupied);
        }

        else if (position == 35) // Short Line
        {
            section.color = Color.black;
            titleOfProperty.text = "Short Line";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            buy.GetComponentInChildren<Text>().text = "Buy for $200";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            //rent
            string[] values = money.text.Split('$');
            int moneyLeft = int.Parse(values[1]);
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 || nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 50);
                    }
                }
                else
                {
                    if (trainCard && moneyLeft > 50)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player2.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player2.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player2.money.text = "$" + moneyAft;
                    }
                    
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 25);
                    }
                }
            }
            Debug.Log(isOccupied);
        }

        //Utilities                 Utilities                   Utilities               Utilities
        else if (position == 12) // Electrical company
        {
            electricalCompany.gameObject.SetActive(true);
            buy.GetComponentInChildren<Text>().text = "Buy for $150";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                notOnUtility = false;
                diceOne.RestartRolling(player1);
                diceTwo.RestartRolling(player1);
                wasNotOnUtility = false;
                notOnUtility = true;

            }
            Debug.Log(isOccupied);
        }

        else if (position == 28) // Water Works
        {
            waterWorks.gameObject.SetActive(true);
            buy.GetComponentInChildren<Text>().text = "Buy for $150";

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                buy.gameObject.SetActive(true);
                auction.gameObject.SetActive(true);
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
            {
                notOnUtility = false;
                diceOne.RestartRolling(player1);
                diceTwo.RestartRolling(player1);

                wasNotOnUtility = false;
                notOnUtility = true;
            }
            Debug.Log(isOccupied);
        }

        //Jail                  Jail                    Jail                    Jail
        else if (position == 30) //Go to Jail
        {
            justGotLocked = true;
            lockedInJail = true;
            StartCoroutine(lockedUp());
            
        }

        player1Index.text = "Player 1 Index: " + (player.index-1);
        hasPlayed = true;
    }

    public void onClickBuy()
    {
        //as soon as get bought, enters in upgrades properties of player
        if (index < 4)
        {
            if (index == 1)
            {
                buttons4PropertiesOwned[0].gameObject.GetComponent<Image>().sprite = violet;
            }
            else if (index == 3)
            {
                buttons4PropertiesOwned[1].gameObject.GetComponent<Image>().sprite = violet;
            }
        }
        else if (index == 5)
        {
            buttons4PropertiesOwned[22].gameObject.GetComponent<Image>().sprite = black;
        }
        else if (index < 10)
        {
            if (index == 6)
            {
                buttons4PropertiesOwned[2].gameObject.GetComponent<Image>().sprite = cyan;
            }
            else if (index == 8)
            {
                buttons4PropertiesOwned[3].gameObject.GetComponent<Image>().sprite = cyan;
            }
            else if (index == 9)
            {
                buttons4PropertiesOwned[4].gameObject.GetComponent<Image>().sprite = cyan;
            }
        }
        else if (index < 15)
        {
            if (index == 11)
            {
                buttons4PropertiesOwned[5].gameObject.GetComponent<Image>().sprite = pink;
            }
            else if (index == 12)
            {
                buttons4PropertiesOwned[26].gameObject.GetComponent<Image>().sprite = white0;
            }
            else if (index == 13)
            {
                buttons4PropertiesOwned[6].gameObject.GetComponent<Image>().sprite = pink;
            }
            else if (index == 14)
            {
                buttons4PropertiesOwned[7].gameObject.GetComponent<Image>().sprite = pink;
            }
        }
        else if (index == 15)
        {
            buttons4PropertiesOwned[23].gameObject.GetComponent<Image>().sprite = black;
        }
        else if (index < 20)
        {
            if (index == 16)
            {
                buttons4PropertiesOwned[8].gameObject.GetComponent<Image>().sprite = orange;
            }
            else if (index == 18)
            {
                buttons4PropertiesOwned[9].gameObject.GetComponent<Image>().sprite = orange;
            }
            else if (index == 19)
            {
                buttons4PropertiesOwned[10].gameObject.GetComponent<Image>().sprite = orange;
            }
        }
        else if (index < 25)
        {
            if (index == 21)
            {
                buttons4PropertiesOwned[11].gameObject.GetComponent<Image>().sprite = red;
            }
            else if (index == 23)
            {
                buttons4PropertiesOwned[12].gameObject.GetComponent<Image>().sprite = red;
            }
            else if (index == 24)
            {
                buttons4PropertiesOwned[13].gameObject.GetComponent<Image>().sprite = red;
            }
        }
        else if (index == 25)
        {
            buttons4PropertiesOwned[24].gameObject.GetComponent<Image>().sprite = black;
        }
        else if (index < 30)
        {
            if (index == 26)
            {
                buttons4PropertiesOwned[14].gameObject.GetComponent<Image>().sprite = yellow;
            }
            else if (index == 27)
            {
                buttons4PropertiesOwned[15].gameObject.GetComponent<Image>().sprite = yellow;
            }
            else if (index == 28)
            {
                buttons4PropertiesOwned[27].gameObject.GetComponent<Image>().sprite = white0;
            }
            else if (index == 29)
            {
                buttons4PropertiesOwned[16].gameObject.GetComponent<Image>().sprite = yellow;
            }
        }
        else if (index < 35)
        {
            if (index == 31)
            {
                buttons4PropertiesOwned[17].gameObject.GetComponent<Image>().sprite = green;
            }
            else if (index == 32)
            {
                buttons4PropertiesOwned[18].gameObject.GetComponent<Image>().sprite = green;
            }
            else if (index == 34)
            {
                buttons4PropertiesOwned[19].gameObject.GetComponent<Image>().sprite = green;
            }
        }
        else if (index == 35)
        {
            buttons4PropertiesOwned[25].gameObject.GetComponent<Image>().sprite = black;
        }
        else
        {
            if (index == 37)
            {
                buttons4PropertiesOwned[20].gameObject.GetComponent<Image>().sprite = blue;
            }
            else if (index == 39)
            {
                buttons4PropertiesOwned[21].gameObject.GetComponent<Image>().sprite = blue;
            }
        }

        string[] split4Value = buy.GetComponentInChildren<Text>().text.Split('$'); // which is smtg like Buy for $200
        string valueOfProperty;
        if (split4Value.Length > 1)
        {
            valueOfProperty = split4Value[1];
        }
        string[] moneyLeft = money.text.Split('$');
        if (int.Parse(moneyLeft[1]) > int.Parse(split4Value[1]))
        {
            nextPositions[index-1].gameObject.GetComponent<Properties>().isOwned = true;
            nextPositions[index-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 = true;
            buy.gameObject.SetActive(false);
            auction.gameObject.SetActive(false);
            money.text = "$"+ (int.Parse(moneyLeft[1]) - int.Parse(split4Value[1]));
        }
        else if (int.Parse(moneyLeft[1]) < int.Parse(split4Value[1]))
        {
            cannotBuy.text = "NOT ENOUGH MONEY";
            //force auction

        }
    }

    public void onClickGetOutOfJail()
    {
        //value of dices b4. The pay 50$ button appears after rolled dices and unsuccessful
        string[] moneyLeft = money.text.Split('$');
        if (jailCard.gameObject.activeInHierarchy)
        {
            index = 10;
            rolls = 0;
            payJail.gameObject.SetActive(false);
            jailCard.gameObject.SetActive(false);
            StartCoroutine(MovePlayerTo(movePlayer, player1));
        }
        
        else if (int.Parse(moneyLeft[1]) > 50)
        {
            money.text = "$" + (int.Parse(moneyLeft[1]) - 50);
            index = 10;
            movePlayer = diceOne.valueOfDice + diceTwo.valueOfDice-1;
            rolls = 0;
            StartCoroutine(MovePlayerTo(movePlayer, player1));
            payJail.gameObject.SetActive(false);
        }
        else
        {
            cannotBuy.text = "NOT ENOUGH MONEY";
        }
    }

    public IEnumerator lockedUp()
    {
        yield return new WaitForSeconds(1);
        player.transform.Translate(new Vector3(inJail.position.x - player.transform.position.x, player.transform.position.z - inJail.position.z, 0));
        nextTurn = true;
    }

    bool pressedEnter()
    {
        return Input.GetKeyUp(KeyCode.Return);
    }

    public void OnClickExchange()
    {

    }

    public void OnClickAuction()
    {
        //auctionPanel.SetActive(true);

    }
    public void OnClickGoBankrupt()
    {
        //end the game
    }

    public void MortgageOrBankrupt(string[] moneyValues, string[] values)
    {
        //Mortgage or bankruptcy
        lackOfMoney = true;
        targetMoney = int.Parse(values[1]);
        targetMoneyToAccumulate.text = "Target : " + targetMoney;
    }

    public void MortgageOrBankrupt(string[] moneyValues, int price)
    {
        //Mortgage or bankruptcy
        lackOfMoney = true;
        targetMoney = price;
        targetMoneyToAccumulate.text = "Target : " + targetMoney;
    }

    void Payment(int position)
    {
        //if owns 1 house
        if (nextPositions[position].gameObject.GetComponent<Properties>().oneHouse)
        {
            string[] values = house1.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }

        }
        //if owns 2 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().twoHouses)
        {
            string[] values = house2.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }

        }
        //if owns 3 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().threeHouses)
        {
            string[] values = house3.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }
        }
        //if owns 4 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().fourHouses)
        {
            string[] values = house4.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }
        }
        else if (nextPositions[position].gameObject.GetComponent<Properties>().hotel)
        {
            string[] values = hotel.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }
        }
        else if (nextPositions[position].gameObject.GetComponent<Properties>().isMortgaged)
        {

        }
        //rent
        else
        {
            string[] values = rent.text.Split('$');
            string[] moneyValues = money.text.Split('$');
            int moneyLeft;
            if (int.Parse(moneyValues[1]) > int.Parse(values[1]))
            {
                moneyLeft = int.Parse(moneyValues[1]) - int.Parse(values[1]);
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player2.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + int.Parse(values[1]);
                player2.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(moneyValues, values);
            }
        }
    }
}

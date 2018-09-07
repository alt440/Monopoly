using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveAI : MonoBehaviour {

    //AI buys immediately everything it can. It still doesnt put houses like P1, and will not know when to put them.

    //Need to tell player what AI did.

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

    public Text player2Index;

    public RawImage railroad;
    public RawImage waterWorks;
    public RawImage electricalCompany;

    public Text cannotBuy;

    public Text money;
    public int value;

    public static bool isFirstTurn = true;

    public DicesRoll diceOne;
    public DicesRoll diceTwo;
    public MoveAI player2;
    public MovePlayer player1;
    public GameObject dices;
    public Camera cameraPlayer;
    public Camera cameraDice1;
    public Camera cameraDice2;
    public Text toContinue;
    public int movePlayer = 0;

    public Transform inJail;
    public Button payJail;
    int rolls = 0;
    public Transform[] nextPositions;
    public int index = 0;

    public bool nextTurn = false;
    bool turnCameraAtZero = false;

    bool hasPlayed = false; //so it doesnt loop Property_Mgmt func
    bool notOnUtility = true;
    bool wasNotOnUtility = true;
    bool switchingPlayers = false;
    bool onlyOnce = false;
    bool onlyOnce4Utilities = false;

    public bool hasMovedFromCard = false;
    public bool utilityCard = false;
    public bool trainCard = false;

    int turns =0;
    bool lackOfMoney = false;
    int targetMoney = 0;
    int valueToPay = 0;

    public bool lockedInJail = false;
    public bool justGotLocked = false;
    public bool justGotOut = false;
    bool onlyOnceInJail = false;

    public Button[] buttons4PropertiesOwned;

    public Image[] housesOnProperty;

    Vector3 fallingPosDice1;
    Vector3 fallingPosDice2;

    // Use this for initialization
    void Start()
    {

        toContinue.text = "";
        player2.money.text = "$1500";

        payJail.GetComponentInChildren<Text>().text = "Pay $50";
        payJail.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (nextTurn && !hasPlayed)
        {
            Properties_Mgmt(index);
        }

        if (diceOne.readyForMove && diceTwo.readyForMove && notOnUtility)
        {
            movePlayer = diceOne.valueOfDice + diceTwo.valueOfDice;
            StartCoroutine(isEnabled());
            if (wasNotOnUtility)
            StartCoroutine(MovePlayerTo(movePlayer, player2));
            else if (lockedInJail)
            {
                rolls++;
                if (diceOne.valueOfDice == diceTwo.valueOfDice)
                {
                    lockedInJail = false;
                    justGotOut = true;
                    player2.index = 10;

                    StartCoroutine(MovePlayerTo(movePlayer, player2));
                }
                nextTurn = true;
            }
            else
            {
                valueToPay = movePlayer;
            }
            diceOne.readyForMove = false;
            diceTwo.readyForMove = false;
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
            if (rolls == 3)
            {
                rolls = 0;
                lockedInJail = false;
                player2.index = 10;
                justGotOut = true;
                StartCoroutine(MovePlayerTo(movePlayer, player2));
            }
        }


        if (valueToPay != 0 && !onlyOnce4Utilities)//4 Electric and WaterWorks
        {
            onlyOnce4Utilities = true;
            string[] emptyPockets = money.text.Split('$');
            
            if (player2.index == 12)
            {
                //if player owns only one utility or two
                if (nextPositions[27].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || utilityCard)
                {
                    valueToPay *= 10;
                    Debug.Log("HO");
                    utilityCard = false;
                }
                else if (!nextPositions[27].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
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
            else if (player2.index == 28)
            {

                //if player owns only one utility or two
                if (nextPositions[11].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || utilityCard)
                {
                    valueToPay *= 10;
                    Debug.Log("HO");
                    utilityCard = false;
                }
                else if (!nextPositions[11].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    valueToPay *= 4;
                    Debug.Log("HA");
                }
                string[] valueMoney = money.text.Split('$');
                if (int.Parse(valueMoney[1]) > valueToPay)
                    money.text = "$" + (int.Parse(valueMoney[1]) - valueToPay);
                else
                {
                    MortgageOrBankrupt(valueMoney, valueToPay);
                }
            }
        }

        if (!diceOne.gameObject.activeInHierarchy && !diceTwo.gameObject.activeInHierarchy && !switchingPlayers) //to position dice b4 reengaging
        {
            if (index < 9 || index == 39)
            {
                diceOne.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x - 200f, player1.dices.transform.position.y, player1.dices.transform.position.z), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x - 200f, player1.dices.transform.position.y + 100f, player1.dices.transform.position.z), 20 * Time.deltaTime);
            }
            else if (index >= 9 && index < 19)
            {
                diceOne.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x, player1.dices.transform.position.y, player1.dices.transform.position.z + 200), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x, player1.dices.transform.position.y + 100f, player1.dices.transform.position.z + 200), 20 * Time.deltaTime);
            }
            else if (index >= 19 && index < 29)
            {
                diceOne.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x + 200f, player1.dices.transform.position.y, player1.dices.transform.position.z), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x + 200f, player1.dices.transform.position.y + 100f, player1.dices.transform.position.z), 20 * Time.deltaTime);
            }
            else if (index >= 29 && index < 39)
            {
                diceOne.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x, player1.dices.transform.position.y, player1.dices.transform.position.z - 200), 20 * Time.deltaTime);
                diceTwo.transform.position = Vector3.Slerp(player1.dices.transform.position, new Vector3(player1.dices.transform.position.x, player1.dices.transform.position.y + 100f, player1.dices.transform.position.z - 200), 20 * Time.deltaTime);
            }

        }

        if (lackOfMoney && !onlyOnce)
        {
            
            onlyOnce = true;
            for (int i = 0; i < nextPositions.Length; i++)
            {
                
                Debug.Log("Mortgage checkpt");
                Mortgage(i);
                string[] values = money.text.Split('$');
                Debug.Log("Mortgage complete");
                if (int.Parse(values[1]) > targetMoney)
                {
                    Debug.Log("value exceeded");
                    money.text = "$" + (int.Parse(values[1]) - targetMoney);
                    string[] valuesP1 = player1.money.text.Split('$');
                    int moneyP1 = int.Parse(valuesP1[1]) + targetMoney;
                    player1.money.text = "$" + moneyP1;
                    lackOfMoney = false;
                    break;
                }
            }
            if (lackOfMoney)
            {
                //END GAME
            }
            
        }

        if (turns %2==0 && turns!=0 && nextTurn && !onlyOnce)
        {
            //METHOD 
            Check4MortgagedProperties();
            AddHouses4AI();
            onlyOnce = true;
        }

        if (nextTurn && Input.GetKeyUp(KeyCode.Return))
        {
            switchingPlayers = true;
            toContinue.text = "";
            cannotBuy.text = "";
            white.gameObject.SetActive(false);
            section.gameObject.SetActive(false);
            railroad.gameObject.SetActive(false);
            waterWorks.gameObject.SetActive(false);
            electricalCompany.gameObject.SetActive(false);
            payJail.gameObject.SetActive(false);
            isFirstTurn = false;
            diceOne.RestartRolling(player1);
            diceTwo.RestartRolling(player1);
            nextTurn = false;
            hasPlayed = false;
            onlyOnce = false;
            onlyOnce4Utilities = false;
            valueToPay = 0;
            onlyOnceInJail = false;
            justGotLocked = false;
            justGotOut = false;

            //so we dont see AI when its player's turn
            player2.gameObject.SetActive(false);
            player1.cameraPlayer.enabled = true;
            player2.cameraPlayer.enabled = false;
            player1.gameObject.SetActive(true);
            player1.money.gameObject.SetActive(true);
            player2.money.gameObject.SetActive(false);
            player1.cameraPlayer.GetComponentInChildren<Light>().enabled = true;
            player2.cameraPlayer.GetComponentInChildren<Light>().enabled = false;
            switchingPlayers = false;
        }

    }

    public IEnumerator MovePlayerTo(int moveValue, MoveAI player)
    {
        yield return new WaitForSeconds(2f);
        while (moveValue > 0)
        {
            if (hasMovedFromCard)
                ++index;
            hasMovedFromCard = false;
            if (index == nextPositions.Length)
                index = 0;
            player.transform.Translate(new Vector3(nextPositions[index].position.x - player.transform.position.x, player.transform.position.z - nextPositions[index].position.z, 0));

            yield return new WaitForSeconds(.7f);
            moveValue--;
            justGotOut = false;

            if (index == nextPositions.Length - 1)
            {
                index = 0;
                string[] moneyValue = money.text.Split('$');
                money.text = "$" + (int.Parse(moneyValue[1]) + 200);
                turns++;
            }

            else
                index++;
        }
        nextTurn = true;
        toContinue.text = "Press Enter to Continue";
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
        diceOne.gameObject.transform.localEulerAngles = new Vector3(0, 0);
        diceTwo.gameObject.transform.localEulerAngles = new Vector3(0, 0);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 60)
                {
                    money.text = "$" + (moneyLeft - 60);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[0].gameObject.GetComponent<Image>().sprite = player1.violet;
                    buttons4PropertiesOwned[0].GetComponentInChildren<Text>().text = "AI";
                }
                
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
            }
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 60)
                {
                    money.text = "$" + (moneyLeft - 60);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[1].gameObject.GetComponent<Image>().sprite = player1.violet;
                    buttons4PropertiesOwned[1].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
            }
            Debug.Log(isOccupied);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 100)
                {
                    money.text = "$" + (moneyLeft - 100);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[2].gameObject.GetComponent<Image>().sprite = player1.cyan;
                    buttons4PropertiesOwned[2].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 100)
                {
                    money.text = "$" + (moneyLeft - 100);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[3].gameObject.GetComponent<Image>().sprite = player1.cyan;
                    buttons4PropertiesOwned[3].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 120)
                {
                    money.text = "$" + (moneyLeft - 120);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[4].gameObject.GetComponent<Image>().sprite = player1.cyan;
                    buttons4PropertiesOwned[4].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 140)
                {
                    money.text = "$" + (moneyLeft - 140);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[5].gameObject.GetComponent<Image>().sprite = player1.pink;
                    buttons4PropertiesOwned[5].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 140)
                {
                    money.text = "$" + (moneyLeft - 140);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[6].gameObject.GetComponent<Image>().sprite = player1.pink;
                    buttons4PropertiesOwned[6].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 160)
                {
                    money.text = "$" + (moneyLeft - 160);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[7].gameObject.GetComponent<Image>().sprite = player1.pink;
                    buttons4PropertiesOwned[7].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
            }
            Debug.Log(isOccupied);
        }

        else if (position == 16) // St. James Place
        {
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 180)
                {
                    money.text = "$" + (moneyLeft - 180);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[8].gameObject.GetComponent<Image>().sprite = player1.orange;
                    buttons4PropertiesOwned[8].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 180)
                {
                    money.text = "$" + (moneyLeft - 180);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[9].gameObject.GetComponent<Image>().sprite = player1.orange;
                    buttons4PropertiesOwned[9].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 200)
                {
                    money.text = "$" + (moneyLeft - 200);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[10].gameObject.GetComponent<Image>().sprite = player1.orange;
                    buttons4PropertiesOwned[10].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 220)
                {
                    money.text = "$" + (moneyLeft - 220);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[11].gameObject.GetComponent<Image>().sprite = player1.red;
                    buttons4PropertiesOwned[11].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 220)
                {
                    money.text = "$" + (moneyLeft - 220);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[12].gameObject.GetComponent<Image>().sprite = player1.red;
                    buttons4PropertiesOwned[12].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 240)
                {
                    money.text = "$" + (moneyLeft - 240);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[13].gameObject.GetComponent<Image>().sprite = player1.red;
                    buttons4PropertiesOwned[13].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 260)
                {
                    money.text = "$" + (moneyLeft - 260);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[14].gameObject.GetComponent<Image>().sprite = player1.yellow;
                    buttons4PropertiesOwned[14].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 260)
                {
                    money.text = "$" + (moneyLeft - 260);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[15].gameObject.GetComponent<Image>().sprite = player1.yellow;
                    buttons4PropertiesOwned[15].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 280)
                {
                    money.text = "$" + (moneyLeft - 280);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[16].gameObject.GetComponent<Image>().sprite = player1.yellow;
                    buttons4PropertiesOwned[16].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 300)
                {
                    money.text = "$" + (moneyLeft - 300);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[17].gameObject.GetComponent<Image>().sprite = player1.green;
                    buttons4PropertiesOwned[17].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 300)
                {
                    money.text = "$" + (moneyLeft - 300);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[18].gameObject.GetComponent<Image>().sprite = player1.green;
                    buttons4PropertiesOwned[18].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 320)
                {
                    money.text = "$" + (moneyLeft - 320);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[19].gameObject.GetComponent<Image>().sprite = player1.green;
                    buttons4PropertiesOwned[19].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 350)
                {
                    money.text = "$" + (moneyLeft - 350);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[20].gameObject.GetComponent<Image>().sprite = player1.blue;
                    buttons4PropertiesOwned[20].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 400)
                {
                    money.text = "$" + (moneyLeft - 400);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[21].gameObject.GetComponent<Image>().sprite = player1.blue;
                    buttons4PropertiesOwned[21].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                Payment(position - 1);
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 200)
                {
                    money.text = "$" + (moneyLeft - 200);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[22].gameObject.GetComponent<Image>().sprite = player1.black;
                    buttons4PropertiesOwned[22].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
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
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy\
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 200)
                {
                    money.text = "$" + (moneyLeft - 200);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[23].gameObject.GetComponent<Image>().sprite = player1.black;
                    buttons4PropertiesOwned[23].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
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
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player1.money.text = "$" + moneyAft;
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 200)
                {
                    money.text = "$" + (moneyLeft - 200);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[24].gameObject.GetComponent<Image>().sprite = player1.black;
                    buttons4PropertiesOwned[24].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[34].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
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
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player1.money.text = "$" + moneyAft;
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 200)
                {
                    money.text = "$" + (moneyLeft - 200);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[25].gameObject.GetComponent<Image>().sprite = player1.black;
                    buttons4PropertiesOwned[25].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                //rent
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 400)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 400);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 400;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 400);
                    }
                    else if (moneyLeft > 200)
                    {
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 200);
                    }
                }
                else if ((nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1) || (nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 && nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1))
                {
                    if (trainCard && moneyLeft > 200)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 200);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 200;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 200);
                    }
                    else if (moneyLeft > 100)
                    {
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    //else mortgage or declare bankruptcy
                    else
                    {
                        MortgageOrBankrupt(values, 100);
                    }
                }
                else if (nextPositions[4].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[14].gameObject.GetComponent<Properties>().isOwnedbyPlayer1 || nextPositions[24].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
                {
                    if (trainCard && moneyLeft > 100)
                    {
                        trainCard = false;
                        money.text = "$" + (moneyLeft - 100);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 100;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 100);
                    }
                    else if (moneyLeft > 50)
                    {
                        money.text = "$" + (moneyLeft - 50);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
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
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 50;
                        player1.money.text = "$" + moneyAft;
                    }
                    else if (trainCard)
                    {
                        trainCard = false;
                        MortgageOrBankrupt(values, 50);
                    }
                    else if (moneyLeft > 25)
                    {
                        money.text = "$" + (moneyLeft - 25);
                        string[] value = player1.money.text.Split('$');
                        int moneyAft = int.Parse(value[1]) + 25;
                        player1.money.text = "$" + moneyAft;
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

            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 150)
                {
                    money.text = "$" + (moneyLeft - 150);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[26].gameObject.GetComponent<Image>().sprite = player1.white0;
                    buttons4PropertiesOwned[26].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                notOnUtility = false;
                diceOne.RestartRolling(player2);
                diceTwo.RestartRolling(player2);

                
                wasNotOnUtility = false;
                notOnUtility = true;
            }
            Debug.Log(isOccupied);
        }

        else if (position == 28) // Water Works
        {
            waterWorks.gameObject.SetActive(true);
            
            Debug.Log(position);
            bool isOccupied = nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned;
            if (isOccupied == false)
            {
                string[] values = money.text.Split('$');
                int moneyLeft = int.Parse(values[1]);
                if (moneyLeft > 150)
                {
                    money.text = "$" + (moneyLeft - 150);
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwned = true;
                    nextPositions[position-1].gameObject.GetComponent<Properties>().isOwnedbyPlayer2 = true;
                    buttons4PropertiesOwned[27].gameObject.GetComponent<Image>().sprite = player1.white0;
                    buttons4PropertiesOwned[27].GetComponentInChildren<Text>().text = "AI";
                }
            }
            else if (isOccupied == true && nextPositions[position].gameObject.GetComponent<Properties>().isOwnedbyPlayer1)
            {
                notOnUtility = false;
                diceOne.RestartRolling(player2);
                diceTwo.RestartRolling(player2);

                wasNotOnUtility = false;
                notOnUtility = true;
            }
            Debug.Log(isOccupied);
        }

        //Jail                  Jail                    Jail                    Jail
        /*else if (position == 30) //Go to Jail
        {
            justGotLocked = true;
            lockedInJail = true;
            StartCoroutine(lockedUp());

        }*/

        player2Index.text = "Player 2 Index: " + (player2.index-1);
        hasPlayed = true;
    }

    IEnumerator lockedUp()
    {
        yield return new WaitForSeconds(1);
        player2.transform.Translate(new Vector3(inJail.position.x - player2.transform.position.x, player2.transform.position.z - inJail.position.z, 0));
        nextTurn = true;
    }

    void AddHouses4AI()
    {
        //check index of player, add 7 so next round could go on it, and start from there
        int i = player1.index + 7;
        if (i > 39)
        {
            i %= 39;
        }
        int moneyLeft = 0;
        string[] values;

        for (; i < nextPositions.Length; i++) // so theres a chance that P1 goes on it
        {
            Debug.Log("Step 1");
            if (i!=1 && i!=3 && i!=6 && i!=9 && i!=11 && i!=16 && i!=19 && i!=21 && i!=27 && i!=29 && i!=32 && i!=35 && i!=37 && i != 39 && i!=4 && i!=14 && i!=24 && i!=34)
            {
                if (nextPositions[i].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    Debug.Log("Step 2");
                    //add house if enough money
                    values = money.text.Split('$');
                    if (int.Parse(values[1]) > 300)
                    {
                        Debug.Log("Step 3");
                        if (i < 10)
                        {
                            //-50$/house
                            Debug.Log("Step 4");
                            moneyLeft = int.Parse(values[1]) - 100;
                            if (nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 500)
                                {
                                    moneyLeft -= 150; //bc 5 X cost of house
                                    nextPositions[i].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            
                            money.text = "$" + moneyLeft;
                            houseIndicator(i);
                        }
                        else if (i < 20)
                        {
                            //-100$/house
                            Debug.Log("Step 4");
                            moneyLeft = int.Parse(values[1]) - 100;
                            if (nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 700)
                                {
                                    moneyLeft -= 400;
                                    nextPositions[i].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().oneHouse = true;
                                
                            }
                            money.text = "$" + moneyLeft;
                            houseIndicator(i);
                        }
                        else if (i < 30)
                        {
                            //-150$/house
                            Debug.Log("Step 4");
                            moneyLeft = int.Parse(values[1]) - 150;
                            if (nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 1000)
                                {
                                    moneyLeft -= 600;
                                    nextPositions[i].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().oneHouse = true;

                            }
                            money.text = "$" + moneyLeft;
                            houseIndicator(i);
                        }
                        else if (i < 40)
                        {
                            //-200$/house
                            Debug.Log("Step 4");
                            moneyLeft = int.Parse(values[1]) - 200;
                            if (nextPositions[i].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 1250)
                                {
                                    moneyLeft -= 800;
                                    nextPositions[i].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[i].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[i].gameObject.GetComponent<Properties>().oneHouse = true;

                            }
                            money.text = "$" + moneyLeft;
                            houseIndicator(i);
                        }
                    }
                }
            }
        }
        /*for (int f = 0; f<i; i++)
        {
            if (f != 2 && f != 4 && f != 7 && f != 10 && f != 12 && f != 17 && f != 20 && f != 22 && f != 28 && f != 30 && f != 33 && f != 36 && f != 38 && f != 0 && f != 5 && f != 15 && f != 25 && f != 35)
            {
                if (nextPositions[f].gameObject.GetComponent<Properties>().isOwnedbyPlayer2)
                {
                    //add house if enough money
                    values = money.text.Split('$');
                    if (int.Parse(values[1]) > 300)
                    {
                        if (f < 10)
                        {
                            //-50$/house
                            moneyLeft = int.Parse(values[1]) - 100;
                            if (nextPositions[f].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 500)
                                {
                                    moneyLeft -= 150; //bc 5 X cost of house
                                    nextPositions[f].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            money.text = "$" + moneyLeft;
                        }
                        else if (f < 20)
                        {
                            //-100$/house
                            moneyLeft = int.Parse(values[1]) - 100;
                            if (nextPositions[f].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 700)
                                {
                                    moneyLeft -= 400;
                                    nextPositions[f].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().oneHouse = true;

                            }
                            money.text = "$" + moneyLeft;
                        }
                        else if (f < 30)
                        {
                            //-150$/house
                            moneyLeft = int.Parse(values[1]) - 150;
                            if (nextPositions[f].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 1000)
                                {
                                    moneyLeft -= 600;
                                    nextPositions[f].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().oneHouse = true;

                            }
                            money.text = "$" + moneyLeft;
                        }
                        else if (f < 40)
                        {
                            //-200$/house
                            moneyLeft = int.Parse(values[1]) - 200;
                            if (nextPositions[f].gameObject.GetComponent<Properties>().fourHouses)
                            {
                                if (int.Parse(values[1]) > 1250)
                                {
                                    moneyLeft -= 800;
                                    nextPositions[f].gameObject.GetComponent<Properties>().hotel = true;
                                }
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().threeHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().fourHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().twoHouses)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().threeHouses = true;
                            }
                            else if (nextPositions[f].gameObject.GetComponent<Properties>().oneHouse)
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().twoHouses = true;
                            }
                            else
                            {
                                nextPositions[f].gameObject.GetComponent<Properties>().oneHouse = true;

                            }
                            money.text = "$" + moneyLeft;
                        }
                    }
                }
            }
        }*/
    }

    void houseIndicator(int k)
    {
        /*for (int i = 0; i < 5; i++)
        {
            housesOnProperty[i].gameObject.SetActive(false);
        }*/
        
        if (player2.nextPositions[k].gameObject.tag == "canHaveHouses")
        {
            Debug.Log("HURRAY");
            
            if (player2.nextPositions[k].GetComponent<Properties>().oneHouse)
            {
                Debug.Log("ENTERS CONDITION");
                player2.nextPositions[k].GetComponent<Properties>().house1.SetActive(true);

                //housesOnProperty[0].gameObject.SetActive(true);
                
            }
            else if (player2.nextPositions[k].GetComponent<Properties>().twoHouses)
            {
                //housesOnProperty[0].gameObject.SetActive(true);
                //housesOnProperty[1].gameObject.SetActive(true);
                Debug.Log("ENTERS CONDITION");
                player2.nextPositions[k].GetComponent<Properties>().house1.SetActive(true);
                player2.nextPositions[k].GetComponent<Properties>().house2.SetActive(true);
            }
            else if (player2.nextPositions[k].GetComponent<Properties>().threeHouses)
            {
                //housesOnProperty[0].gameObject.SetActive(true);
                //housesOnProperty[1].gameObject.SetActive(true);
                //housesOnProperty[2].gameObject.SetActive(true);
                Debug.Log("ENTERS CONDITION");
                player2.nextPositions[k].GetComponent<Properties>().house3.SetActive(true);
            }
            else if (player2.nextPositions[k].GetComponent<Properties>().fourHouses)
            {
                //housesOnProperty[0].gameObject.SetActive(true);
                //housesOnProperty[1].gameObject.SetActive(true);
                //housesOnProperty[2].gameObject.SetActive(true);
                //housesOnProperty[3].gameObject.SetActive(true);
                Debug.Log("ENTERS CONDITION");
                player2.nextPositions[k].GetComponent<Properties>().house4.SetActive(true);
            }
            else if (player2.nextPositions[k].GetComponent<Properties>().hotel)
            {
                //housesOnProperty[4].gameObject.SetActive(true);
                Debug.Log("ENTERS CONDITION");
                player2.nextPositions[k].GetComponent<Properties>().house1.SetActive(false);
                player2.nextPositions[k].GetComponent<Properties>().house2.SetActive(false);
                player2.nextPositions[k].GetComponent<Properties>().house3.SetActive(false);
                player2.nextPositions[k].GetComponent<Properties>().house4.SetActive(false);
                player2.nextPositions[k].GetComponent<Properties>().hotelG.SetActive(true);
            }
        }
        

    }

    public void MortgageOrBankrupt(string[] moneyValues, string[] values)
    {
        //Mortgage or bankruptcy
        lackOfMoney = true;
        targetMoney = int.Parse(values[1]);
    }

    public void MortgageOrBankrupt(string[] moneyValues, int price)
    {
        //Mortgage or bankruptcy
        lackOfMoney = true;
        targetMoney = price;
    }

    void Payment(int position)
    {
        //if owns 1 house
        if (nextPositions[position].gameObject.GetComponent<Properties>().oneHouse)
        {
            string[] values = money.text.Split('$');
            int moneyLeft = 0;
            int cost = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 10;
                cost = 10;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 20;
                cost = 20;
            }
                
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 30;
                cost = 30;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 40;
                cost = 40;
            }
               
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 50;
                cost = 50;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 60;
                cost = 60;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 70;
                cost = 70;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 80;
                cost = 80;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 90;
                cost = 90;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 100;
                cost = 100;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 110;
                cost = 110;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 120;
                cost = 120;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 130;
                cost = 130;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 150;
                cost = 150;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 175;
                cost = 175;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 200;
                cost = 200;
            }
                
            
            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, cost);
            }

        }
        //if owns 2 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().twoHouses)
        {
            string[] values = money.text.Split('$');
            int cost = 0;
            int moneyLeft = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 30;
                cost = 30;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 60;
                cost = 60;
            }
               
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 90;
                cost = 90;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 100;
                cost = 100;
            }
                
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 150;
                cost = 150;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 180;
                cost = 180;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 200;
                cost = 200;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 220;
                cost = 220;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 250;
                cost = 250;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 300;
                cost = 300;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 330;
                cost = 330;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 360;
                cost = 360;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 390;
                cost = 390;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 450;
                cost = 450;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 500;
                cost = 500;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 600;
                cost = 600;
            }
                

            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, cost);
            }
        }
        //if owns 3 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().threeHouses)
        {
            string[] values = money.text.Split('$');
            int cost = 0;
            int moneyLeft = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 90;
                cost = 90;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 180;
                cost = 180;
            }
               
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 270;
                cost = 270;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 300;
                cost = 300;
            }
                
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 450;
                cost = 450;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 500;
                cost = 500;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 550;
                cost = 550;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 600;
                cost = 600;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 700;
                cost = 700;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 750;
                cost = 750;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 800;
                cost = 800;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 850;
                cost = 850;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 900;
                cost = 900;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 1000;
                cost = 1000;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 1100;
                cost = 1100;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 1400;
                cost = 1400;
            }
                

            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, cost);
            }
        }
        //if owns 4 houses
        else if (nextPositions[position].gameObject.GetComponent<Properties>().fourHouses)
        {
            string[] values = money.text.Split('$');
            int cost = 0;
            int moneyLeft = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 160;
                cost = 160;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 320;
                cost = 320;
            }
                
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 400;
                cost = 400;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 450;
                cost = 450;
            }
                
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 625;
                cost = 625;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 700;
                cost = 700;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 750;
                cost = 750;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 800;
                cost = 800;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 875;
                cost = 875;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 925;
                cost = 925;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 975;
                cost = 975;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 1025;
                cost = 1025;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 1100;
                cost = 1100;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 1200;
                cost = 1200;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 1300;
                cost = 1300;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 1700;
                cost = 1700;
            }
                

            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, cost);
            }
        }
        else if (nextPositions[position].gameObject.GetComponent<Properties>().hotel)
        {
            string[] values = money.text.Split('$');
            int cost = 0;
            int moneyLeft = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 250;
                cost = 250;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 450;
                cost = 450;
            }
                
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 550;
                cost = 550;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 600;
                cost = 600;
            }
                
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 750;
                cost = 750;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 900;
                cost = 900;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 950;
                cost = 950;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 1000;
                cost = 1000;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 1050;
                cost = 1050;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 1100;
                cost = 1100;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 1150;
                cost = 1150;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 1200;
                cost = 1200;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 1275;
                cost = 1275;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 1400;
                cost = 1400;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 1500;
                cost = 1500;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 2000;
                cost = 2000;
            }
                

            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, cost);
            }
        }
        else if (nextPositions[position].gameObject.GetComponent<Properties>().isAIMortgaged)
        {

        }
        //rent
        else
        {
            string[] values = money.text.Split('$');
            int cost = 0;
            int moneyLeft = 0;
            if (position == 0)
            {
                moneyLeft = int.Parse(values[1]) - 2;
                cost = 2;
            }
                
            else if (position == 2)
            {
                moneyLeft = int.Parse(values[1]) - 4;
                cost = 4;
            }
                
            else if (position == 5 || position == 7)
            {
                moneyLeft = int.Parse(values[1]) - 6;
                cost = 6;
            }
                
            else if (position == 8)
            {
                moneyLeft = int.Parse(values[1]) - 8;
                cost = 8;
            }
                
            else if (position == 10 || position == 12)
            {
                moneyLeft = int.Parse(values[1]) - 10;
                cost = 10;
            }
                
            else if (position == 13)
            {
                moneyLeft = int.Parse(values[1]) - 12;
                cost = 12;
            }
                
            else if (position == 15 || position == 17)
            {
                moneyLeft = int.Parse(values[1]) - 14;
                cost = 14;
            }
                
            else if (position == 18)
            {
                moneyLeft = int.Parse(values[1]) - 16;
                cost = 16;
            }
                
            else if (position == 20 || position == 22)
            {
                moneyLeft = int.Parse(values[1]) - 18;
                cost = 18;
            }
                
            else if (position == 23)
            {
                moneyLeft = int.Parse(values[1]) - 20;
                cost = 20;
            }
                
            else if (position == 25 || position == 26)
            {
                moneyLeft = int.Parse(values[1]) - 22;
                cost = 22;
            }
                
            else if (position == 28)
            {
                moneyLeft = int.Parse(values[1]) - 24;
                cost = 24;
            }
                
            else if (position == 30 || position == 31)
            {
                moneyLeft = int.Parse(values[1]) - 26;
                cost = 26;
            }
                
            else if (position == 33)
            {
                moneyLeft = int.Parse(values[1]) - 28;
                cost = 28;
            }
                
            else if (position == 36)
            {
                moneyLeft = int.Parse(values[1]) - 35;
                cost = 35;
            }
                
            else if (position == 38)
            {
                moneyLeft = int.Parse(values[1]) - 50;
                cost = 50;
            }
                

            if (moneyLeft > 0)
            {
                money.text = "$" + moneyLeft;
                string[] moneyValues2 = player1.money.text.Split('$');
                int moneyAfter = int.Parse(moneyValues2[1]) + cost;
                player1.money.text = "$" + moneyAfter;
            }
            else
            {
                //Mortgage or bankruptcy
                MortgageOrBankrupt(values, -moneyLeft);
            }
        }
    }

    void Mortgage(int position)
    {
        if (player2.nextPositions[position].GetComponent<Properties>().isOwnedbyPlayer2)
        {
            player2.nextPositions[position].GetComponent<Properties>().isOwnedbyPlayer2 = false;
            player2.nextPositions[position].GetComponent<Properties>().isAIMortgaged = true;
            //mortgaged.text = "Property mortgaged";
            string[] values = money.text.Split('$');
            int moneyLeft = 0;
            Debug.Log(position);
            if (position != 11 && position != 27 && position != 4 && position != 14 && position != 24 && position != 34)
            {
                if (position == 0 || position == 2)
                    moneyLeft = int.Parse(values[1]) + 30;
                else if (position == 5 || position == 7)
                    moneyLeft = int.Parse(values[1]) + 50;
                else if (position == 8)
                    moneyLeft = int.Parse(values[1]) + 60;
                else if (position == 10 || position == 12)
                    moneyLeft = int.Parse(values[1]) + 70;
                else if (position == 13)
                    moneyLeft = int.Parse(values[1]) + 80;
                else if (position == 15 || position == 17)
                    moneyLeft = int.Parse(values[1]) + 90;
                else if (position == 18)
                    moneyLeft = int.Parse(values[1]) + 100;
                else if (position == 20 || position == 22)
                    moneyLeft = int.Parse(values[1]) + 110;
                else if (position == 23)
                    moneyLeft = int.Parse(values[1]) + 120;
                else if (position == 25 || position == 26)
                    moneyLeft = int.Parse(values[1]) + 130;
                else if (position == 28)
                    moneyLeft = int.Parse(values[1]) + 140;
                else if (position == 30 || position == 31)
                    moneyLeft = int.Parse(values[1]) + 150;
                else if (position == 33)
                    moneyLeft = int.Parse(values[1]) + 160;
                else if (position == 36)
                    moneyLeft = int.Parse(values[1]) + 175;
                else if (position == 38)
                    moneyLeft = int.Parse(values[1]) + 200;
                //////////////////////////////////

                    /*string[] mortgageValue = mortgage.text.Split('$');
                    moneyLeft = int.Parse(values[1]) + int.Parse(mortgageValue[1]);*/
            }
            else if (position == 11 || position == 27)
            {
                moneyLeft = int.Parse(values[1]) + 75;
            }
            else if (position == 4 || position == 14 || position == 24 || position == 34)
            {
                moneyLeft = int.Parse(values[1]) + 100;
            }
            money.text = "$" + moneyLeft;
        }
        else if (player2.nextPositions[position].GetComponent<Properties>().isOwnedbyPlayer2 == false && player2.nextPositions[position].GetComponent<Properties>().isAIMortgaged)
        {
            player2.nextPositions[position].GetComponent<Properties>().isOwnedbyPlayer2 = true;
            player2.nextPositions[position].GetComponent<Properties>().isAIMortgaged = false;
            string[] values = money.text.Split('$');
            int moneyLeft=0;
            if (position != 11 && position != 27 && position != 4 && position != 14 && position != 24 && position != 34)
            {
                if (int.Parse(values[1]) > 200)
                {
                    if (position == 0 || position == 2)
                        moneyLeft = int.Parse(values[1]) - 30;
                    else if (position == 5 || position == 7)
                        moneyLeft = int.Parse(values[1]) - 50;
                    else if (position == 8)
                        moneyLeft = int.Parse(values[1]) - 60;
                    else if (position == 10 || position == 12)
                        moneyLeft = int.Parse(values[1]) - 70;
                    else if (position == 13)
                        moneyLeft = int.Parse(values[1]) - 80;
                    else if (position == 15 || position == 17)
                        moneyLeft = int.Parse(values[1]) - 90;
                    else if (position == 18)
                        moneyLeft = int.Parse(values[1]) - 100;
                    else if (position == 20 || position == 22)
                        moneyLeft = int.Parse(values[1]) - 110;
                    else if (position == 23)
                        moneyLeft = int.Parse(values[1]) - 120;
                    else if (position == 25 || position == 26)
                        moneyLeft = int.Parse(values[1]) - 130;
                    else if (position == 28)
                        moneyLeft = int.Parse(values[1]) - 140;
                    else if (position == 30 || position == 31)
                        moneyLeft = int.Parse(values[1]) - 150;
                    else if (position == 33)
                        moneyLeft = int.Parse(values[1]) - 160;
                    else if (position == 36)
                        moneyLeft = int.Parse(values[1]) - 175;
                    else if (position == 38)
                        moneyLeft = int.Parse(values[1]) - 200;

                    money.text = "$" + moneyLeft;
                }
                
            }
            else if (position == 11 || position == 27)
            {
                if (int.Parse(values[1]) > 75)
                {
                    moneyLeft = int.Parse(values[1]) - 75;
                    money.text = "$" + moneyLeft;
                }
            }
            else if (position == 4 || position == 14 || position == 24 || position == 34)
            {
                if (int.Parse(values[1]) > 100)
                {
                    moneyLeft = int.Parse(values[1]) - 100;
                    money.text = "$" + moneyLeft;
                }
            }
        }
    }

    void Check4MortgagedProperties()
    {
        for (int i = 0; i < nextPositions.Length; i++)
        {
            string[] values = money.text.Split('$');
            if (nextPositions[i].GetComponent<Properties>().isAIMortgaged)
            {
                if (int.Parse(values[1]) > 100)
                {
                    if (i != 11 && i != 27 && i != 4 && i != 14 && i != 24 && i != 34)
                    {
                        int moneyLeft = 0;
                        if (i == 0 || i == 2)
                            moneyLeft = int.Parse(values[1]) - 30;
                        else if (i == 5 || i == 7)
                            moneyLeft = int.Parse(values[1]) - 50;
                        else if (i == 8)
                            moneyLeft = int.Parse(values[1]) - 60;
                        else if (i == 10 || i == 12)
                            moneyLeft = int.Parse(values[1]) - 70;
                        else if (i == 13)
                            moneyLeft = int.Parse(values[1]) - 80;
                        else if (i == 15 || i == 17)
                            moneyLeft = int.Parse(values[1]) - 90;
                        else if (i == 18)
                            moneyLeft = int.Parse(values[1]) - 100;
                        else if (i == 20 || i == 22)
                            moneyLeft = int.Parse(values[1]) - 110;
                        else if (i == 23)
                            moneyLeft = int.Parse(values[1]) - 120;
                        else if (i == 25 || i == 26)
                            moneyLeft = int.Parse(values[1]) - 130;
                        else if (i == 28)
                            moneyLeft = int.Parse(values[1]) - 140;
                        else if (i == 30 || i == 31)
                            moneyLeft = int.Parse(values[1]) - 150;
                        else if (i == 33)
                            moneyLeft = int.Parse(values[1]) - 160;
                        else if (i == 36)
                            moneyLeft = int.Parse(values[1]) - 175;
                        else if (i == 38)
                            moneyLeft = int.Parse(values[1]) - 200;

                        money.text = "$" + moneyLeft;
                    }
                    else if (i == 11 || i == 27)
                    {
                        if (int.Parse(values[1]) > 75)
                        {
                            int moneyLeft = int.Parse(values[1]) - 75;
                            money.text = "$" + moneyLeft;
                        }
                    }
                    else if (i == 4 || i == 14 || i == 24 || i == 34)
                    {
                        if (int.Parse(values[1]) > 100)
                        {
                            int moneyLeft = int.Parse(values[1]) - 100;
                            money.text = "$" + moneyLeft;
                        }
                    }
                }
                else
                {
                    break;
                }
                
            }
        }
    }
}


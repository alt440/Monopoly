using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradingMortgage : MonoBehaviour {

	//the player object
    public MovePlayer player;
	//the AI object
    public MoveAI player2;
	//the button that allows the user to buy a house on a property
    public Button buyHouse;
	//the option that allows the user to mortgage the property
    public Button mortgageProperty;
	//the property on which the user fell on
    int propertyChosen =0;
	//the text "Mortgage"
    public Text mortgage;
	//the text "Cannot buy the property"
    public Text cannotBuy;
	//the price of the property
    int priceOfProperty;

	//the top indicator of a property indicating how many houses are on the property
    public Image[] houseIndicators;

	//the scene that appears once the user presses a key to show all the properties he/she owns
    public Button[] buttons4PropertiesOwned;

	//the text "Mortgaged" to show it is currently mortgaged
    public Text mortgaged;
	//the text indicating the money the user owns/ AI owns
    public Text money;

    public InputField amount;
	//the text that indicates if an exchange was accepted
    public Text offerAccepted;
	//the text that indicates if an exchange was denied
    public Text offerDeclined;
	//the button that allows the exchange
    public Button exchange;

	//These text values are all the information shown on the property card of any property.
    public Text rent;
    public Text house1;
    public Text house2;
    public Text house3;
    public Text house4;
    public Text hotel;
    public Text houseCost;
    public Text hotelCost;
    public Text exception;
	//These images are the backgrounds of the card (section is the top of the card, indicating the section of the property,
	//and white is simply a white background to a card.
    public RawImage white;
    public RawImage section; //need to change color of it.
    public Text titleOfProperty;

	//These images are used for the special properties of monopoly, so that once the user falls on them, he has the
	//right image representing the special property.
    public RawImage railroad;
    public RawImage waterWorks;
    public RawImage electricalCompany;

    bool moreInfo = false;

	//to show that an action is not possible.
    public Text actionNotPossible;
    bool resetText = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
       if (resetText)
        {
            actionNotPossible.text = "";
            resetText = false;
        }
       if (Input.GetKeyUp(KeyCode.U))
        {
            actionNotPossible.text = "";

        }

	}

    public void OnClickOpenInfo(int position)
    {
        resetText = true;
        buyHouse.gameObject.SetActive(false);
        mortgageProperty.gameObject.SetActive(false);
        exchange.gameObject.SetActive(false);
        amount.gameObject.SetActive(false);
        offerDeclined.gameObject.SetActive(false);
        offerAccepted.gameObject.SetActive(false);

        moreInfo = true;
        propertyChosen = position;
        Properties_Mgmt(propertyChosen);

        //WTV
        if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().isOwnedbyPlayer1)
        {
            buyHouse.gameObject.SetActive(true);
            mortgageProperty.gameObject.SetActive(true);
        }
        else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().isOwnedbyPlayer2)
        {
            exchange.gameObject.SetActive(true);
            amount.gameObject.SetActive(true);
        }
    }

	//to add a house on the property landed on
    public void OnClickAddHouse()
    {
        Debug.Log("HOUSES");
        Debug.Log(propertyChosen);
        resetText = true;
	    //gets the money left of the player
        string[] values = money.text.Split('$');
        int moneyLeft = int.Parse(values[1]);
        
	    //if the property is not a property that does not allow to add houses, then continue inside the if condition
        if (propertyChosen!=12 && propertyChosen!=28 && propertyChosen!=5 && propertyChosen!=15 && propertyChosen!=25 && propertyChosen != 35)
        {
            string[] houseValue = houseCost.text.Split(new string[] { "$"," " }, System.StringSplitOptions.None);
		
		//grabbing the house and hotel prices
            int housePrice = int.Parse(houseValue[3]);
            int hotelPrice = int.Parse(houseValue[3])*5;
		
		//if conditions to know how many houses are there going to be after the user bought a house. It also
		//removes the right amount of money once the user bought a property.
		//the nextPositions array has all the properties within it
		//if there are already four, then we put a hotel on the property
            if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().fourHouses && moneyLeft>hotelPrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().hotel = true;
                money.text = "$" + (moneyLeft - hotelPrice);
                Debug.Log("4HOUSES");
            }
		//if already three, the total of houses on the property is four.
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().threeHouses && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().fourHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("3HOUSES");
            }
		//if already two, the total of houses on the property is three.
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().twoHouses && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().threeHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("2HOUSES");
            }
		//if already one, the total of houses on the property is two.
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().oneHouse && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().twoHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("HOUSE");
            }

		//if the property already has an hotel, then no action is possible.
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().hotel)
            {
                actionNotPossible.text = "Action not possible";
                Debug.Log("HOTEL");
            }
		//if no house on the property, then put one house on the property
            else if (!player.nextPositions[propertyChosen-1].GetComponent<Properties>().oneHouse && moneyLeft > housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().oneHouse = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("NOTHING");
            }
            else
            {
                actionNotPossible.text = "Action not possible";
            }
        }
	    //if the user fell on a property that does not allow housing, indicate that the action is not possible.
        else
        {
            actionNotPossible.text = "Action not possible";
        }
        //Add positions to put houses
        //Indicates hou much houses on property
        houseIndicator();
    }

    public void OnClickExchange()
    {
        if (amount.GetComponentInChildren<Text>().text != null)
        {
            int number;
            bool result = int.TryParse(amount.GetComponentInChildren<Text>().text, out number);
            if (result)
            {
		    //this calculates the cost that the AI is ready to negotiate for a property. If the value that the user
		    //inputs is below that value that the AI expects, the offer is not accepted.
                string[] valueMoney = player.money.text.Split('$');
                string[] houseValue = houseCost.text.Split(new string[] { "$", " " }, System.StringSplitOptions.None);
                int housePrice = int.Parse(houseValue[3]);
                int cost = priceOfProperty;
                cost += (int)(priceOfProperty * 0.5);
                if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().oneHouse)
                {
                    cost += (housePrice * 2);
                }
                else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().twoHouses)
                {
                    cost += (housePrice * 3);
                }
                else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().threeHouses)
                {
                    cost += (housePrice * 4);
                }
                else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().fourHouses)
                {
                    cost += (housePrice * 7);
                }
                else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().hotel)
                {
                    cost += (housePrice * 14);
                }

                if (int.Parse(valueMoney[1]) > cost)
                {
                    if (int.Parse(amount.GetComponentInChildren<Text>().text) > cost)
                    {
                        //offer Accepted
                        offerDeclined.gameObject.SetActive(false);
                        offerAccepted.gameObject.SetActive(true);
                        player.money.text = "$" + (int.Parse(valueMoney[1]) - cost);
                        string[] valueMoneyP2 = player2.money.text.Split('$');
                        int moneyAft = int.Parse(valueMoneyP2[1]) + cost;
                        player2.money.text = "$" + moneyAft;
                        exchange.gameObject.SetActive(false);
                        player.nextPositions[propertyChosen - 1].GetComponent<Properties>().isOwnedbyPlayer2 = false;
                        player.nextPositions[propertyChosen - 1].GetComponent<Properties>().isOwnedbyPlayer1 = true;
                    }
                    else
                    {
                        offerAccepted.gameObject.SetActive(false);
                        offerDeclined.gameObject.SetActive(true);
                    }
                }
                
                else
                {
                    offerDeclined.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnClickMortgageProperty()
    {
	    //operates the property with the MORTGAGE status.
        Debug.Log("MORTGAGE");
        resetText = true;
        //Add or withdraw money too
        if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().isOwnedbyPlayer1)
        {
            player.nextPositions[propertyChosen-1].GetComponent<Properties>().isOwnedbyPlayer1 = false;
            player.nextPositions[propertyChosen-1].GetComponent<Properties>().isMortgaged = true;
            //mortgaged.text = "Property mortgaged";
            string[] values = money.text.Split('$');
            int moneyLeft = 0;
		
		//these properties have a fixed mortgage rate
            if (propertyChosen != 11 && propertyChosen != 27 && propertyChosen != 4 && propertyChosen != 14 && propertyChosen != 24 && propertyChosen != 34)
            {
                string[] mortgageValue = mortgage.text.Split('$');
                moneyLeft = int.Parse(values[1]) + int.Parse(mortgageValue[1]);
            }
            else if (propertyChosen ==11 || propertyChosen == 27)
            {
                moneyLeft = int.Parse(values[1]) + 75;
            }
            else if (propertyChosen == 4 || propertyChosen == 14 || propertyChosen == 24 || propertyChosen == 34)
            {
                moneyLeft = int.Parse(values[1]) + 100;
            }
            money.text = "$" + moneyLeft;
        }
	    //if the property is already mortgaged, then demortgage it.
        else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().isOwnedbyPlayer1 == false)
        {
            player.nextPositions[propertyChosen-1].GetComponent<Properties>().isOwnedbyPlayer1 = true;
            player.nextPositions[propertyChosen-1].GetComponent<Properties>().isMortgaged = false;
            mortgaged.text = "";
            string[] values = money.text.Split('$');
            if (propertyChosen != 11 && propertyChosen != 27 && propertyChosen != 4 && propertyChosen != 14 && propertyChosen != 24 && propertyChosen != 34)
            {
                string[] mortgageValue = mortgage.text.Split('$');
                if (int.Parse(values[1]) > int.Parse(mortgageValue[1]))
                {
                    int moneyLeft = int.Parse(values[1]) - int.Parse(mortgageValue[1]);
                    money.text = "$" + moneyLeft;
                }
                else
                {
                    cannotBuy.text = "NOT ENOUGH MONEY";
                }
            }
            else if (propertyChosen == 11 || propertyChosen == 27)
            {
                if (int.Parse(values[1]) > 75)
                {
                    int moneyLeft = int.Parse(values[1]) - 75;
                    money.text = "$" + moneyLeft;
                }
                else
                {
                    cannotBuy.text = "NOT ENOUGH MONEY";
                }
            }
            else if (propertyChosen == 4 || propertyChosen == 14 || propertyChosen == 24 || propertyChosen == 34)
            {
                if (int.Parse(values[1]) > 100)
                {
                    int moneyLeft = int.Parse(values[1]) - 100;
                    money.text = "$" + moneyLeft;
                }
                else
                {
                    cannotBuy.text = "NOT ENOUGH MONEY";
                }
            }
        }
    }

	
	//this manages the information shown for every property on the game. All this information is shown
	//on the card that it displayed when the player falls on a property.
    public void Properties_Mgmt(int position)
    {
        railroad.gameObject.SetActive(false);
        electricalCompany.gameObject.SetActive(false);
        waterWorks.gameObject.SetActive(false);
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
            priceOfProperty = 60;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);
            houseIndicator();
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
            priceOfProperty = 60;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 100;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 100;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);
            houseIndicator();
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
            priceOfProperty = 120;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);
            houseIndicator();
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
            priceOfProperty = 140;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);
            houseIndicator();
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
            priceOfProperty = 140;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 160;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 180;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();   
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
            priceOfProperty = 180;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 200;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 220;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 220;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 240;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 260;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 260;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 280;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 300;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 300;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 320;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);
            houseIndicator();
            
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
            priceOfProperty = 350;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();
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
            priceOfProperty = 400;
            white.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            houseIndicator();

            
        }

        //Railroads                         Railroads                           Railroads

        else if (position == 5) //Reading Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "Reading Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            
        }

        else if (position == 15) // Pennsylvania Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "Pennsylvania Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

           
        }

        else if (position == 25) // B & O Railroad
        {
            section.color = Color.black;
            titleOfProperty.text = "B & O Railroad";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            
        }

        else if (position == 35) // Short Line
        {
            section.color = Color.black;
            titleOfProperty.text = "Short Line";

            railroad.gameObject.SetActive(true);
            section.gameObject.SetActive(true);

            
        }

        //Utilities                 Utilities                   Utilities               Utilities
        else if (position == 12) // Electrical company
        {
            electricalCompany.gameObject.SetActive(true);
            section.gameObject.SetActive(false);

            
        }

        else if (position == 28) // Water Works
        {
            waterWorks.gameObject.SetActive(true);
            section.gameObject.SetActive(false);

            
        }


        
    }

	//the houseIndicator method defines how many images of houses or if the image of a hotel will be displayed
	//above the card when the user falls on a property. This depends on how many houses were bought on the property.
    void houseIndicator()
    {
        for (int i = 0; i < 5; i++)
        {
            houseIndicators[i].gameObject.SetActive(false);
        }

        if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().oneHouse)
        {
            houseIndicators[0].gameObject.SetActive(true);
        }
        else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().twoHouses)
        {
            houseIndicators[0].gameObject.SetActive(true);
            houseIndicators[1].gameObject.SetActive(true);
        }
        else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().threeHouses)
        {
            houseIndicators[0].gameObject.SetActive(true);
            houseIndicators[1].gameObject.SetActive(true);
            houseIndicators[2].gameObject.SetActive(true);
        }
        else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().fourHouses)
        {
            houseIndicators[0].gameObject.SetActive(true);
            houseIndicators[1].gameObject.SetActive(true);
            houseIndicators[2].gameObject.SetActive(true);
            houseIndicators[3].gameObject.SetActive(true);
        }
        else if (player.nextPositions[propertyChosen - 1].GetComponent<Properties>().hotel)
        {
            houseIndicators[4].gameObject.SetActive(true);
        }

    }


}

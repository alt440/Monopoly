using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradingMortgage : MonoBehaviour {

    public MovePlayer player;
    public MoveAI player2;
    public Button buyHouse;
    public Button mortgageProperty;
    int propertyChosen =0;
    public Text mortgage;
    public Text cannotBuy;
    int priceOfProperty;

    public Image[] houseIndicators;

    public Button[] buttons4PropertiesOwned;

    public Text mortgaged;
    public Text money;

    public InputField amount;
    public Text offerAccepted;
    public Text offerDeclined;
    public Button exchange;

    public Text rent;
    public Text house1;
    public Text house2;
    public Text house3;
    public Text house4;
    public Text hotel;
    public Text houseCost;
    public Text hotelCost;
    public Text exception;
    public RawImage white;
    public RawImage section; //need to change color of it.
    public Text titleOfProperty;

    public RawImage railroad;
    public RawImage waterWorks;
    public RawImage electricalCompany;

    bool moreInfo = false;

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

    public void OnClickAddHouse()
    {
        Debug.Log("HOUSES");
        Debug.Log(propertyChosen);
        resetText = true;
        string[] values = money.text.Split('$');
        int moneyLeft = int.Parse(values[1]);
        
        if (propertyChosen!=12 && propertyChosen!=28 && propertyChosen!=5 && propertyChosen!=15 && propertyChosen!=25 && propertyChosen != 35)
        {
            string[] houseValue = houseCost.text.Split(new string[] { "$"," " }, System.StringSplitOptions.None);
            int housePrice = int.Parse(houseValue[3]);
            int hotelPrice = int.Parse(houseValue[3])*5;
            if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().fourHouses && moneyLeft>hotelPrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().hotel = true;
                money.text = "$" + (moneyLeft - hotelPrice);
                Debug.Log("4HOUSES");
            }
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().threeHouses && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().fourHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("3HOUSES");
            }
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().twoHouses && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().threeHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("2HOUSES");
            }
            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().oneHouse && moneyLeft>housePrice)
            {
                player.nextPositions[propertyChosen-1].GetComponent<Properties>().twoHouses = true;
                money.text = "$" + (moneyLeft - housePrice);
                Debug.Log("HOUSE");
            }

            else if (player.nextPositions[propertyChosen-1].GetComponent<Properties>().hotel)
            {
                actionNotPossible.text = "Action not possible";
                Debug.Log("HOTEL");
            }
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

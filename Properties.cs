using UnityEngine;
using System.Collections;

public class Properties : MonoBehaviour {

    public bool isOwned=false;
    public bool isOwnedbyPlayer1 = false;
    public bool isOwnedbyPlayer2 = false;

    public bool oneHouse = false;
    public bool twoHouses = false;
    public bool threeHouses = false;
    public bool fourHouses = false;
    public bool hotel = false;

    public bool canHaveHouses = true;

    public MovePlayer player;

    public GameObject house1;
    public GameObject house2;
    public GameObject house3;
    public GameObject house4;
    public GameObject hotelG;
    public GameObject mortgaged;

    public bool isMortgaged;
    public bool isAIMortgaged;

	// Use this for initialization
	void Start () {

        if (house1 != null)
        {
            house1.SetActive(false);
            house2.SetActive(false);
            house3.SetActive(false);
            house4.SetActive(false);
            hotelG.SetActive(false);
        }
        

        isOwned = false;

        if (player.index == 1 || player.index ==3 || player.index == 5 || player.index == 8 || player.index == 11 || player.index == 18 || player.index == 21 || player.index == 23 || player.index == 29 || player.index == 31 || player.index == 34 || player.index == 37 || player.index == 39)
        {
            isOwned = true;
            canHaveHouses = false;
        }
    }

    void Update()
    {
        //if user asks for one house to be put, this is where it will receive the demand.
        //for each operation, add in upgrade panel the information concerning property (how much houses/hotel)
        if (house1 != null)
        {
            if (hotel)
            {
                house4.SetActive(false);
                house3.SetActive(false);
                house2.SetActive(false);
                house1.SetActive(false);
                hotelG.SetActive(true);
                fourHouses = false;
            }
            else if (fourHouses)
            {
                house4.SetActive(true);
                threeHouses = false;
            }
            else if (threeHouses)
            {
                //add house at 4th transform
                house3.SetActive(true);
                twoHouses = false;
            }
            else if (twoHouses)
            {
                //add house at 2nd transform
                house2.SetActive(true);
                oneHouse = false;
            }
            else if (oneHouse)
            {
                //add house at 2nd transform
                house1.SetActive(true);
            }
        }

        if (mortgaged!=null) // is condition good?YES
        {
            if (isMortgaged || isAIMortgaged)
            {
                mortgaged.SetActive(true);
            }
            else
            {
                mortgaged.SetActive(false);
            }
        }
        
        

    }

}

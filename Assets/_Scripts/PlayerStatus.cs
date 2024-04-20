using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public enum Item
    {
        NONE, ROPE, COIN_MULT, DAMAGE_MULT, INSTAKILL, NO_DAMAGE, LIMITED_SIGHT
    }

    public static int NUMBER_OF_BUFFS = 3;
    public static int NUMBER_OF_DEBUFFS = 3;

    //the transform of the player
    //public Transform playerTransform;

    //Permanent multipliers that can be increased by the shopkeeper
    public int PERMANENT_DAMAGE_MULTIPLIER;
    public static int PERMANENT_COIN_MULTIPLIER;

    //Temporary Buffs
    public int tempDamageMult;
    public static int tempCoinMult;

    //Debuffs
    public int damageDebuff;
    public static bool isInverted;

    //Current Item
    private static Item currentItem;

    //time when item was grabbed.
    private static int roomNumItemGrabbed;


    //number of coins the player has
    public static int coinCount;

    // Start is called before the first frame update
    void Start()
    {
        PERMANENT_DAMAGE_MULTIPLIER = 1;
        PERMANENT_COIN_MULTIPLIER = 1;

        damageDebuff = 0;

        roomNumItemGrabbed = -1;
        
        tempDamageMult = 1;
        tempCoinMult = 1;

        isInverted = false;

        currentItem = Item.NONE;

        coinCount = 0;

        ////TEST CODE: IF YOU WANT TO TEST YOUR STATUS, JUST CHANGE
        ////"currentItem" TO BE WHAT YOU WANT TO TEST!!!
        ////EX) currentItem = Item.ROPE;

        currentItem = Item.COIN_MULT;
        roomNumItemGrabbed = LoaderBorder.roomCount;
        tempCoinMult = 2;
        coinCount = 10;
        
        //////////////
    }

    // Update is called once per frame
    void Update()
    {
        if(roomNumItemGrabbed != -1 && roomNumItemGrabbed + 2 == LoaderBorder.roomCount){
            roomNumItemGrabbed = -1;
            switch (currentItem)
            {
                case Item.ROPE:
                    //rope logic
                    break;
                case Item.COIN_MULT:
                    //coin logic
                    removeCoinMultiplier();
                    break;
                case Item.DAMAGE_MULT:
                    //damage logic
                    break;
                //we don't have a "none" check since nothing happens
            }
        }

        if (isInverted && roomNumItemGrabbed + 1 == LoaderBorder.roomCount)
        {
            isInverted = false;
        }

        if (isInverted && roomNumItemGrabbed + 1 == LoaderBorder.roomCount)
        {
            isInverted = false;
        }
    }

    public static void setItem(Item i)
    {
        currentItem = i;
        if(currentItem != Item.NONE)
        {
            roomNumItemGrabbed = LoaderBorder.roomCount;
        }
    }

    public static Item getItem()
    {
        return currentItem;
    }
/*    private void addCoinMultiplier()
    {
        tempCoinMult = 2;
        roomNumItemGrabbed = LoaderBorder.roomCount;
        setItem(Item.COIN_MULT);
    }*/

    private void removeCoinMultiplier() { 
        tempCoinMult = 1;
        setItem(Item.NONE);
    }

    private void activateInvertedControls()
    {
        isInverted = true;
        roomNumItemGrabbed = LoaderBorder.roomCount;
    }

    public static void addCoin()
    {
        coinCount += 1 * PERMANENT_COIN_MULTIPLIER * tempCoinMult;
        Debug.Log("coin Count == " + coinCount);
    }

    public static void subtractCoins()
    {

    }
}

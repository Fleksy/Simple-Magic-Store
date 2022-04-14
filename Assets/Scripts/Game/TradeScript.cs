using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TradeScript : MonoBehaviour
{
    public InventoryScript inventory;
    public GameManagerScript gameManager;
    public CharacterScript character;
    public List<ItemScript> itemsBuy;
    public List<ItemScript> itemsSell;
    private int[] index = new int[8];
    public Transform buyPanel;
    public Transform sellPanel;
    private int countBuy;
    private int countSell;


    private void OnEnable()
    {
        transform.GetChild(2).GetComponent<Text>().text = "Money: " + character.money.ToString();
        for (int i = 0; i < countBuy; i++)
        {
            buyPanel.GetChild(i).GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0, inventory.CheckItem(itemsBuy[index[i]].name) ? 1f : 0.5f);
            buyPanel.GetChild(i).GetChild(1).GetComponent<Text>().color = new Color(0, 0, 0, inventory.CheckItem(itemsBuy[index[i]].name) ? 1f : 0.5f);
        }

        for (int i = 0; i < countSell; i++)
        {
            int cost = int.Parse(sellPanel.GetChild(i).GetChild(1).GetComponent<Text>().text);
            sellPanel.GetChild(i).GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0, character.money >= cost ? 1f : 0.5f);
            sellPanel.GetChild(i).GetChild(1).GetComponent<Text>().color = new Color(0, 0, 0, character.money>=cost ? 1f : 0.5f);
        }
    }

    private void Start()
    {
        itemsBuy.Add(new ItemScript("Зелье лечения", "item", 30));
        itemsBuy.Add(new ItemScript("Зелье магии", "item", 30));
        itemsBuy.Add(new ItemScript("Зелье второго дыхания", "item", 65));
        itemsBuy.Add(new ItemScript("Зелье силы", "item", 25));
        itemsBuy.Add(new ItemScript("Зелье брони", "item", 25));
        itemsBuy.Add(new ItemScript("Огненный меч", "item", 80));
        itemsBuy.Add(new ItemScript("Доспех защиты", "item", 100));

        itemsSell.Add(new ItemScript("Железная руда", "item", 20));
        itemsSell.Add(new ItemScript("Древесина", "item", 10));
        itemsSell.Add(new ItemScript("Старый доспех", "item", 40));
        GenerateTradeOffer();
        OnEnable();
    }

    public void GenerateTradeOffer()
    {
        countBuy = Random.Range(3, 6);
        for (int i = 0; i < 5; i++)
        {
            int rnd = Random.Range(0, itemsBuy.Count);
            index[i] = rnd;
            if (i < countBuy)
            {
                buyPanel.GetChild(i).gameObject.SetActive(true);
                buyPanel.GetChild(i).GetChild(0).GetComponent<Text>().text = itemsBuy[rnd].name;
                buyPanel.GetChild(i).GetChild(1).GetComponent<Text>().text = itemsBuy[rnd].cost.ToString();
            }
            else
            {
                buyPanel.GetChild(i).gameObject.SetActive(false);
            }
        }

        countSell = Random.Range(1, 4);
        for (int i = 0; i < 3; i++)
        {
            int rnd = Random.Range(0, itemsSell.Count);
            index[i + 5] = rnd;
            if (i < countSell)
            {
                sellPanel.GetChild(i).gameObject.SetActive(true);
                sellPanel.GetChild(i).GetChild(0).GetComponent<Text>().text = itemsSell[rnd].name;
                sellPanel.GetChild(i).GetChild(1).GetComponent<Text>().text = itemsSell[rnd].cost.ToString();
            }
            else
            {
                sellPanel.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    public void BuyItem(int n)
    {
        int cost = int.Parse(buyPanel.GetChild(n).GetChild(1).GetComponent<Text>().text);
        if (inventory.CheckItem(itemsBuy[index[n]].name))
        {
            character.money += cost;
            transform.GetChild(2).GetComponent<Text>().text = "Money: " + character.money.ToString();
            inventory.TakeFromInventory(itemsBuy[index[n]].name, 1);
            buyPanel.GetChild(n).gameObject.SetActive(false);
        }
        else
        {
            gameManager.ShowMessage("Вы не можете продать этот предмет");
        }
    }

    public void SellItem(int n)
    {
        int cost = int.Parse(sellPanel.GetChild(n - 5).GetChild(1).GetComponent<Text>().text);

        if (character.money >= cost)
        {
            inventory.AddItem(itemsSell[index[n]]);
            character.money -= cost;
            transform.GetChild(2).GetComponent<Text>().text = "Money: " + character.money.ToString();
            sellPanel.GetChild(n - 5).gameObject.SetActive(false);
        }
        else
        {
            gameManager.ShowMessage("Не хватает денег");
        }
    }
}
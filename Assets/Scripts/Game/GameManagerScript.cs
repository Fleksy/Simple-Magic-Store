using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour
{
    private double weedTime;
    private double customerTime;
    public GameObject messagePanel;
    public InventoryScript inventory;
    public Tilemap groundMap;
    public Tilemap plantsMap;
    public Tile weedTile;
    public GameObject customerPref;
    public List<GameObject> customers;
    public PlantScript[] plants;

   

    void Update()
    {
        weedTime += Time.deltaTime;
        customerTime += Time.deltaTime;
        if (weedTime > 30)
        {
            weedTime = 0;
            CreateWeed();
        }

        if (customerTime > 10)
        {
            customerTime = 0;
            if (customers.Count == 5)
            {
                //ShowMessage("Очередь переполнена, вы теряете клиентов!");
            }
            else
            {
                ShowMessage("Пришел новый посетитель!");
                CreateCustomer();
            }
        }
    }


    public void ShowMessage(string text)
    {
        Transform message = messagePanel.transform.GetChild(0);
        message.transform.SetParent(null);
        message.transform.SetParent(messagePanel.transform);
        message.transform.GetChild(0).GetComponent<Text>().text = text;
        message.GetComponent<MessageScript>().Show();
    }

    void CreateWeed()
    {
        Vector3Int pos = new Vector3Int(Random.Range(19, 47), Random.Range(-10, 6), 0);
        if (groundMap.GetTile(pos).name == "Grass" && plantsMap.GetTile(pos) == null)
        {
            plantsMap.SetTile(pos, weedTile);
        }
    }

    public void GiveSeed()
    {
        int rand = Random.Range(0, plants.Length + plants.Length / 2);
        if (rand < plants.Length)
        {
            inventory.AddItem(new ItemScript(plants[rand].seed, "plant", plants[rand].prefab));
        }
    }

    private void CreateCustomer()
    {
        GameObject newcustomer = Instantiate(customerPref);
        newcustomer.GetComponent<CustomerScript>().gamaManager = transform.GetComponent<GameManagerScript>();
        customers.Add(newcustomer);
        newcustomer.GetComponent<CustomerScript>().GoBeginOfQueue();
        if (customers.Count == 1)
            customers.ToArray()[customers.Count - 1].GetComponent<CustomerScript>().posFinal = 7;
        else
        {
            customers.ToArray()[customers.Count - 1].GetComponent<CustomerScript>().posFinal =
                customers.ToArray()[customers.Count - 2].GetComponent<CustomerScript>().posFinal - 1;
        }
    }

    public void MoveQueue()
    {
        customers[0].GetComponent<CustomerScript>().GoAway();
        for (int i = 1; i < customers.Count; i++)
        {
            customers[i].GetComponent<CustomerScript>().posFinal++;
            customers[i].GetComponent<CustomerScript>().moveFlag = true;
        }
    }
}
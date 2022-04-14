using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public Transform itemPrefab;
    public GameManagerScript gameManager;

    public void AddItem(ItemScript item)
    {
        Transform obj = transform.Find(item.name);
        if (obj != null)
        {
            Text count = obj.GetChild(0).GetComponent<Text>();
            count.text = (int.Parse(count.text) + 1).ToString();
        }
        else
        {
            Transform newItem = Instantiate(itemPrefab, transform);
            newItem.name = item.name;
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.transform.GetChild(1).GetComponent<Text>().text = item.name;
            newItem.transform.GetChild(0).GetComponent<Text>().text = "1";
            newItem.GetComponent<ItemScript>().name = item.name;
            newItem.GetComponent<ItemScript>().type = item.type;
            newItem.GetComponent<ItemScript>().tile = item.tile;
            newItem.GetComponent<ItemScript>().cost = item.cost;
        }
        gameManager.ShowMessage("Получен предмет: "+ item.name );
    }

   public  void TakeFromInventory(string name, int takeCount)
   {
       Transform obj = transform.Find(name);
       Text count = obj.GetChild(0).GetComponent<Text>();
       count.text = (int.Parse(count.text) - takeCount).ToString(); 
       if (int.Parse(count.text) <= 0) 
           DestroyImmediate(obj.gameObject);
       gameManager.ShowMessage("Потерян предмет: "+name );
   }
   
   public bool CheckItem(string item)
   {
       for (int i = 0; i < transform.childCount; i++)
       {
           if (item == transform.GetChild(i).name)
           {
               return true;
           }
       }
       return false;
   }
   
}
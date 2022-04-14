using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RecipeScript : MonoBehaviour
{
    public InventoryScript inventory;
    public string itemName;
    public string itemType;
    public int cost;
    public  string[] components;
    private Outline outline;
    private GameObject componentsPanel;
    //private static bool canCraft;
    private Color color;

    private void Start()
    {
        componentsPanel = transform.GetChild(1).gameObject;
        foreach (var str in components)
        {
            componentsPanel.transform.GetChild(0).GetComponent<Text>().text += str + "\n";
        }
        componentsPanel.transform.GetChild(0).GetComponent<Text>().text += "Цена: "+cost;
        color = transform.GetChild(0).GetComponent<Text>().color;
        outline = transform.GetComponent<Outline>();
        outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0);
       
    }

    private void Update()
    {
        transform.GetChild(0).GetComponent<Text>().color =
            new Color(color.r, color.g, color.b, CheckComponents() ? 1f : 0.5f);
    }


    private void OnDisable()
    {
        componentsPanel.SetActive(false);
        componentsPanel.transform.SetParent(this.transform);
    }


    public void CreateItem()
    {
        if (CheckComponents())
        {
            inventory.GetComponent<InventoryScript>().AddItem(new ItemScript(itemName, itemType, cost));
            foreach (var component in components)
            {
                inventory.TakeFromInventory(component, 1);
                Thread.Sleep(10);
            }
            outline.effectColor = Color.green;
        }
        else
        {
            outline.effectColor = Color.red;
        }
        
        
        transform.GetChild(0).GetComponent<Text>().color = new Color(color.r, color.g, color.b, CheckComponents() ? 1f : 0.5f);
        StopAllCoroutines();
        StartCoroutine("FadingColor");
    }

    bool CheckComponents()
    {
        foreach (var component in components)
        {
            if (!inventory.CheckItem(component))
                return false;
        }
        return true;
    }

   

    IEnumerator FadingColor()
    {
        for (float f = 1f; f > -0.1; f -= 0.1f)
        {
            outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, f);
            yield return new WaitForSeconds(0.1f);
        }
    }


    private void OnMouseOver()
    {
        componentsPanel.SetActive(true);
        componentsPanel.transform.SetParent(GameObject.Find("Canvas").transform);
        componentsPanel.transform.position = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x +
            componentsPanel.GetComponent<RectTransform>().rect.width / 200,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y -
            componentsPanel.GetComponent<RectTransform>().rect.height / 200 - 0.2f, 0);
    }

    private void OnMouseExit()
    {
        componentsPanel.SetActive(false);
        componentsPanel.transform.SetParent(this.transform);
    }
}
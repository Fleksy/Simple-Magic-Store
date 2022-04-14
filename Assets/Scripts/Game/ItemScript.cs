using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public new string name;
    public string type;
    public string tile;
    public int cost;

    public ItemScript(string name, string type, int cost = 0)
    {
        this.name = name;
        this.type = type;
        this.cost = cost;
    }

    public ItemScript( string name, string type, string tile = "")
    {
        this.name = name;
        this.type = type;
        this.tile = tile;
    }

    public void SetSelectedItem()
    {
        CharacterScript character = GameObject.Find("Character").GetComponent<CharacterScript>();
        if (character.selectedItem != null)
        {
            character.selectedItem.GetComponent<Outline>().enabled = false;
        }

        character.selectedItem = this.gameObject;
        character.SetSelectedTool("Item");
        character.selectedItem.GetComponent<Outline>().enabled = true;
    }
}
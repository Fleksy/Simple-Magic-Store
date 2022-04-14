using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public GameManagerScript gameManager;
    public Tilemap groundMap;
    public Tilemap plantsMap;
    public GameObject inventory;
    public GameObject craftTable;
    public GameObject tradePanel;
    public GameObject settingsPanel;
    private string selectedTool;
    public GameObject selectedItem;
    public GameObject toolsPanels;
    private Animator animator;
    public int money = 100;
    private int speed = 20;
    private bool tradeZone;
    private Vector3Int posInt;
    private Vector3Int offset;

    void Start()
    {
        settingsPanel.SetActive(false);
        animator = transform.GetComponent<Animator>();
        inventory.GetComponent<InventoryScript>().AddItem(new ItemScript("Лазуритовая лоза", "plant",  "BluePlantTile"));
        inventory.GetComponent<InventoryScript>().AddItem(new ItemScript("Кровоцвет", "plant", "RedPlantTile"));
        inventory.GetComponent<InventoryScript>().AddItem(new ItemScript("Слизневик", "plant",  "SlimePlantTile"));
        inventory.GetComponent<InventoryScript>().AddItem(new ItemScript("Гриб", "plant", "MushroomPlantTile"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventory.activeSelf)
                inventory.SetActive(false);
            else if (craftTable.activeSelf)
                craftTable.SetActive(false);
            else if (settingsPanel.activeSelf)
                settingsPanel.SetActive(false);
            else
            {
               settingsPanel.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.SetActive(!inventory.activeSelf);
            craftTable.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            craftTable.SetActive(!craftTable.activeSelf);
            inventory.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (tradePanel.gameObject.activeSelf)
                tradePanel.SetActive(false);
            else if (tradeZone && offset == Vector3Int.up && gameManager.customers.Count > 0)
                tradePanel.SetActive(true);
        }

        else if (!inventory.activeSelf && !craftTable.activeSelf)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.F))
            {
                posInt = Vector3Int.FloorToInt(transform.position) + Vector3Int.down + offset;
                switch (selectedTool)
                {
                    case "Shovel":
                        Dig();
                        break;
                    case "Water":
                        Water();
                        break;
                    case "Sickle":
                        Harvest();
                        break;
                    case "Item":
                        Plant();
                        break;
                }
            }
        }
    }

    private void Move()
    {
        animator.SetBool("walkUp", Input.GetKey(KeyCode.W) && !animator.GetBool("walkLeft") &&
                                   !animator.GetBool("walkRight") &&
                                   !animator.GetBool("walkDown"));

        animator.SetBool("walkRight", Input.GetKey(KeyCode.D) && !animator.GetBool("walkLeft") &&
                                      !animator.GetBool("walkUp") &&
                                      !animator.GetBool("walkDown"));

        animator.SetBool("walkDown", (Input.GetKey(KeyCode.S) && !animator.GetBool("walkLeft") &&
                                      !animator.GetBool("walkUp") &&
                                      !animator.GetBool("walkRight")));

        animator.SetBool("walkLeft", Input.GetKey(KeyCode.A) && !animator.GetBool("walkDown") &&
                                     !animator.GetBool("walkUp") &&
                                     !animator.GetBool("walkRight"));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A))
        {
            offset = Vector3Int.zero;
            if (animator.GetBool("walkUp"))
                offset = Vector3Int.up;
            if (animator.GetBool("walkLeft"))
                offset = Vector3Int.left;
            if (animator.GetBool("walkRight"))
                offset = Vector3Int.right;
            if (animator.GetBool("walkDown"))
                offset = Vector3Int.down;
        }

        GetComponent<Rigidbody2D>().MovePosition(transform.position +
                                                 new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
                                                     0) * (Time.deltaTime * speed));
    }

    public void SetSelectedTool(string tool)
    {
        if (selectedTool != null)
            GameObject.Find(selectedTool).GetComponent<Outline>().enabled = false;
        selectedTool = tool;
        GameObject.Find(selectedTool).GetComponent<Outline>().enabled = true;
    }

    void Dig()
    {
        if (plantsMap.GetTile(posInt) != null)
        {
            Harvest();
        }

        if ((groundMap.GetTile(posInt).name == "Grass"))
            groundMap.SetTile(posInt, Resources.Load<Tile>("Tiles/Ground/Farmland"));
        else if ((groundMap.GetTile(posInt).name == "Farmland"))
            groundMap.SetTile(posInt, Resources.Load<Tile>("Tiles/Ground/Grass"));
    }

    void Water()
    {
        if (plantsMap.GetTile(posInt) != null)
        {
            PlantScript plant = plantsMap.GetInstantiatedObject(posInt).GetComponent<PlantScript>();
            if (plant.waterBalance <= 0)
            {
                plant.transform.GetChild(0).gameObject.SetActive(false);
                plant.transform.GetChild(plant.growthStage).gameObject.SetActive(true);
            }

            plant.waterBalance = 60;
        }
    }

    void Harvest()
    {
        if (plantsMap.GetTile(posInt) != null)
        {
            if (plantsMap.GetTile(posInt).name == "Weed")
            {
                gameManager.GiveSeed();
                plantsMap.SetTile(posInt, null);
            }
            else
            {
                PlantScript plant = plantsMap.GetInstantiatedObject(posInt).GetComponent<PlantScript>();
                if (plant.growthStage == 3)
                {
                    inventory.GetComponent<InventoryScript>().AddItem(new ItemScript(plant.seed, "plant", plant.prefab));
                    inventory.GetComponent<InventoryScript>().AddItem(new ItemScript(plant.loot, "item", 0));
                }
                else
                {
                    inventory.GetComponent<InventoryScript>().AddItem(new ItemScript(plant.seed, "plant", plant.prefab));
                }

                plantsMap.SetTile(posInt, null);
            }
        }
    }

    void Plant()
    {
        if ((selectedItem != null) && (selectedItem.GetComponent<ItemScript>().type == "plant") &&
            (groundMap.GetTile(posInt).name == "Farmland") && (plantsMap.GetTile(posInt) == null))
        {
            plantsMap.SetTile(posInt,
                Resources.Load<PlantTile>("Tiles/Plants/" + selectedItem.GetComponent<ItemScript>().tile));
            inventory.GetComponent<InventoryScript>().TakeFromInventory(selectedItem.name, 1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "Entry")
        {
            toolsPanels.SetActive(false);
        }
        else if (col.transform.name == "TradeZone")
        {
            tradeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.name == "Entry" && col.transform.position.y > transform.position.y)
        {
            toolsPanels.SetActive(true);
        }
        else if (col.transform.name == "TradeZone")
        {
            tradeZone = false;
        }
    }
}
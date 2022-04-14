
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int growthTime;
    public int growthStage = 1;
    private double time;
    public double waterBalance;
    public string seed;
    public string loot;
    public string prefab;
    
    
    // Update is called once per frame
    void Update()
    {
        if (waterBalance > 0)
        {
            waterBalance -= Time.deltaTime;
            if (growthStage < 3)
            {
                time += Time.deltaTime;
                if (time >= growthTime)
                {
                    time = 0;
                    transform.GetChild(growthStage).transform.gameObject.SetActive(false);
                    growthStage++;
                    transform.GetChild(growthStage).transform.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            transform.GetChild(growthStage).transform.gameObject.SetActive(false);
            transform.GetChild(0).transform.gameObject.SetActive(true);
        }
    }

    
}
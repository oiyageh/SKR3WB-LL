using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{

    [Header("Spawn Condition Checkers")]
    public bool canSpawn = true;
    public float coolDownTime;
    public float counter;
    public GameObject customerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn)
        {
            if (counter >= coolDownTime)
            {
                canSpawn = true;
            }
            else
            {
                counter += Time.deltaTime;
            }
        }
        if (canSpawn) SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        Instantiate(customerPrefab);
        canSpawn = false;
        counter = 0;
    }
}

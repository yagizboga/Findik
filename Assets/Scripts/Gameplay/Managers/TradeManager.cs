using UnityEngine;

public class TradeManager : MonoBehaviour
{
    [SerializeField] CustomerScriptableObject customerData;
    [SerializeField] GameObject customer;
    [SerializeField] Transform customerFirstDestination;
    [SerializeField] Transform customerSpawnPosition;

    public void SpawnTrade(){
        GameObject newCustomer = GameObject.Instantiate(customer,customerSpawnPosition.position,Quaternion.identity);
        newCustomer.GetComponent<Customer>().SetCustomerData(customerData);
        newCustomer.GetComponent<Customer>().SetFirstDestination(customerFirstDestination);
    }
}

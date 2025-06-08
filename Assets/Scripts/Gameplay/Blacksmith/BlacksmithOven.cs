using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BlacksmithOven : MonoBehaviour
{
    [SerializeField] GameObject meltedironpartice;
    [SerializeField] Transform meltedironspawn;
    bool ismelting = false;
    public void MeltIron()
    {
        StartCoroutine(IColorChanger(5));
        int ironCount = GameObject.FindGameObjectWithTag("irons").GetComponent<BlacksmithIron>().GetIronCount();
        GameObject.FindGameObjectWithTag("irons").GetComponent<BlacksmithIron>().SetIronCount(ironCount - 1);
        ismelting = true;

        StartCoroutine(spawnMeltedIron());

    }
    public IEnumerator IColorChanger(int seconds)
    {
        gameObject.GetComponent<Image>().color = new Color32(100, 15, 0, 255);
        yield return new WaitForSeconds(seconds);
        ismelting = false;
        gameObject.GetComponent<Image>().color = new Color32(40, 15, 0, 255);
    }
    public IEnumerator spawnMeltedIron()
    {
        while (true)
        {
            if (ismelting)
            {
                yield return new WaitForSeconds(0.4f);
                GameObject newParticle = GameObject.Instantiate(meltedironpartice, meltedironspawn.position, Quaternion.identity);
                newParticle.transform.SetParent(transform.Find("liquid"));
            }
            else
            {
                break;
            }
        }
    }
    public bool GetIsMelting(){ return ismelting; }
}

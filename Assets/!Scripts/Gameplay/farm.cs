using System.Collections;
using UnityEngine;

public class farm : MonoBehaviour
{
    Grid farmGrid;
    [SerializeField] GameObject gridStartPos;
    public bool[,] isPlanted;
    public bool[,] isWatered;
    public bool[,] isGrowing;
    GameObject[,] squares;
    GameObject[,] plants;
    public int[,] currentPhase;

    void Awake()
    {
        farmGrid = new Grid(4, 4, 2, gridStartPos.transform.position, gridStartPos);
        int width = (int)farmGrid.GetSize().x;
        int height = (int)farmGrid.GetSize().y;

        isPlanted = new bool[width, height];
        isWatered = new bool[width, height];
        isGrowing = new bool[width, height];
        squares = new GameObject[width, height];
        plants = new GameObject[width, height];
        currentPhase = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                squares[x, y] = new GameObject();
                squares[x, y].transform.position = farmGrid.GetValueText(x, y).transform.position;
                farmGrid.GetValueText(x, y).gameObject.tag = "farmGridCell";
                squares[x, y].transform.parent = farmGrid.GetValueText(x, y).transform;
                var sr = squares[x, y].AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("Farm/Tarla");
                sr.sortingOrder = 1;
                sr.color = new Color32(0, 150, 45, 255);

                isPlanted[x, y] = false;
                isWatered[x, y] = false;
                isGrowing[x, y] = false;
                currentPhase[x, y] = 0;
            }
        }
    }

    public Grid GetFarmGrid() { return farmGrid; }
    public GameObject GetFarmSquares(int x, int y) { return squares[x, y]; }

    public IEnumerator GrowPlant(int x, int y, int time, SeedScriptableObject seed)
    {
        if (isGrowing[x, y] || !isWatered[x, y] || !isPlanted[x, y]) yield break;

        isGrowing[x, y] = true;

        plants[x, y].GetComponent<SpriteRenderer>().color = new Color32(0, 150, 45, 255);
        yield return new WaitForSeconds(time);
        plants[x, y].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

        currentPhase[x, y]++;

        if (currentPhase[x, y] == 2)
        {
            plants[x, y].GetComponent<SpriteRenderer>().sprite = seed.GetPhase2();
        }
        else if (currentPhase[x, y] == 3)
        {
            plants[x, y].GetComponent<SpriteRenderer>().sprite = seed.GetPhase3();
            isPlanted[x, y] = false; 
        }

        isWatered[x, y] = false;
        isGrowing[x, y] = false;

       

        Debug.Log($"Phase at {x},{y} = {currentPhase[x, y]}");
    }

    public void InitializePlant(int x, int y, SeedScriptableObject seed)
    {
        if (plants[x, y] == null)
        {
            plants[x, y] = new GameObject();
            var sr = plants[x, y].AddComponent<SpriteRenderer>();
            sr.sortingOrder = 2;
            plants[x, y].layer = 2;
            plants[x, y].transform.position = squares[x, y].transform.position;
        }

        plants[x, y].GetComponent<SpriteRenderer>().sprite = seed.GetPhase1();
        currentPhase[x, y] = 1;
    }
}
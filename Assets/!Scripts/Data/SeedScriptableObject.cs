using UnityEngine;
[CreateAssetMenu(fileName = "ScriptableObject", menuName = "seed")]
public class SeedScriptableObject : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] Sprite image;
    [SerializeField] Sprite phase1;
    [SerializeField] Sprite phase2;
    [SerializeField] Sprite phase3;
    int currentphase = 1;

    public Sprite GetImage() { return image; }
    public Sprite GetPhase1() { return phase1; }
    public Sprite GetPhase2() { return phase2; }
    public Sprite GetPhase3() { return phase3; }
    public int GetCurrentPhase() { return currentphase; }
    public void SetCurrentPhase(int a) { currentphase = a; }
    public void IncrementCurrentPhase() { currentphase++; }
    
}

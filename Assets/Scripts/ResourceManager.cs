using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {
    public float stone;
    public float maxStone;
    public float wood;
    public float maxWood;
    public float gold;
    public float maxGold;
    public float food;
    public float maxFood;
    public float population;
    public float maxPopulation;

    public Text stoneDisplay;
    public Text foodDisplay;
    public Text goldDisplay;
    public Text woodDisplay;
    public Text populationDisplay;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        stoneDisplay.text = "" + stone + "/" + maxStone;
        foodDisplay.text = "" + food + "/" + maxFood;
        goldDisplay.text = "" + gold + "/" + maxGold;
        woodDisplay.text = "" + wood + "/" + maxWood;
        populationDisplay.text = "" + population + "/" + maxPopulation;
    }
}

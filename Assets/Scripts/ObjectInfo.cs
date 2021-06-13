using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour {

    public CanvasGroup UserPanel;
    public bool isSelected = false;
    public string objectName;
    public Text nameDisplay;
    public Slider HB;
    public int health;
    public int maxHealth;

    public Slider MB;
    public int magicka;
    public int maxMagicka;

    public Slider SB;
    public int stamina;
    public int maxStamina;

    public int physicalAttack;
    public int physicalDefence;
    public int magicalAttack;
    public int magicalDefence;
    public int rangedAttack;
    public int rangedDefence;
    public Text physicalAttackDisplay;
    public Text physicalDefenceDisplay;
    public Text magicalAttackDisplay;
    public Text magicalDefenceDisplay;
    public Text rangedAttackDisplay;
    public Text rangedDefenceDisplay;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if ( maxStamina <= 0 ) {
            SB.gameObject.SetActive( false );
        }

        if ( maxMagicka <= 0 ) {
            MB.gameObject.SetActive( false );
        }

        if ( health <= 0 ) {
            Destroy( gameObject );
        }

        nameDisplay.text = objectName;

        HB.maxValue = maxHealth;
        HB.value = health;

        MB.maxValue = maxMagicka;
        MB.value = magicka;

        SB.maxValue = maxStamina;
        SB.value = stamina;

        physicalAttackDisplay.text = "PATK: " + physicalAttack;
        physicalDefenceDisplay.text = "PDEF: " + physicalDefence;
        magicalAttackDisplay.text = "MATK: " + magicalAttack;
        magicalDefenceDisplay.text = "MDEF: " + magicalDefence;
        rangedAttackDisplay.text = "RATK: " + rangedAttack;
        rangedDefenceDisplay.text = "RDEF: " + rangedDefence;

        if ( isSelected ) {
            UserPanel.alpha = 1;
            UserPanel.blocksRaycasts = true;
            UserPanel.interactable = true;
        } else {
            UserPanel.alpha = 0;
            UserPanel.blocksRaycasts = false;
            UserPanel.interactable = false;
        }
    }
}

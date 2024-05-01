using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject characterPanel, tilePanel, cityPanel;

    public TMP_Text characterName,
        characterMove,
        characterAtk,
        characterHp,
        characterDescription,
        tileName,
        tileMovementCost,
        tileDescription,
        cityName,
        cityHp,
        cityGold;

    public Image characterImage, tileImage, cityImage;

    private HumanPlayer _humanPlayer;

    private AIPlayer _aiPlayer;

    private Character _characterPiece;

    private Tile _tile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateDisplay()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

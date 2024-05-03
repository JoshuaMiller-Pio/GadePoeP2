using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{
    public GameObject characterInfoPanel, playerInfoPanel, characterActionPanel, tileInfoPanel, cityInfoPanel, cityActionPanel, pauseMenuCanvas, gameOverCanvas;

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
        cityGold,
        gameOverText,
        currentPlayer,
        currentPlayerAP;

    public Image characterImage, tileImage, cityImage;

    private HumanPlayer _humanPlayer;

    private AIPlayer _aiPlayer;

    private Character _characterPiece;

    private CharacterScriptable _characterScript;

    private TileScriptable _tile;

   
    // Start is called before the first frame update
    void Start()
    {
        Tile.tileUI += TileInfoUIUpdater;
        Character.characterInfoUI += CharacterInfoUIUpdater;
        Character.characterActionUI += UpdateCharacterActionDisplay;
        CityManager.cityActionUI += UpdateCityActionDisplay;
        CityManager.cityInfoUI += UpdateCityInfoDisplay;
        CityManager.gameOver += DisplayGameOver;
    }

    private void TileInfoUIUpdater(TileScriptable tile)
    {
       characterInfoPanel.SetActive(false);
        cityInfoPanel.SetActive(false);
        tileInfoPanel.SetActive(true);
        _tile = tile;
        tileName.text = _tile.name;
        tileMovementCost.text = "Move cost = " + _tile.APneeded;
        tileDescription.text = _tile.tileDescription;
    }

    public void CharacterInfoUIUpdater(Character character)
    {
        _characterPiece = character;
        
      //  _characterScript = character.GetComponent<CharacterScriptable>();
        cityInfoPanel.SetActive(false);
        tileInfoPanel.SetActive(false);
        characterInfoPanel.SetActive(true);
        
        characterName.text = _characterPiece.name;
        characterAtk.text = "Attack: " + _characterPiece.characterScript.damage;
        characterHp.text = "HP: " + _characterPiece.currentHealth + "/" + _characterPiece.characterScript.maxHealth;
        characterMove.text = "Move: " + _characterPiece.characterScript.moveableTiles;
    }
    public void UpdateCharacterActionDisplay(Character character)
    {
        cityActionPanel.SetActive(false);
        characterActionPanel.SetActive(true);
        
    }

    public void UpdateCityActionDisplay(CityManager city)
    {
        characterActionPanel.SetActive(false);
        cityActionPanel.SetActive(true);
    }

    public void UpdateCityInfoDisplay(CityManager city)
    {
        tileInfoPanel.SetActive(false);
        characterInfoPanel.SetActive(false);
        cityInfoPanel.SetActive(true);
        cityGold.text = "Gold: " + city.TGold.ToString();
        cityHp.text = "HP: " + city.CityHealth.ToString();
        
    }

    public void UpdatePlayerInfoPanel()
    {
        switch (TurnManager.TurnPlayer)
        {
            case TurnManager.TurnOrder.Player1:
                currentPlayer.text = "Player 1";
                break;
                case TurnManager.TurnOrder.Player2:
                currentPlayer.text = "Player 2";
                    break;
                default:
                break;
        }
        
    }
    public void DisplayGameOver(String tag)
    {
        gameOverCanvas.SetActive(true);
        if (tag != "Human")
        {
            gameOverText.text = "Player 1 Wins! Congratulations!";
        }
        else
        {
            gameOverText.text = "Player 2 Wins! Congratulations!";
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            pauseMenuCanvas.SetActive(true);
        }
        UpdatePlayerInfoPanel();
    }
}

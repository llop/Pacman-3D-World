using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Text.RegularExpressions;

public class HiScoreMenu : MonoBehaviour {

	
  public InputField inputField;
  public Text playerScoreText;


  public void Start() {
    playerScoreText.text = GameManager.Instance.pacmanData.score.ToString();
  }


  public void Update() {
    // bullet-proof select input
    if (inputField.enabled) {
      EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
      inputField.OnPointerClick(new PointerEventData(EventSystem.current));

      inputField.ActivateInputField();
      inputField.Select();
    }
  }


  public void correctInputText() {
    if (inputField.enabled) inputField.text = Regex.Replace(inputField.text, @"[^a-zA-Z]", "");
  }


  public void advance() {
    if (inputField.enabled) {
      inputField.enabled = false;

      GameManager gameManager = GameManager.Instance;
      HiScoreManager.Instance.setHiScore(inputField.text.ToLower(), gameManager.pacmanData.score);

      if (gameManager.gameOver) gameManager.transitionToScene(Tags.GameOverScene);
      else gameManager.transitionToScene(Tags.MenuScene);
    }
  }



}

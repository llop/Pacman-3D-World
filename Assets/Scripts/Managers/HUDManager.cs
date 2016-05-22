using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {



  protected GameManager gameManager;
  protected Text livesText;
  protected Text scoreText;
  protected Text pelletsText;



  public void Awake() {
    //DontDestroyOnLoad(transform.gameObject);
  }


  public void Start() {
    gameManager = GameManager.Instance;
    livesText = transform.Find(Tags.Lives).gameObject.GetComponent<Text>();
    scoreText = transform.Find(Tags.Score).gameObject.GetComponent<Text>();
    pelletsText = transform.Find(Tags.Pellets).gameObject.GetComponent<Text>();
  }


  void Update() {
    livesText.text = "Lives " + gameManager.pacmanData().lives();
    scoreText.text = "Score " + gameManager.pacmanData().score();
    pelletsText.text = "Pellets " + 0;//gameManager.pacmanData().pellets();
	}



}

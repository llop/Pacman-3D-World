using UnityEngine;
using System.Collections;



public class GameCamera : MonoBehaviour {


	
  protected GameObject pacman;
  protected Canvas canvas;



  public void Start() {
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);

    canvas = Instantiate(Resources.Load("Prefabs/GameCanvas")) as Canvas;
    if (canvas != null) canvas.worldCamera = Camera.main;
  }



}

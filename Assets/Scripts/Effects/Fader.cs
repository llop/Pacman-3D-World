using UnityEngine;
using UnityEngine.Events;
using System.Collections;



public class Fader : MonoBehaviour {



  public float damp = 0.5f;
  public float alpha = 0.0f;
  public Color color = Color.black;
  public bool fadeOut = true;
  public bool start = false;



  //---------------------------------------------------------------------------------
  // set these functions to whatever you want to do between transitions
  //---------------------------------------------------------------------------------

  public UnityAction onFadeOut;
  public UnityAction onFadeIn;  



  public void Awake() {
    DontDestroyOnLoad(transform.gameObject);
  }


  void OnGUI() {
    if (start) {
      // draw colored texture with current alpha
      GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
      Texture2D tex = new Texture2D(1, 1);
      tex.SetPixel(0, 0, color);
      tex.Apply();
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

      // recalc alpha and do stuff
      if (fadeOut) {
        alpha = Mathf.Lerp(alpha, 1.1f, damp * Time.deltaTime);
        if (alpha >= 1f) {
          onFadeOut();
          fadeOut = false;
        }
      } else {
        alpha = Mathf.Lerp(alpha, -.1f, damp * Time.deltaTime);
        if (alpha <= 0f) {
          onFadeIn();
          Destroy(transform.gameObject);
        }
      }
    }
  }



}

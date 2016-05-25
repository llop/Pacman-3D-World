using UnityEngine;
using System.Collections;




public class MenuAnimator : MonoBehaviour {


  private Animator anim;


  public bool open {
    get { return anim.GetBool("IsOpen"); }
    set { anim.SetBool("IsOpen", value); }
  }


  public void Awake() {
    anim = GetComponent<Animator>();
  }



}

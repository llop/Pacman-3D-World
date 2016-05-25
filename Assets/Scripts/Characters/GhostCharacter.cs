using UnityEngine;
using System.Collections;



public class GhostCharacter : Character {



  private Renderer bodyRenderer;
  private Material chaseMaterial;
  private Material frightenedMaterial;
  private GhostAIState lastState;



  public void Start() {
    GameObject ghostBody = transform.Find(Tags.GhostBody).gameObject;
    bodyRenderer = ghostBody.GetComponent<Renderer>();
    chaseMaterial = bodyRenderer.materials[0];
    frightenedMaterial = bodyRenderer.materials[1];
    lastState = GhostAIState.Chase;
  }



  protected override void setAnimationState() {
    GhostWalker ghost = walker as GhostWalker;
    if (ghost.chase()) {
      if (lastState == GhostAIState.Frightened || lastState == GhostAIState.Dead) {
        bodyRenderer.enabled = true;
        bodyRenderer.material = chaseMaterial;
      }
      lastState = GhostAIState.Chase;
    }
    if (ghost.scatter()) {
      if (lastState == GhostAIState.Frightened || lastState == GhostAIState.Dead) {
        bodyRenderer.enabled = true;
        bodyRenderer.material = chaseMaterial;
      }
      lastState = GhostAIState.Scatter;
    }
    if (ghost.frightened()) {
      if (lastState != GhostAIState.Frightened) {
        bodyRenderer.enabled = true;
        bodyRenderer.material = frightenedMaterial;
      }
      lastState = GhostAIState.Frightened;
    }
    if (ghost.dead()) {
      if (lastState != GhostAIState.Dead) {
        bodyRenderer.enabled = false;
      }
      lastState = GhostAIState.Dead;
    }
  }



}

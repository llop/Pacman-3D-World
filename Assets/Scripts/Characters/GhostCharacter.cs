using UnityEngine;
using System.Collections;



public class GhostCharacter : Character {



  private Renderer bodyRenderer;
  private Material chaseMaterial;
  private Material frightenedMaterial;
  private int materialIndex;
  private GhostAI ai;
  private GhostAIState lastState;



  public void Start() {
    GameObject ghostBody = transform.Find(Tags.GhostBody).gameObject;
    bodyRenderer = ghostBody.GetComponent<Renderer>();
    chaseMaterial = bodyRenderer.materials[0];
    frightenedMaterial = bodyRenderer.materials[1];
    materialIndex = 0;
    ai = GetComponent<GhostAI>();
    lastState = GhostAIState.Chase;
  }


  private void setMaterial(int index) {
    materialIndex = index;
    bodyRenderer.enabled = true;
    bodyRenderer.material = materialIndex == 0 ? chaseMaterial : frightenedMaterial;
  }

  protected override void setAnimationState() {
    GhostWalker ghost = walker as GhostWalker;
    if (ghost.chase()) {
      if (lastState == GhostAIState.Frightened || lastState == GhostAIState.Dead) setMaterial(0);
      lastState = GhostAIState.Chase;
    }
    if (ghost.scatter()) {
      if (lastState == GhostAIState.Frightened || lastState == GhostAIState.Dead) setMaterial(0);
      lastState = GhostAIState.Scatter;
    }
    if (ghost.frightened()) {
      if (lastState != GhostAIState.Frightened) setMaterial(1);
      else {
        // 5 flashes
        double t = ai.remainingFrightenedTime;
        if (t <= 1.5) setMaterial((int)(t / 0.15) % 2 == 0 ? 1 : 0);
      }
      lastState = GhostAIState.Frightened;
    }
    if (ghost.dead()) {
      if (lastState != GhostAIState.Dead) bodyRenderer.enabled = false;
      lastState = GhostAIState.Dead;
    }
  }



}

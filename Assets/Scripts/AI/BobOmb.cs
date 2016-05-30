using UnityEngine;
using System.Collections;



public class BobOmb : MonoBehaviour {


  public WaypointNode currentNode;
  public WaypointNode nextNode;

  public Direction currentDirection;

  private bool _active = false;

  public void Update() {
    if (!_active) return;

    if (Input.GetKeyDown(KeyCode.V))
      Invoke("explode", 1f); 
  }

  public void OnCollisionEnter(Collision collision) {
    if (collision.transform.gameObject.tag == Tags.Ghost) {
      GhostAI ghostAI = collision.transform.gameObject.GetComponent<GhostAI>();
      if (ghostAI.state != GhostAIState.Dead) explode();
    }
  }

  private void explode() {
    ObjectPool objectPool = GameManager.Instance.levelManager.objectPool;
    
    // instance explosion walker prefabs
    GameObject walkerGO1 = objectPool.instantiate(Tags.ExplosionWalker);
    walkerGO1.transform.position = transform.position;
    walkerGO1.transform.rotation = transform.rotation;
    ExplosionWalker walker1 = walkerGO1.GetComponent<ExplosionWalker>();
    walker1.init(currentNode, nextNode, currentDirection);
    walker1.Start();

    if (currentNode != nextNode) {
      GameObject walkerGO2 = objectPool.instantiate(Tags.ExplosionWalker);
      walkerGO2.transform.position = transform.position;
      walkerGO2.transform.rotation = transform.rotation;
      ExplosionWalker walker2 = walkerGO2.GetComponent<ExplosionWalker>();
      walker2.init(nextNode, currentNode, currentDirection.getOpposite());
      walker2.Start();
    }

    // instance explosion effect
    GameObject exploEffect = objectPool.instantiate(Tags.BombBoom);
    exploEffect.transform.position = transform.position;
    exploEffect.transform.rotation = transform.rotation;

    // wipe the gameObject from the pool
    gameObject.SetActive(false);
  }


  public void init(WaypointNode cur, WaypointNode nex, Direction dir) {
    currentNode = cur;
    nextNode = nex;
    currentDirection = dir;

    _active = true;
  }


}

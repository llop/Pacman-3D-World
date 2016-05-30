using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class ObjectPool : MonoBehaviour {


  [System.Serializable]
  public class StringGameObjectPair {
    public string name;
    public GameObject obj;
  }

  public int numEntries = 10;
  public List<StringGameObjectPair> prefabs;
  public Dictionary<string, GameObject> prefabsMap = new Dictionary<string, GameObject>();

  private Dictionary<string, List<GameObject>> map = new Dictionary<string, List<GameObject>>();


  public void Start() {
    foreach (StringGameObjectPair entry in prefabs) {
      entry.obj.SetActive(false);
      prefabsMap[entry.name] = entry.obj;
      map[entry.name] = new List<GameObject>();
      for (int i=0; i<numEntries; ++i) {
        GameObject go = Instantiate(entry.obj);
        //go.transform.parent = gameObject.transform;
        map[entry.name].Add(go);
      }

    }
  }


  private GameObject nextInactive(string name) {
    // find first inactive
    foreach (GameObject obj in map[name]) if (!obj.activeInHierarchy) return obj;
    
    // if they're all busy, create a new one
    GameObject newObj = Instantiate(prefabsMap[name]);
    map[name].Add(newObj);
    return newObj;
  }


  public GameObject instantiate(string name) {
    if (map[name] != null) {
 
      GameObject go = nextInactive(name);
      go.SetActive(true);
      return go;

    }
    return null;
  }

  //public void destroy(string name, GameObject go) {
  //  if (map[name] != null) {
  //    
  //    if (map[name].active.Contains(go)) {
  //      Debug.Log("destroy hit "+name);
  //      map[name].active.Remove(go);
  //      go.SetActive(false);
  //      map[name].inactive.Add(go);
  //    } else Debug.Log("destroy miss "+name);
  //  }
  //}


}

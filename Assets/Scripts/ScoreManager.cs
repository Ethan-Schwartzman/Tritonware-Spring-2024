using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public GameObject textObject;
    // Start is called before the first frame update
    void Start() {
        textObject = transform.GetChild(0).gameObject;
        textObject.GetComponent<TextMesh>().text = "Fish";
    }

    // Update is called once per frame
    void Update() {

    }
}

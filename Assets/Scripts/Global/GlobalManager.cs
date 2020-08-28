using UnityEngine;
using System.Collections;
using GameDataDictionary;

public class GlobalManager : MonoBehaviour {

    private void Start() {

        //Read files from streaming assets and populate DataMart databases.
        StartCoroutine(DataMart.LoadAllDatabasesFromFile());
    }

}

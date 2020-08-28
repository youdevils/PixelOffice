using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoSortingLayer : MonoBehaviour
{
    private List<SpriteRenderer> spriteRenderers;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = new List<SpriteRenderer>();
        GameObject[] sortobjects = GameObject.FindGameObjectsWithTag("AutoSort");
        if(sortobjects.Length > 0) {
            foreach (GameObject gameobject in sortobjects) {
                if (gameobject.GetComponent<SpriteRenderer>() != null)
                    spriteRenderers.Add(gameobject.GetComponent<SpriteRenderer>());
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (spriteRenderers.Count > 0) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
                
            }
        }
    }
}

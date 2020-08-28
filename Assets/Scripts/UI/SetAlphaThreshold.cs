using UnityEngine;
using UnityEngine.UI;

public class SetAlphaThreshold : MonoBehaviour {
    public float AlphaThreshold = 0.1f;

    void Start() {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}

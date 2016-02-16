using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {
    public GameObject StartMonitor;
    public GameObject GameMonitor;
    public GameObject FinalMonitor;
    public Canvas CanvasMenu;

    private byte Monitor = 0;

    void Start() {
		Vector3 sm = GameObject.Find ("StartMonitor").transform.position;
		transform.position = new Vector3 (sm.x,sm.y,-10);
        StartMonitor.SetActive(true);
        GameMonitor.SetActive(false);
        FinalMonitor.SetActive(false);
        CanvasMenu.enabled = false;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Monitor==0){
            Monitor = 1;
            StartMonitor.SetActive(false);
            GameMonitor.SetActive(true);
        }
        if (Monitor == 1){
            CanvasMenu.enabled = true;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), 0.05f);
        }
        if (Monitor == 2){
            CanvasMenu.enabled = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(22, 0, -10), 0.05f);
        }
    }

    public void GameOverInCamera() {
        Monitor = 2;
        GameMonitor.SetActive(false);
        FinalMonitor.SetActive(true);
    }
}

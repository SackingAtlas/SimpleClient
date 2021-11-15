using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XOButtonScript : MonoBehaviour
{
    private Text cellMarking;
    private Button button;
    public GameObject gameManager;

    private void Start()
    {
        cellMarking = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        cellMarking.text = "";
    }

    public void PlayInCell()
    {
        //cellMarking.text = gameManager.GetComponent<GameSystemManager>().currentPlayerMarker; ;
        //button.interactable = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Canvas UI;
    private UIManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = UI.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

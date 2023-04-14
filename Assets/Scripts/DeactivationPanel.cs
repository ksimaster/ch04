using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivationPanel : MonoBehaviour
{
    public GameObject panelGameOver;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) || panelGameOver.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivationPanel : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.SetActive(false);
        }
    }
}

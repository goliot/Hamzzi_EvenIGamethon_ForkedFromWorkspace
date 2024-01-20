using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public void LockCard()
    {
        this.gameObject.SetActive(false);
    }
}

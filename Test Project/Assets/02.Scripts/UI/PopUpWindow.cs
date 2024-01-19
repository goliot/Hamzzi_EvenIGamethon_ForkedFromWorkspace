using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpWindow : MonoBehaviour
{
    // ÆË¾÷ Ã¢ ²ô´Â ÇÔ¼ö
    public void OnClose()
    {
        PopUpManager.Inst.ClosePopUp(this);
        Destroy(gameObject);
    }
}

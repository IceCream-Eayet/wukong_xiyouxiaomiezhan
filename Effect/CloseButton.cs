using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    private Button closeButton;

    void Start()
    {
        closeButton = GetComponent<Button>();
        closeButton.onClick.AddListener(OnClickCloseButton);
    }


    private void OnClickCloseButton()
    {
        UIManager.Instance.PopPanel();
    }
}

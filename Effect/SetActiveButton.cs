using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveButton : MonoBehaviour
{
    private Button setActive;

    // Start is called before the first frame update
    void Start()
    {
        setActive = GetComponent<Button>();
        setActive.onClick.AddListener(SetActiveButtonOnClick);
    }

    private void SetActiveButtonOnClick()
    {
        gameObject.SetActive(false);
    }
}

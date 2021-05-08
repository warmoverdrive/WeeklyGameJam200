using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    Image background;
    [SerializeField] Text title;
    [SerializeField] Text text;
    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        SetVisible(false);
    }

    private void Update()
    {
        if (active)
            transform.position = Input.mousePosition;
    }

    public void ActivateTooltip(string name, string tooltip)
    {
        title.text = name;
        text.text = tooltip;
        SetVisible(true);
    }

    public void DeactivateTooltip() => SetVisible(false);

    private void SetVisible(bool visible)
    {
        active = visible;
        background.enabled = visible;
        title.enabled = visible;
        text.enabled = visible;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerCustomSetter : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCustom();
        GameManager.instance.onRangerColorChanged += UpdateCustom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameManager.instance.onRangerColorChanged -= UpdateCustom;
    }
    private void OnDisable()
    {
        GameManager.instance.onRangerColorChanged -= UpdateCustom;
    }

    void UpdateCustom()
    {
        meshRenderer.materials[0].color = GameManager.instance.GetColorFromList(GameManager.instance.playerData.rangerCustom.skinIndex);
        meshRenderer.materials[1].color = GameManager.instance.GetColorFromList(GameManager.instance.playerData.rangerCustom.bodyIndex);
    }
}

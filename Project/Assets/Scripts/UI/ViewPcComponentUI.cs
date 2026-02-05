using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewPcComponentUI : MonoBehaviour
{
    public TMP_Text T_Name, T_Description, T_Price;
    public Image I_Sprite;
    public Button B_Buy;
    public SpecUI OriginalSpecUI;
    public List<SpecUI> SpecsUI = new();

    public void Create(PcComponentUI pcui)
    {
        T_Name.text = pcui.T_Name.text;
        T_Description.text = pcui.T_Description.text;
        T_Price.text = pcui.T_Price.text;
        I_Sprite.sprite = pcui.I_Sprite.sprite;

        B_Buy.onClick.RemoveAllListeners();
        B_Buy.onClick = pcui.B_Buy.onClick;
        B_Buy.onClick.AddListener(delegate {
            AudioManager.Play("buy"); 
            gameObject.SetActive(false);
        });

        foreach (var spec in SpecsUI)
        {
            Destroy(spec.gameObject);
        }
        SpecsUI.Clear();

        foreach(var spec in pcui.thisComponent.Specs)
        {
            var newSpec = Instantiate(OriginalSpecUI, OriginalSpecUI.transform.parent);
            newSpec.Create(spec);
            newSpec.gameObject.SetActive(true);
            SpecsUI.Add(newSpec);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SoldOrderUI : MonoBehaviour
{
    public TMP_Text T_Title, T_Price, T_OriginalItem;

    public void Create(List<PhisicalPcComponent> components, int ind)
    {
        T_Title.text = "Sale #" + ind;
        T_Price.text = "Total: $" + (components.Sum(c => c.thisComponent.Price) + components.Sum(c => c.thisComponent.Price) / 10);

        foreach (var pc in components)
        {
            var newItem = Instantiate(T_OriginalItem, T_OriginalItem.transform.parent);
            newItem.gameObject.SetActive(true);
            newItem.text = $"<align=left>{pc.thisComponent.Name}<line-height=0>\r\n<align=right>${pc.thisComponent.Price}<line-height=1em>";
        }
    }
}

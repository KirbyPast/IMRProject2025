using UnityEngine;

[ExecuteInEditMode]
public class RemoveAllComponentsFromGameObject : MonoBehaviour
{
    //This is an editor script that is used to remove all components of a gameobject.
    //To use: Add this script to a gameobject
    //To clear multiple objects from components, mark multiple objects
    //and add the script to them

    void OnEnable()
    {
        for (int i = 0; i < 6; i++)
        {
            foreach (var comp in gameObject.GetComponents<Component>())
            {
                //Don't remove the Transform component
                if (!(comp is Transform))
                {
                    //Don't remove this script until the loop has finished
                    if (!(comp is RemoveAllComponentsFromGameObject))
                    {
                        DestroyImmediate(comp);
                    }
                }
            }
        }

        if (gameObject.GetComponents<Component>().Length > 0)
            Debug.LogWarning("Tried clearing object but could not finish: "
                + gameObject.transform.name);
        else
            Debug.LogWarning("Cleared components from object successfully: "
                + gameObject.transform.name);

        DestroyImmediate(gameObject.GetComponent<RemoveAllComponentsFromGameObject>());
    }
}
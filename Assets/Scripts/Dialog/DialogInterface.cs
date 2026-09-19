using UnityEngine;

public class DialogInterface : MonoBehaviour
{
    public void TriggerDialog(int id)
    {
        DialogController.instance.DisplayDialogLine(id);
    }
}

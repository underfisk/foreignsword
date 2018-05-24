using HelperPackage;
using HttpPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteCharacterController : MonoBehaviour
{
    /// <summary>
    /// ID of the character being deleted
    /// </summary>
    public int c_delete_id = -1;
    [SerializeField] public Button ConfirmButton, CancelButton;

    protected virtual void Start()
    {
        ConfirmButton.onClick.AddListener(() => ProceedDelete());
        CancelButton.onClick.AddListener(() => CancelDelete());
    }

    private void ProceedDelete()
    {
        if (c_delete_id != -1)
        {
            //for test now
            ILog.toUnity("Deleting.. char id = " + c_delete_id);
            HttpForm formData = new HttpForm();
            formData.AddField("char_id", c_delete_id);

            HttpRequest request = new HttpRequest();
            request.Post(HttpLinks.character_delete, formData);

            if (request.isDone)
            {
                if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
                {
                    ILog.toUnity("Deleted.", LType.Success);
                    //After we remove the gameobject with this id from the CharactersGrid
                    Destroy(GameObject.Find($"Character_ID_{c_delete_id}"));
                    var cs = GameObject.Find("GUI").gameObject.GetComponent<CharacterSceneController>();
                    if (cs._Characters.Count >= 1)
                    {
                        cs.UpdateCharacter(cs._Characters[0].ID, cs._Characters[0].ClassID);
                    }
                    gameObject.SetActive(false);
                }
                else
                {
                    ILog.toUnity("Failed deleting the character", LType.Error);
                    //TODO: Show an error message
                }
            }

            //request httprequest to delete it

            //success? we remove from grid and reset the char selected

            //success? reset the c_delete_id to -1

            //error? we just simply show a message saying it
        }
    }

    private void CancelDelete()
    {
        //reset char_id
        c_delete_id = -1;

        //hide popup
        gameObject.SetActive(false);
    }
}

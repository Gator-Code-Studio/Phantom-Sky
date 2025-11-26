using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSceneController : MonoBehaviour
{
    [SerializeField] private GameObject leftRightBoard;
    [SerializeField] private GameObject spaceBoard;
    [SerializeField] private GameObject shiftToDashBoard;





    private void Start()
    {





    }




    private void Update()
    {
        if(Keyboard.current.aKey.wasPressedThisFrame|| Keyboard.current.dKey.wasPressedThisFrame)
        {

            StartCoroutine(WaitfroLeftRightInput());
          


        }



       





    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        spaceBoard.SetActive(true);



    }





    private void OnTriggerStay2D(Collider2D collision)
    {
            Debug.Log("We are in OnTiggerStay ");
        if (Keyboard.current.spaceKey.isPressed)
        {
            spaceBoard.SetActive(false);
            shiftToDashBoard.SetActive(true);
            Debug.Log("Space Key is Pressed ");
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {

            shiftToDashBoard.SetActive(false);
        }


    }




    IEnumerator WaitfroLeftRightInput()
    {

        yield return new WaitForSeconds(3f);

        leftRightBoard.SetActive(false);

    }





}

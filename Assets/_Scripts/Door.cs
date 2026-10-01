using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour
{
    //This is a test class and a example of how to use interaction with anything in the game. 




    private HoldInteraction interaction;        //Interaction type

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float smoothSpeed = 2f;
    public float openDuration = 3f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool opening = false;
    private bool closing = false;
    private float openTimer = 0f;

    private void Start()
    {
        interaction = GetComponentInChildren<HoldInteraction>();                     //Be sure to get reference for interaction

        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        interaction.OnComplete += OpenDoor;                                          //Marry onComplete to the function you want
    }

    private void Update()
    {
        // Opening
        if (opening)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                Time.deltaTime * smoothSpeed
            );

            if (Quaternion.Angle(transform.rotation, openRotation) < 0.1f)
            {
                transform.rotation = openRotation;
                opening = false;

                // Start timer once fully open
                openTimer = 0f;
            }
        }

        // Door is fully open
        if (!opening && !closing && transform.rotation == openRotation)
        {
            openTimer += Time.deltaTime;

            if (openTimer >= openDuration)
            {
                closing = true;
            }
        }

        // Closing
        if (closing)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                closedRotation,
                Time.deltaTime * smoothSpeed
            );

            if (Quaternion.Angle(transform.rotation, closedRotation) < 0.1f)
            {
                transform.rotation = closedRotation;
                closing = false;
            }
        }
    }

    private void OpenDoor()                          //Function that will be called by interaction 
    {


        opening = true;
        closing = false;
    }

    private void OnDestroy()                        //Always call this OnDestroy just incase of any issues with objects being destroyed in game
    {
        if (interaction != null)
        {
            interaction.OnComplete -= OpenDoor;         
        }
    }
}

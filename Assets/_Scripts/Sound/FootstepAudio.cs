using FMODUnity;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("FMOD Footstep Event")]
    [SerializeField] private EventReference footstepEvent;


    public void PlayFootstep()
    {
        //Debug.Log("FOOTSTEP EVENT FIRED!");

        RuntimeManager.PlayOneShot(footstepEvent,transform.position);
    }
}

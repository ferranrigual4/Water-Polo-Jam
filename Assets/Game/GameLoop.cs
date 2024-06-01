using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] AudioSource splashAudioSource;
    public GameObject ballInstance;
    public ManScript myCharacter;
    public ManScript otherCharacter;
    private ManScript theActiveCharacter;
    

    // Start is called before the first frame update
    void Start()
    {
        ballInstance = GameObject.Instantiate(ballPrefab, Vector3.zero, Quaternion.identity, myCharacter.handBone);
        ballInstance.transform.localPosition = Vector3.zero;
        ballInstance.transform.localRotation = Quaternion.identity;

        SetActiveCharacter(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (theActiveCharacter != null)
            {
                ballInstance.transform.SetParent(null, true);
                Vector3 passDirection = theActiveCharacter.lastForwardDirection.HasValue ? theActiveCharacter.lastForwardDirection.Value : theActiveCharacter.transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(theActiveCharacter.transform.position, passDirection, out hit, 5, ~LayerMask.GetMask("Characters")))
                {
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    Debug.Log("did not hit");
                }
                ThrowBall(myCharacter, otherCharacter);
                SetActiveCharacter(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 passDirection = theActiveCharacter.lastForwardDirection.HasValue ? theActiveCharacter.lastForwardDirection.Value : theActiveCharacter.transform.forward;
        Gizmos.DrawRay(theActiveCharacter.transform.position, passDirection * 5);
    }

    private void ThrowBall (ManScript passer, ManScript receiver)
    {
        Vector3 origin = passer.handBone.transform.position;
        Vector3 dest = receiver.handBone.transform.position;
        FindAnyObjectByType<BallScript>().SetupThrowing(origin, dest, PlaySplashSound);
    }

    private void PlaySplashSound()
    {
        splashAudioSource.Play();
    }

    private void SetActiveCharacter(bool isMyCharacter)
    {
        if (isMyCharacter)
        {
            theActiveCharacter = myCharacter;
            myCharacter.SetActivePlayer(true);
            otherCharacter.SetActivePlayer(false);
        }
        else
        {
            theActiveCharacter = otherCharacter;
            myCharacter.SetActivePlayer(false);
            otherCharacter.SetActivePlayer(true);
        }


    }


}

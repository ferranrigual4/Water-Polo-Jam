using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManScript : MonoBehaviour
{
    [SerializeField] float speed;
    public Transform handBone;
    Animator animator;
    [SerializeField] bool isActivePlayer;
    public Vector3? lastForwardDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lastForwardDirection = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isActivePlayer)
        {
            return;
        }

        // Get input horizontal and vertical axes (WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float velocity = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal)) * Time.fixedDeltaTime * speed;

        if (velocity > 0)
        {
            Vector3 forward = new Vector3(horizontal, 0, vertical);

            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            lastForwardDirection = forward;

            // Move character based on input
            transform.position += transform.forward * velocity;

            // Compute velocity
            animator.SetFloat("speed", velocity);
        }
        else { lastForwardDirection = null; }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (!other.gameObject.CompareTag("Ball"))
        {
            return;
        }

        Debug.Log("Collision with ball with man: " + this.name);
        other.transform.SetParent(handBone, true);
        other.transform.localPosition = Vector3.zero;
        other.transform.localRotation = Quaternion.identity;
        other.GetComponent<BallScript>().StopPass();

    }

    public void SetActivePlayer(bool active) {
        isActivePlayer = active;
    }
}

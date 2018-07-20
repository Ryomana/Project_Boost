using UnityEngine;
using UnityEngine.SceneManagement;

enum State { Alive, Dying, Transcending }

public class Rocket : MonoBehaviour {

    private Rigidbody rigidBody;
    private AudioSource thrustSound;
    State state = State.Alive;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float thrustSpeed = 100f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thrustSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collision when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friend");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            if (!thrustSound.isPlaying)
            {
                thrustSound.Play();
            }
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        }
        else
        {
            thrustSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control over the rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // auto control over the rotation
    }
}

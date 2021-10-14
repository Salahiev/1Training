using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] Text energyText;
    [SerializeField] int energyTotal = 2000;
    [SerializeField] int energyApply = 20;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip FinishSound;
    [SerializeField] AudioClip BoomSound;
    [SerializeField] ParticleSystem FlyParticle;
    [SerializeField] ParticleSystem FlyParticles;
    [SerializeField] ParticleSystem DeathParticle;
    [SerializeField] ParticleSystem FinishParticle;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Playing,Dead,NextLevel};
    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            if (Input.GetMouseButtonDown(0))
                Debug.Log("Pressed primary button.");

            if (Input.GetMouseButtonDown(1))
                Debug.Log("Pressed secondary button.");

            if (Input.GetMouseButtonDown(2))
                Debug.Log("Pressed middle click.");
            Launch();
            Rotation();
        }       
    }
    private void OnCollisionEnter(Collision collision)          
    {
        if (state != State.Playing)
        {
            return;
        }
            switch (collision.gameObject.tag)
            {
            case "Friendly":
                print("ok");
                if (energyTotal <=0)
                {
                    Lose();
                }
                break;
            case "Finish":
                Finish();
                break;
            case "Battery":
                PlusEnergy(200, collision.gameObject);
                break;
            default:
                Lose();
                break;
            }            
    }
    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(FinishSound);
        FinishParticle.Play();
        Invoke("LoadNextLevel", 2f); // задержка 2 секунды
    }
    void Lose()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(BoomSound);
        DeathParticle.Play();
        Invoke("LoadFirstLevel", 2f);
    }
    void LoadNextLevel() // Finish
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 1;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }
    void LoadFirstLevel() //Load
    {
        
        if (SceneManager.GetActiveScene().buildIndex!=1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);        
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlusEnergy(int EnergyToAdd, GameObject batteryObj)
    {
        batteryObj.GetComponent<BoxCollider>().enabled = false;
        energyTotal += EnergyToAdd;
        energyText.text = energyTotal.ToString();
        Destroy(batteryObj);
    }
    void Launch()
    {
        if (Input.GetKey(KeyCode.Space) && energyTotal > 0)
        {   
            FlyParticle.Play();
            FlyParticles.Play();       
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime); // energyT = energyT-10
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up* flySpeed * Time.deltaTime);// полет вверх
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);             
        }
        else
        {
            audioSource.Pause();
            FlyParticle.Stop();
            FlyParticles.Stop();
        }
    }
    void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationSpeed);  
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }
 }

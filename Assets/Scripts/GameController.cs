using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject toiletPrefab;
    public Slider timeSlider;

    // position that the first cabin will spawn on the scene
    public float originalPosition;


    // distance between the cabins
    private int toiletDistance = 3;
    private int toiletNumber = 6;
    private float timeLeft;
    private float totalTime = 5f;
    // this changes after each cabin is instantiated
    private float actualPosition;
    // keep track if the player have selected any cabin
    private bool playerSelected = false;

    void Start() {
        actualPosition = originalPosition;
        spawnToilets(toiletNumber);
        timeLeft = totalTime;
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeSlider.value = timeLeft;
        }

        if (timeLeft <= 0) {
            if (!playerSelected) {
                SceneManager.LoadScene("TimeOver");
                return;
            }

            // play some kind of animation of the toilet exploding
            // maybe use coroutine to make the game wait for the end of the animation before continue

            bool looser = checkToilets();

            if (looser) {
                SceneManager.LoadScene("GameOver");
                return;
            }

            if (toiletNumber - 1 == 1) {
                SceneManager.LoadScene("EndGame");
                return;
            }

            toiletNumber -= 1;
            deleteToilets();
            actualPosition = originalPosition;
            spawnToilets(toiletNumber);
            playerSelected = false;
            timeLeft = totalTime;
            timeSlider.value = timeLeft;
        }
    }

    public void setPlayerSelected(bool playerSelected) {
        this.playerSelected = playerSelected;
    }

    public bool getPlayerSelected() {
        return this.playerSelected;
    }

    bool checkToilets() {
        foreach (var toilet in GameObject.FindGameObjectsWithTag("toilet")) {
            if (toilet.GetComponent<Toilet>().getHavePoop() && toilet.GetComponent<Toilet>().getPlayerChoice()) {
                return true;
            }
        }

        return false;
    }

    void deleteToilets() {
        foreach (var toilet in GameObject.FindGameObjectsWithTag("toilet")) {
            Destroy(toilet);
        }
    }

    void spawnToilets(int number) {
        int cabinNumber = Random.Range(0, number - 1);
        // Debug.Log(cabinNumber);

        for (int i = 0; i < number; i++) {
            GameObject toilet = Instantiate(toiletPrefab, new Vector3(actualPosition, -2f, 0), Quaternion.identity);
            actualPosition += toiletDistance;

            if (i == cabinNumber) {
                toilet.GetComponent<Toilet>().setHavePoop(true);
            }
        }
    }
}

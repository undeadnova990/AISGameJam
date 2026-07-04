using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerBehavior : MonoBehaviour
{
    public GameObject[] peopleList;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            foreach (GameObject person in peopleList)
            {
                person.SetActive(true);
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

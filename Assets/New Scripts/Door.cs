using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Door : MonoBehaviour
{
    public GameObject handUI;
    public GameObject UIText;
    public GameObject invKey;  // Represents key being collected
    public GameObject fadeFX;
    public string nextSceneName;

    private bool inReach = false;

    void Start()
    {
        handUI.SetActive(false);
        UIText.SetActive(false);
        fadeFX.SetActive(false);
        invKey.SetActive(false); // Only if you're sure this is inactive until collected
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;
            handUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
            handUI.SetActive(false);
            UIText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed while in reach");

            if (invKey.activeInHierarchy)
            {
                Debug.Log("Key is active. Starting ending.");
                handUI.SetActive(false);
                UIText.SetActive(false);
                fadeFX.SetActive(true);
                StartCoroutine(ending());
            }
            else
            {
                Debug.Log("Key not active. Showing text.");
                UIText.SetActive(true);
            }
        }
    }

    IEnumerator ending()
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(nextSceneName);
    }
}
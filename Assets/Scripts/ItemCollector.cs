using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : MonoBehaviour
{
    private int coffee = 0;
    [SerializeField] private Text coffeeText;

    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource finishSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coffee"))
        {
            collect.Play();
            Destroy(collision.gameObject);
            coffee++;
            coffeeText.text = "Coffee: " + coffee;
        }
        if (collision.gameObject.CompareTag("FinalCoffee"))
        {
            finishSound.Play();
            Destroy(collision.gameObject);
            coffee += 10;
            coffeeText.text = "Coffee: " + coffee;
            Invoke("CompleteLevel", 2); 
        }
    }
    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeBook : MonoBehaviour
{
    [Header("UI")]
    public GameObject bookUI;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public Image recipeImage; // NEW

    [Header("Pages")]
    public List<RecipePage> pages = new List<RecipePage>();

    private int currentPage = 0;
    private bool isOpen = false;

    void Start()
    {
        bookUI.SetActive(false);
        UpdatePage();
    }

    void Update()
    {
        Debug.Log("RecipeBook Update running");

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed");
            ToggleBook();
        }
    }

    public void ToggleBook()
    {
        isOpen = !isOpen;
        bookUI.SetActive(isOpen);

        Time.timeScale = isOpen ? 0f : 1f;
    }

    public void NextPage()
    {
        if (pages.Count == 0) return;

        currentPage++;
        if (currentPage >= pages.Count)
            currentPage = 0;

        UpdatePage();
    }

    public void PreviousPage()
    {
        if (pages.Count == 0) return;

        currentPage--;
        if (currentPage < 0)
            currentPage = pages.Count - 1;

        UpdatePage();
    }

    void UpdatePage()
    {
        if (pages.Count == 0) return;

        RecipePage page = pages[currentPage];

        titleText.text = page.title;
        contentText.text = page.content;

        if (page.image != null)
        {
            recipeImage.sprite = page.image;
            recipeImage.enabled = true;
        }
        else
        {
            recipeImage.enabled = false;
        }
    }
}

[System.Serializable]
public class RecipePage
{
    public string title;

    [TextArea(5, 10)]
    public string content;

    public Sprite image; // NEW
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintScene : MonoBehaviour
{

    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        Button bckBtn = btn.GetComponent<Button>();
        bckBtn.onClick.AddListener(switchScene);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void switchScene(){
        SceneManager.LoadScene("JednymTahom");
    }
}

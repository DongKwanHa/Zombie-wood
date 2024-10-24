using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Image AnyKeyDown;//±ô¹ÚÀÏ ÀÌ¹ÌÁö
    public float blinkSpeed = 1f;//±ôºýÀÌ´Â ¼Óµµ

    private bool isBlinking = true;
    void Start()
    {
        Debug.Log("LoadSceneB");
        StartCoroutine(BlinkImage());
    }

    IEnumerator BlinkImage()
    {
        while(isBlinking) 
        {
            for(float alpha =0f;alpha<=1f;alpha+=Time.deltaTime*blinkSpeed)
            {
                SetImageAlpha(alpha);
                yield return null;
            }

            for(float alpha =1f;alpha>=0f;alpha-=Time.deltaTime*blinkSpeed)
            {
                SetImageAlpha(alpha);
                yield return null;
            }
        }
    }

    void SetImageAlpha(float alpha)
    {
        if(AnyKeyDown!=null)
        {
            Color color = AnyKeyDown.color;
            color.a = alpha;
            AnyKeyDown.color = color;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown&&isBlinking)
        {
            isBlinking = false;
            StartCoroutine(LoadSceneWithDelay(1));
            
        }
    }

    IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(0.1f);
        LoadingScene(1);
    }

   public void LoadingScene(int SceneANumber)
    {
        SceneManager.LoadScene(SceneANumber);
  
        Debug.Log("sceneBuildIndex to load: " + SceneANumber);

    }
}

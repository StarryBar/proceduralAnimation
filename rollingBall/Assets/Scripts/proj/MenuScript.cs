using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MenuScript : MonoBehaviour
{
    public GameObject menuPanel;
    public InputField inputField;
    public GameObject ball;
    public GameObject refPointPrefab;
    public GameObject camera;

    public float mass = 0.1f;
    public float drag = 0.05f;
    public float angularDrag = 0.05f;

    private void Start()
    {
        Debug.Log("fuck0");
        Debug.Log("fuck1");
        Debug.Log("inputField0:" + inputField.text);
    }

    public void Host()
    {
        NetworkManager.singleton.StartHost();
        menuPanel.SetActive(false);
        GameObject b = Instantiate(ball);
        b.AddComponent<Rigidbody>();
        b.GetComponent<Rigidbody>().mass = mass;
        b.GetComponent<Rigidbody>().drag = drag;
        b.GetComponent<Rigidbody>().useGravity = true;
        b.GetComponent<Rigidbody>().angularDrag = angularDrag;
        NetworkServer.Spawn(b);

        GameObject refPoint = Instantiate(refPointPrefab, 
            camera.transform.position, camera.transform.rotation);
        NetworkServer.Spawn(refPoint);
    }
    public void Join()
    {
        Debug.Log("inputField1:" + inputField.text);
        if (inputField.text.Length <= 0)
        {
            NetworkManager.singleton.networkAddress = "127.0.0.1";
        } else
        {
            NetworkManager.singleton.networkAddress = inputField.text;
        }
        NetworkManager.singleton.StartClient();
        menuPanel.SetActive(false);
        Debug.Log("fuck2");
    }


}

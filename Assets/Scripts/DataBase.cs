using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{ 
    //variables
    DatabaseReference reference;

    //variables texto
    //public Text t1, t2, t3, t4, t5, t6, t7, t8, t9, t10;
    public InputField nombre, apellido, id, edad, mail;
    private string nom, ape, ced, eda, mai,
        v1, v2, v3, v4, v5, v6, v7, v8, v9, v10;

    //variables abrir
    public GameObject prefab;
    private GameObject obj;

    private GameObject obj1;

    void Start()
    {
        //configurar url de firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gy-medic.firebaseio.com/");

        //obtiene la ubicacion referencia de la raiz de firebase
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //iniciar DB
        IniciarDB();
    }

    //inicializar firebase
    public void IniciarDB()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    //recuperar datos
    public bool RecuperarDatos()
    {
        if (nombre.text.Equals("") || apellido.text.Equals("") ||
            id.text.Equals("") || edad.text.Equals("") ||
            mail.text.Equals(""))
        {
            return false;
        }
        else
        {
            nom = nombre.text;
            ape = apellido.text;
            ced = id.text;
            eda = edad.text;
            mai = mail.text;

            v1 = GameObject.Find("t1_reposo").GetComponent<Text>().text; 
            v2 = GameObject.Find("t2_reposo").GetComponent<Text>().text;
            v3 = GameObject.Find("t3_reposo").GetComponent<Text>().text;

            v4 = GameObject.Find("t1_ajustada").GetComponent<Text>().text;
            v5 = GameObject.Find("t2_ajustada").GetComponent<Text>().text; 
            v6 = GameObject.Find("t3_ajustada").GetComponent<Text>().text; 

            v7 = GameObject.Find("t1_agamma").GetComponent<Text>().text;
            v8 = GameObject.Find("t2_agamma").GetComponent<Text>().text;

            v9 = GameObject.Find("t1_total").GetComponent<Text>().text;
            v10 = GameObject.Find("t1_total").GetComponent<Text>().text;
            return true;
        }
    }

    //accion guardar GY Medic
    public void GYGuardar()
    {
        //recuperar datos
        RecuperarDatos();

        if (RecuperarDatos().Equals(true))
        {
            //daer color a los input field
            nombre.image.color = Color.green;
            apellido.image.color = Color.green;
            id.image.color = Color.green;
            edad.image.color = Color.green;
            mail.image.color = Color.green;


            try
            {
                Guardar(ced, nom, ape, eda, mai, v1, v2, v3, v4, v5, v6, v7, v8, v9, v10);
            }
            catch(Exception e)
            {
                //Debug.Log("Error");
                Debug.LogException(e, this);
            }
        }
        else
        {
            //validar campos vacios
            if (nombre.text.Equals(""))
                nombre.image.color = Color.red;

            if (apellido.text.Equals(""))
                apellido.image.color = Color.red;

            if (id.text.Equals(""))
                id.image.color = Color.red;

            if (edad.text.Equals(""))
                edad.image.color = Color.red;

            if (mail.text.Equals(""))
                mail.image.color = Color.red;
        }

    }


    public void Guardar(string cedula, string nombre, string apellido, string edad, string mail,
        string val1, string val2, string val3, string val4, string val5,
        string val6, string val7, string val8, string val9, string val10)
    {
        Paciente paciente = new Paciente(nombre, apellido, cedula, edad, mail, val1, val2, val3, val4, val5, val6, val7, val8, val9, val10);
        string json = JsonUtility.ToJson(paciente);

        reference.Child("pacientes").Child(cedula).SetRawJsonValueAsync(json);

        //cerrar el cuadro de informacion una vez se guarden los datos 
        obj1 = GameObject.Find("InfoCanvas(Clone)");
        GameObject.Destroy(obj1);

        //abrir cuadro de niveles
        GYAbrirN();
    }

    //accion abrir cuadro de niveles
    public void GYAbrirN()
    {
        obj = Instantiate(prefab);
    }

}


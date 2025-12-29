using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions_Button : MonoBehaviour
{

    //variables abrir
    public GameObject prefab;
    private GameObject obj;
    

    //variables eliminar
    public Text t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16;

    //variables iniciar pausar
    //public Button btn_iniciar;
    public Sprite play, pause;
    public bool estado = true;

    //accion abrir cuadro de informacion
    public void GYAbrir()
    {
        obj = Instantiate(prefab);
    }

    //accion eliminar GY Medic
    public void GYEliminar()
    {
        t1.text = "--";
        t2.text = "--";
        t3.text = "--";
        t4.text = "--";
        t5.text = "--";
        t6.text = "--";
        t7.text = "--";
        t8.text = "--";
        t9.text = "--";
        t10.text = "--";
        t11.text = "--";
        t12.text = "--";
        t13.text = "--";
        t14.text = "--";
        t15.text = "--";
        t16.text = "--";
    }

    //accion play/pause GY Medic
    /*public void GYIniciarPausar()
    {
        if (estado.Equals(true))
        {
            //cambiar a estado pausa
            btn_iniciar.GetComponent<Image>().sprite = play
;
            //inicar pausa
            Time.timeScale = 0;
            //print("Holaaaaaaaaaaaaaaaaa");

            estado = !estado;
        }
        else
        {
            //cambiar  a estado play
            btn_iniciar.GetComponent<Image>().sprite = pause;
            
            //terminar pausa
            Time.timeScale = 1;

            estado = !estado;
        }

    }*/

    //accion salir GY Medic
    public void GYExit()
    {
        Application.Quit();
    }

    
}



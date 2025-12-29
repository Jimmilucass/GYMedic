using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion_Canvas : MonoBehaviour
{
    //variables
    private GameObject obj;

    //accion cerrar cuadro de informacion
    public void CACerrar()
    {
        obj = GameObject.Find("InfoCanvas(Clone)");
        GameObject.Destroy(obj);
    }

    //accion cerrar cuadro de de niveles
    public void CNCerrar()
    {
        obj = GameObject.Find("CargarCanvas(Clone)");
        GameObject.Destroy(obj);
    }
}


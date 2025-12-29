using System;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.Face;

internal class Paciente : MonoBehaviour
{
    //variables
    public string nombre, apellido, cedula, edad, mail;

    public string reposo_cejas, reposo_ojos, reposo_boca;
    public string ajustada_cejas, ajustada_ojos, ajustada_boca;
    public string area_ojos, angulo_boca;
    public string total_ojos, total_boca;

    public Paciente(string nombre, string apellido, string cedula, string edad, string mail,
        string reposo_cejas, string reposo_ojos, string reposo_boca,
        string ajustada_cejas, string ajustada_ojos, string ajustada_boca,
        string area_ojos, string angulo_boca,
        string total_ojos, string total_boca)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.cedula = cedula;
        this.edad = edad;
        this.mail = mail;

        this.reposo_cejas = reposo_cejas;
        this.reposo_ojos = reposo_ojos;
        this.reposo_boca = reposo_boca;
        this.ajustada_cejas = ajustada_cejas;
        this.ajustada_ojos = ajustada_ojos;
        this.ajustada_boca = ajustada_boca;
        this.area_ojos = area_ojos;
        this.angulo_boca = angulo_boca;
        this.total_ojos = total_ojos;
        this.total_boca = total_boca;
    }

}
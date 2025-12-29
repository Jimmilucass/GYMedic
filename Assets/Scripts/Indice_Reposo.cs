using System;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.Face;

internal class Indice_Reposo : MonoBehaviour
{
    //variables reposo
    private double xc1, yc1, zc1, xc2, yc2, zc2, xc3, yc3, zc3, dc1, dc2;
    private double xo1, yo1, zo1, xo2, yo2, zo2, xo3, yo3, zo3, xo4, yo4, zo4, do1, do2;
    private double xb1, yb1, zb1, xb2, yb2, zb2, xb3, yb3, zb3, db1, db2;

    //variables si
    private double SI_cejas, SI_ojos, SI_boca;

    //calcular simetria reposo cejas
    public Double getCReposo(int i, CameraSpacePoint point)
    {
        if (i == 18)
        {
            xc1 = point.X;
            yc1 = point.Y;
            zc1 = point.Z;
        }
        if (i == 222)
        {
            xc2 = point.X;
            yc2 = point.Y;
            zc2 = point.Z;
        }
        if (i == 849)
        {
            xc3 = point.X;
            yc3 = point.Y;
            zc3 = point.Z;
        }

        dc1 = Math.Sqrt(Math.Pow(xc2 - xc1, 2) + Math.Pow(yc2 - yc1, 2) + Math.Pow(zc2 - zc1, 2));
        dc2 = Math.Sqrt(Math.Pow(xc3 - xc1, 2) + Math.Pow(yc3 - yc1, 2) + Math.Pow(zc3 - zc1, 2));

        if (dc1 < dc2)
            SI_cejas = (dc1 / dc2) * 100;
        else
            SI_cejas = (dc2 / dc1) * 100;
        
        return SI_cejas;
    }

    //calcular simetria reposo ojos
    public Double getOReposo(int i, CameraSpacePoint point)
    {
        if (i == 241)
        {
            xo1 = point.X;
            yo1 = point.Y;
            zo1 = point.Z;
        }
        if (i == 1104)
        {
            xo2 = point.X;
            yo2 = point.Y;
            zo2 = point.Z;
        }
        if (i == 731)
        {
            xo3 = point.X;
            yo3 = point.Y;
            zo3 = point.Z;
        }
        if (i == 1090)
        {
            xo4 = point.X;
            yo4 = point.Y;
            zo4 = point.Z;
        }
        do1 = Math.Sqrt(Math.Pow((xo2 - xo1), 2) + Math.Pow((yo2 - yo1), 2) + Math.Pow((zo2 - zo1), 2));
        do2 = Math.Sqrt(Math.Pow((xo4 - xo3), 2) + Math.Pow((yo4 - yo3), 2) + Math.Pow((zo4 - zo3), 2));
        if (do1 < do2)
            SI_ojos = (do1 / do2) * 100;
        else
            SI_ojos = (do2 / do1) * 100;

        return SI_ojos;
    }

    //calcular simetria reposo boca
    public Double getBReposo(int i, CameraSpacePoint point)
    {
        if (i == 18)
        {
            xb1 = point.X;
            yb1 = point.Y;
            zb1 = point.Z;
        }
        if (i == 91)
        {
            xb2 = point.X;
            yb2 = point.Y;
            zb2 = point.Z;
        }
        if (i == 687)
        {
            xb3 = point.X;
            yb3 = point.Y;
            zb3 = point.Z;
        }
        db1 = Math.Sqrt(Math.Pow((xb2 - xb1), 2) + Math.Pow((yb2 - yb1), 2) + Math.Pow((zb2 - zb1), 2));
        db2 = Math.Sqrt(Math.Pow((xb3 - xb1), 2) + Math.Pow((yb3 - yb1), 2) + Math.Pow((zb3 - zb1), 2));
        if (db1 < db2)
            SI_boca = (db1 / db2) * 100;
        else
            SI_boca = (db2 / db1) * 100;

        return SI_boca;
    }

    //calcular gamma cejas
    public double getGCejas(double cejas)
    {
        return 250 * Math.Pow(cejas, 2.5);
    }

    //calcular gamma ojos
    public double getGOjos(double ojos)
    {
        return 500 * Math.Pow(ojos, 2.5);
    }

    //calcular gamma boca
    public double getGBoca(double boca)
    {
        return 30 * Math.Pow(boca, 2.5);
    }
}
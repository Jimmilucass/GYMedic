using System;
using UnityEngine;
using Windows.Kinect;

internal class Simetria_Ajuste: MonoBehaviour
{
    //variables area ojo izquierda
    public double xi1, yi1, zi1, xi2, yi2, zi2, xi3, yi3, zi3, xi4, yi4, zi4, di1, di2;

    //variables area ojo derecho
    public double xd1, yd1, zd1, xd2, yd2, zd2, xd3, yd3, zd3, xd4, yd4, zd4, dd1, dd2;

    //variables angulo UV
    public double x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4, ux, uy, uz, vx, vy, vz, mu, mv;

    //areas ojo izquierdo
    public Double getAreaOIzquierdo(int i, CameraSpacePoint point)
    {
        if (i == 469)
        {
            xi1 = point.X;
            yi1 = point.Y;
            zi1 = point.Z;
        }
        if (i == 210)
        {
            xi2 = point.X;
            yi2 = point.Y;
            zi2 = point.Z;
        }
        if (i == 241)
        {
            xi3 = point.X;
            yi3 = point.Y;
            zi3 = point.Z;
        }
        if (i == 1104)
        {
            xi4 = point.X;
            yi4 = point.Y;
            zi4 = point.Z;
        }
        di1 = Math.Sqrt(Math.Pow((xi2 - xi1), 2) + Math.Pow((yi2 - yi1), 2) + Math.Pow((zi2 - zi1), 2));
        di2 = Math.Sqrt(Math.Pow((xi4 - xi3), 2) + Math.Pow((yi4 - yi3), 2) + Math.Pow((zi4 - zi3), 2));

        return  ((di1 * 100 )/ 2) * ((di2 * 100 )/ 2) * Math.PI;
    }

    //area ojo derecho
    public Double getAreaODerecho(int i, CameraSpacePoint point)
    {
        if (i == 843)
        {
            xd1 = point.X;
            yd1 = point.Y;
            zd1 = point.Z;
        }
        if (i == 1117)
        {
            xd2 = point.X;
            yd2 = point.Y;
            zd2 = point.Z;
        }
        if (i == 731)
        {
            xd3 = point.X;
            yd3 = point.Y;
            zd3 = point.Z;
        }
        if (i == 1090)
        {
            xd4 = point.X;
            yd4 = point.Y;
            zd4 = point.Z;
        }
        dd1 = Math.Sqrt(Math.Pow((xd2 - xd1), 2) + Math.Pow((yd2 - yd1), 2) + Math.Pow((zd2 - zd1), 2));
        dd2 = Math.Sqrt(Math.Pow((xd4 - xd3), 2) + Math.Pow((yd4 - yd3), 2) + Math.Pow((zd4 - zd3), 2));

        return ((dd1 *100)/ 2) * ((dd2 *100) / 2) * Math.PI;
    }

    //area ajustada ojos
    public double getAreaAOjos(double ai, double ad)
    {
        return 150 * Math.Pow(ai - ad, 2);
    }

    //angulo ajustado angulo boca
    public double getAABoca(double angulo)
    {
        return 1 * Math.Pow(90 - angulo, 2);
    }

    //calcular angulo boca
    public Double getAnguloUV(int i, CameraSpacePoint point)
    {
        if (i == 91)
        {
            x1 = point.X;
            y1 = point.Y;
            z1 = point.Z;
        }
        if (i == 687)
        {
            x2 = point.X;
            y2 = point.Y;
            z2 = point.Z;
        }
        if (i == 4)
        {
            x3 = point.X;
            y3 = point.Y;
            z3= point.Z;
        }
        if (i == 18)
        {
            x4 = point.X;
            y4 = point.Y;
            z4 = point.Z;
        }

        ux = x2 - x1;
        uy = y2 - y1;
        uz = z2 - z1;

        vx = x4 - x3;
        vy = y4 - y3;
        vz = z4 - z3;

        mu = Math.Sqrt(Math.Pow(ux, 2) + Math.Pow(uy, 2) + Math.Pow(uz, 2));
        mv = Math.Sqrt(Math.Pow(vx, 2) + Math.Pow(vy, 2) + Math.Pow(vz, 2));
        //obtener el angulo y transformar a grados
        return Math.Acos(((ux * vx) + (uy * vy) + (uz * vz)) / (mu * mv)) * (180 / Math.PI);
    }

    //calcular simetria ajustada
    public double getISACejas(double gamma_cejas)
    {
        return 100 - (gamma_cejas / 100);
    }

    public double getISAOjos(double gamma_ojos)
    {
        return 100 - (gamma_ojos / 100);
    }

    public double getISABoca(double gamma_boca)
    {
        return 100 - (gamma_boca / 100);
    }

    //simetria final ojos
    public double getIFOjos(double ojos_ajustada, double ojos_area)
    {
        return 100 - ojos_ajustada - ojos_area;
    }

    //simetria final boca
    public double getIFBoca(double boca_ajustada, double boca_angulo)
    {
        return 100 - boca_ajustada - boca_angulo;
    }

}
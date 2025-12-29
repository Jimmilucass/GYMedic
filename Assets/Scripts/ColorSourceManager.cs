using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using Microsoft.Kinect.Face;

public class ColorSourceManager : MonoBehaviour
{
    public int ColorWidth { get; private set; }
    public int ColorHeight { get; private set; }

    private KinectSensor Sensor;
    private ColorFrameReader Reader;
    private Texture2D Texture;
    private byte[] datos;

    //variables reconocimiento
    private BodyFrameSource bodySource = null;
    private BodyFrameReader bodyReader = null;
    private HighDefinitionFaceFrameSource faceSource = null;
    private HighDefinitionFaceFrameReader faceReader = null;
    private FaceAlignment faceAlignment = null;
    private FaceModel faceModel = null;
    private GameObject h1;
    private GameObject[] go = new GameObject[17];

    //instanciar clases
    private Indice_Reposo indice_reposo = new Indice_Reposo();
    private Simetria_Ajuste simetria_ajuste = new Simetria_Ajuste();

    //variables gestos
    private FaceFrameSource faceFrameSource = null;
    private FaceFrameReader faceFrameReader = null;

    //varialbles de indice de simetria
    Double cejas, ojos, boca, z1, z2, por_error;

    //variables simetria reposo
    private double ceja_reposo, ojos_reposo, boca_reposo;
    public Text text_restc, text_resto, text_restb;

    //variables correcion gamma
    private double ceja_gamma, ojos_gamma, boca_gamma, ojos_area_gamma, boca_angulo_gamma;
    public Text text_gammac, text_gammao, text_gammab, text_area_gammao, text_angulo_gammab;

    //variables simetria ajustada
    private double ceja_ajustada, ojos_ajustada, boca_ajustada;
    public Text text_adjc, text_adjo, text_adjb;

    //variables areas ojos
    private double ojo_i, ojo_d;
    public Text text_areaoi, text_areaod;

    //varibale angulo boca
    private double boca_angulo;
    public Text text_angulob;

    //variables simetria final
    private double ojos_final, boca_final;
    public Text text_finalo, text_finalb;

    //varible panel
    private Image p_notificacion;

    public Texture2D GetColorTexture()
    {
        return Texture;
    }

    void Start()
    {
        //instanciar la elipse que servira como punto para reconocimiento facial
        h1 = GameObject.Find("Elipse");
        //instanciar el panel de estado
        p_notificacion = GameObject.Find("P Status").GetComponent<Image>();
        //instanciar los 16 puntos a usarse en el reconocimiento facial
        getPuntosInstantiate();

        //iniciar sensor
        Sensor = KinectSensor.GetDefault();
        if (Sensor != null)
        {
            Reader = Sensor.ColorFrameSource.OpenReader();

            var frameDesc = Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            ColorWidth = frameDesc.Width;
            ColorHeight = frameDesc.Height;
            print("medidas pantalla: " + ColorWidth + " " + ColorHeight);

            Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
            datos = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
                       
            bodySource = Sensor.BodyFrameSource;
            bodyReader = bodySource.OpenReader();
            bodyReader.FrameArrived += BodyReader_FrameArrived;

            faceSource = HighDefinitionFaceFrameSource.Create(Sensor);
            faceReader = faceSource.OpenReader();
            faceReader.FrameArrived += FaceReader_FrameArrived;

            faceAlignment = FaceAlignment.Create();
            faceModel = FaceModel.Create();

            Sensor.Open();
        }
    }

    private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
    {
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null && frame.IsFaceTracked)
            {
                frame.GetAndRefreshFaceAlignmentResult(faceAlignment);
                UpdateFacePoints();
            }
        }
    }

    private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null)
            {
                Body[] bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);

                Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                if (!faceSource.IsTrackingIdValid)
                {
                    if (body != null)
                    {
                        faceSource.TrackingId = body.TrackingId;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Reader != null)
        {
            var frame = Reader.AcquireLatestFrame();

            if (frame != null)
            {
                frame.CopyConvertedFrameDataToArray(datos, ColorImageFormat.Rgba);
                Texture.LoadRawTextureData(datos);

                Texture.Apply();

                frame.Dispose();
                frame = null;
            }
        }
        //validacion barra estado kienct
        if (Sensor.IsAvailable.Equals(true))
        {
            p_notificacion.color = UnityEngine.Color.green;
        }
        else
        {
            p_notificacion.color = UnityEngine.Color.red;
        }
    }

    void OnApplicationQuit()
    {
        if (Reader != null)
        {
            Reader.Dispose();
            Reader = null;
        }
        if (Sensor != null)
        {
            if (Sensor.IsOpen)
            {
                Sensor.Close();
            }
            Sensor = null;
        }
    }

    private void UpdateFacePoints()
    {
        if (faceModel == null) return;

        if (faceFrameReader != null)
        {
            faceFrameReader.Dispose();
        }

        var vertices = faceModel.CalculateVerticesForAlignment(faceAlignment);

        if (vertices.Count > 0)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                //filtar los 16 puntos necesarios para el proyecto
                if (i == 4 || i == 8 || i == 18 || i == 19 || i == 91 || i == 210 || i == 222 || i == 241 ||
                    i == 469 || i == 687 || i == 731 || i == 843 || i == 849 || i == 1090 || i == 1104 || i == 1117
                    || i == 412 || i == 933)
                {
                    
                    //Representa un punto 3D en el espacio de la cámara (en metros)
                    CameraSpacePoint vertice = vertices[i];

                    //Representa coordenadas de píxeles de una imagen de profundidad dentro
                    DepthSpacePoint point = Sensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);


                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y))
                        return;

                    //transformar puntos
                    float tx = point.X;
                    float ty = point.Y;

                    //dibujar los 16 puntos del rostro                 
                    getPuntos(i, tx, -ty);

                    //obtener las coordenadas en z del centro de la mejilla
                    if (i == 412)
                    {
                        z1 = vertice.Z;
                    }

                    if (i == 933)
                    {
                        z2 = vertice.Z;
                    }
                    //calcular porcentaje de error de medicion
                    if (z1 < z2)
                        por_error = (z1 / z2) * 100;
                    else
                        por_error = (z2 / z1) * 100;
                    por_error = 100 - por_error;

                    //validar el envio de datos
                    if (por_error <= 2f)
                    {
                        //calcular indice reposo
                        ceja_reposo = indice_reposo.getCReposo(i, vertice);
                        ojos_reposo = indice_reposo.getOReposo(i, vertice);
                        boca_reposo = indice_reposo.getBReposo(i, vertice);

                        //calcular indice simetria ajustado
                        ojo_i = simetria_ajuste.getAreaOIzquierdo(i, vertice);
                        ojo_d = simetria_ajuste.getAreaODerecho(i, vertice);
                        boca_angulo = simetria_ajuste.getAnguloUV(i, vertice);

                        //aplicar correcion gamma indice reposo
                        ceja_gamma = indice_reposo.getGCejas(ceja_reposo / 100);
                        ojos_gamma = indice_reposo.getGOjos(ojos_reposo / 100);
                        boca_gamma = indice_reposo.getGBoca(boca_reposo / 100);

                        //calcular indice de simetria ajustado
                        ceja_ajustada = simetria_ajuste.getISACejas(ceja_gamma);
                        ojos_ajustada = simetria_ajuste.getISAOjos(ojos_gamma);
                        boca_ajustada = simetria_ajuste.getISABoca(boca_gamma);

                        //correcion gamma areas, angulo, ojos y boca
                        ojos_area_gamma = simetria_ajuste.getAreaAOjos(ojo_i, ojo_d);
                        boca_angulo_gamma = simetria_ajuste.getAABoca(boca_angulo);

                        //calcular simetria final ajustada
                        ojos_final = simetria_ajuste.getIFOjos((ojos_ajustada / 100), ojos_area_gamma);
                        boca_final = simetria_ajuste.getIFBoca((boca_ajustada / 100), boca_angulo_gamma);

                        //imprimir resultados
                        text_restc.text = String.Format("{0:0.000}", ceja_reposo) + " %";
                        text_resto.text = String.Format("{0:0.000}", ojos_reposo) + " %";
                        text_restb.text = String.Format("{0:0.000}", boca_reposo) + " %";

                        text_gammac.text = String.Format("{0:0.000}", ceja_gamma) + " %";
                        text_gammao.text = String.Format("{0:0.000}", ojos_gamma) + " %";
                        text_gammab.text = String.Format("{0:0.000}", boca_gamma) + " %";

                        text_adjc.text = String.Format("{0:0.000}", ceja_ajustada) + " %";
                        text_adjo.text = String.Format("{0:0.000}", ojos_ajustada) + " %";
                        text_adjb.text = String.Format("{0:0.000}", boca_ajustada) + " %";

                        text_areaoi.text = String.Format("{0:0.000}", ojo_i);
                        text_areaod.text = String.Format("{0:0.000}", ojo_d);
                        text_angulob.text = String.Format("{0:0.000}", boca_angulo);

                        text_area_gammao.text = String.Format("{0:0.000}", ojos_area_gamma);
                        text_angulo_gammab.text = String.Format("{0:0.000}", boca_angulo_gamma);

                        text_finalo.text = String.Format("{0:0.000}", ojos_final);
                        text_finalb.text = String.Format("{0:0.000}", boca_final);


                    }
                }
            }
        }
        /*else
        {
            print("Sensor not Found");
        }*/
    }

    //crear 16 puntos a partir de uno con Instantiate()
    private void getPuntosInstantiate()
    {
        for (int i = 1; i <= 16; i++)
        {
            go[i] = Instantiate(h1);
        }
    }

    private void getPuntos(int i, float x, float y)
    {
        if (i == 4)
        {
            go[1].transform.position = new Vector3(x, y, -1);
        }
        if (i == 8)
        {
            go[2].transform.position = new Vector3(x, y, -1);
        }
        if (i == 18)
        {
            go[3].transform.position = new Vector3(x, y, -1);
        }
        if (i == 19)
        {
            go[4].transform.position = new Vector3(x, y, -1);
        }
        if (i == 91)
        {
            go[5].transform.position = new Vector3(x, y, -1);
        }
        if (i == 210)
        {
            go[6].transform.position = new Vector3(x, y, -1);
        }
        if (i == 222)
        {
            go[7].transform.position = new Vector3(x, y, -1);
        }
        if (i == 241)
        {
            go[8].transform.position = new Vector3(x, y, -1);
        }
        if (i == 469)
        {
            go[9].transform.position = new Vector3(x, y, -1);
        }
        if (i == 687)
        {
            go[10].transform.position = new Vector3(x, y, -1);
        }
        if (i == 731)
        {
            go[11].transform.position = new Vector3(x, y, -1);
        }
        if (i == 843)
        {
            go[12].transform.position = new Vector3(x, y, -1);
        }
        if (i == 849)
        {
            go[13].transform.position = new Vector3(x, y, -1);
        }
        if (i == 1090)
        {
            go[14].transform.position = new Vector3(x, y, -1);
        }
        if (i == 1104)
        {
            go[15].transform.position = new Vector3(x, y, -1);
        }
        if (i == 1117)
        {
            go[16].transform.position = new Vector3(x, y, -1);
        }
    }
}

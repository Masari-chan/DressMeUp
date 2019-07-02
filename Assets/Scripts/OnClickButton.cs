using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OnClickButton : MonoBehaviour
{
    const int MAX_ANIMATION_NUMBER = 5;
    const string LEFT_KEY = "Left";
    const string RIGHT_KEY = "Right";
    const string HAIR_KEY = "Hair";
    const string BOT_KEY = "Bot";
    const string TOP_KEY = "Top";
    const string SHOES_KEY = "Shoes";
    public GameObject[] HairArray;
    public GameObject[] BotArray;
    public GameObject[] TopArray;
    public GameObject[] ShoesArray;



    // array de animaciones para ropa
    public GameObject[] BlusaArray;
    public GameObject[] TopVolantesArray;
    public GameObject[] FaldaLargaArray;
    public GameObject[] FaldaCortaArray;
    public GameObject mainMenuButtons, editionButtons, photocallButtons;
    private GameObject TopSpecialAnimation;
    private GameObject BottomSpecialAnimation;


    private bool takeScreenshotOnNextFrame;

    // COLOR PELO
    [Tooltip("Array de colores de pelo disponibles")]
    [SerializeField]
    public Color[] hairColors;
    // Representa el color actual de pelo que se está representando.
    private int currentHairColor = 0;

    // COLOR PIEL
    [Tooltip("Array de colores de pelo disponibles")]
    [SerializeField]
    public Color[] skinColors;
    // Representa el color actual de piel que se está representando.
    private int currentSkinColor = 0;

    // COLOR OJOS
    [Tooltip("Array de texturas de ojos")]
    [SerializeField]
    public Texture[] eyesTextures;
    // Representa la textura actual de ojos que se está representando.
    private int currentEyeTexture = 0;

    [Tooltip("Material del pelo")]
    [SerializeField]
    private Material materialPelo;

    [Tooltip("Material de la piel")]
    [SerializeField]
    private Material materialPiel;

    [Tooltip("Material de los ojos")]
    [SerializeField]
    private Material materialOjos;

    [Tooltip("Canvas de Edición")]
    [SerializeField]
    private GameObject canvasEdicion;

    [Tooltip("Canvas de Photo")]
    [SerializeField]
    private GameObject canvasPhoto;

    // false = Canvas de edición; true = canvas de Foto.
    private bool currentCanvas = false;


    private bool currentCanvas1 = true;

    [Tooltip("Array de texturas de fondos")]
    [SerializeField]
    public Texture[] wallPapers;

    [Tooltip("Objeto de fondo de fotocall")]
    [SerializeField]
    private Material wallpaperMaterial;

    [Tooltip("Cámara")]
    [SerializeField]
    private Camera camara;

    [Tooltip("Nombre de snapshot")]
    [SerializeField]
    private string snapshotName = "snapshot";

    [Tooltip("Canvas de mensajes")]
    [SerializeField]
    private GameObject canvasMensajes;

    [Tooltip("GameObject del suelo")]
    [SerializeField]
    private GameObject floorItem;

    [Tooltip("GameObject de objeto de mensaje")]
    [SerializeField]
    private Text mensajeTexto;




    private int currentWP = 0;

    private int currentSnapshot;

    Animator myAnimator;
    int actualAnimationIndex = 0;
    int actualHairIndex = 0;
    int actualBotIndex = 0;
    int actualTopIndex = 0;
    int actualShoesIndex = 0;

    public AnimationClip Animacion1;

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    // Cambia el color de pelo. 
    // Parámetro direccion: true = derecha; false = izquierda.
    public void changeHairColor(bool direccion)
    {
        if (direccion)
        {
            currentHairColor++;
            if (currentHairColor > hairColors.Length - 1) { currentHairColor = 0; }
        }
        else
        {
            currentHairColor--;
            if (currentHairColor < 0) { currentHairColor = hairColors.Length - 1; }
        }

        materialPelo.color = hairColors[currentHairColor];
    }

    // Cambia el color de piel. 
    // Parámetro direccion: true = derecha; false = izquierda.
    public void changeSkinColor(bool direccion)
    {
        if (direccion)
        {
            currentSkinColor++;
            if (currentSkinColor > skinColors.Length - 1) { currentSkinColor = 0; }
        }
        else
        {
            currentSkinColor--;
            if (currentSkinColor < 0) { currentSkinColor = skinColors.Length - 1; }
        }
        materialPiel.color = skinColors[currentSkinColor];
        // materialPiel.SetColor("_Color", skinColors[currentSkinColor]);
    }

    // Cambia el color de de los ojos. 
    // Parámetro direccion: true = derecha; false = izquierda.
    public void changeEyeColor(bool direccion)
    {
        if (direccion)
        {
            currentEyeTexture++;
            if (currentEyeTexture > eyesTextures.Length - 1) { currentEyeTexture = 0; }
        }
        else
        {
            currentEyeTexture--;
            if (currentEyeTexture < 0) { currentEyeTexture = eyesTextures.Length - 1; }
        }
        materialOjos.SetTexture("_MainTex", eyesTextures[currentEyeTexture]);
    }

    // Cambia el fondo 
    // Parámetro direccion: true = derecha; false = izquierda.
    public void changeWallPaper(bool direccion)
    {
        if (direccion)
        {
            currentWP++;
            if (currentWP > wallPapers.Length - 1) { currentWP = 0; }
        }
        else
        {
            currentWP--;
            if (currentWP < 0) { currentWP = wallPapers.Length - 1; }
        }
        wallpaperMaterial.SetTexture("_MainTex", wallPapers[currentWP]);
    }

    public void backToFirstWallpaper()
    {
        currentWP = 0;
        wallpaperMaterial.SetTexture("_MainTex", wallPapers[currentWP]);
        floorItem.SetActive(false);

    }



    // public void takeScreenshot(int width, int height) {
    public void takeScreenshot()
    {
        canvasEdicion.SetActive(false);
        canvasPhoto.SetActive(false);
        if (!System.IO.Directory.Exists(Application.dataPath + "/snapshots/"))
            System.IO.Directory.CreateDirectory(Application.dataPath + "/snapshots/");
        // int height = Mathf.RoundToInt(2f * camara.orthographicSize);
        // int width = Mathf.RoundToInt(height * camara.aspect);
        // camara.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/snapshots/" + snapshotName + currentSnapshot.ToString() + ".png");
        StartCoroutine(bringBackCanvas());


        // takeScreenshotOnNextFrame = true;
        // OnPostRender();
    }

    IEnumerator bringBackCanvas()
    {
        mensajeTexto.text = "¡The screenshot " + snapshotName + currentSnapshot.ToString() + ".png has been saved!";
        //canvasMensajes.SetActive(true);
        yield return new WaitForSeconds(1);
        if (currentCanvas)
        {
            canvasPhoto.SetActive(true);
        }
        else
        {
            canvasEdicion.SetActive(true);
        }
        currentSnapshot++;
        canvasMensajes.SetActive(true);
        StartCoroutine(hideMensajes());
    }

    IEnumerator hideMensajes()
    {
        yield return new WaitForSeconds(2);
        canvasMensajes.SetActive(false);
    }

    // public void takeScreenshot_Static(int width, int height) {
    //     takeScreenshot(width, height);
    // }

    private IEnumerator OnPostRender()
    {
        yield return frameEnd;
        Debug.Log("He terminado de renderizar.");
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = camara.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
            Debug.Log("Captura realizada");

            RenderTexture.ReleaseTemporary(renderTexture);
            camara.targetTexture = null;

            if (currentCanvas)
            {
                canvasPhoto.SetActive(true);
            }
            else
            {
                canvasEdicion.SetActive(true);
            }
        }
        else
        {
            Debug.Log("ALGO HA IDO MAL XD");
        }
    }

    public void OnButtonPressed(string actualTypeOfCloth)
    {
        ResetSpecialAnimation();
        myAnimator.SetBool("ButtonPressed", true);
        myAnimator.SetInteger("AnimationIndex", actualAnimationIndex);
        if (actualTypeOfCloth.Contains(HAIR_KEY))
        {
            if (actualTypeOfCloth.Contains(RIGHT_KEY))
            {
                HairArray[actualHairIndex].SetActive(false);
                actualHairIndex++;
                if (actualHairIndex >= HairArray.Length)
                {
                    actualHairIndex = 0;
                }
                HairArray[actualHairIndex].SetActive(true);
            }
            else if (actualTypeOfCloth.Contains(LEFT_KEY))
            {
                HairArray[actualHairIndex].SetActive(false);
                actualHairIndex--;
                if (actualHairIndex < 0)
                {
                    actualHairIndex = HairArray.Length - 1;
                }
                HairArray[actualHairIndex].SetActive(true);
            }
        }
        //shoes
        else if (actualTypeOfCloth.Contains(SHOES_KEY))
        {
            if (actualTypeOfCloth.Contains(RIGHT_KEY))
            {
                ShoesArray[actualShoesIndex].SetActive(false);
                actualShoesIndex++;
                if (actualShoesIndex >= ShoesArray.Length)
                {
                    actualShoesIndex = 0;
                }
                ShoesArray[actualShoesIndex].SetActive(true);
            }
            else if (actualTypeOfCloth.Contains(LEFT_KEY))
            {
                ShoesArray[actualShoesIndex].SetActive(false);
                actualShoesIndex--;
                if (actualShoesIndex < 0)
                {
                    actualShoesIndex = ShoesArray.Length - 1;
                }
                ShoesArray[actualShoesIndex].SetActive(true);
            }
        }
        // end shoes
        else if (actualTypeOfCloth.Contains(TOP_KEY))
        {
            if (actualTypeOfCloth.Contains(RIGHT_KEY))
            {

                TopArray[actualTopIndex].SetActive(false);
                actualTopIndex++;
                if (actualTopIndex >= TopArray.Length)
                {
                    actualTopIndex = 0;
                }
                if (TopArray[actualTopIndex] == BlusaArray[0] || TopArray[actualTopIndex] == TopVolantesArray[0])
                {
                    TopSpecialAnimation = TopArray[actualTopIndex];
                }
                else
                {
                    TopSpecialAnimation = null;
                    TopArray[actualTopIndex].SetActive(true);
                }

            }
            else if (actualTypeOfCloth.Contains(LEFT_KEY))
            {

                TopArray[actualTopIndex].SetActive(false);
                actualTopIndex--;
                if (actualTopIndex < 0)
                {
                    if (actualTopIndex < 0)
                        actualTopIndex = TopArray.Length - 1;
                }
                if (TopArray[actualTopIndex] == BlusaArray[0] || TopArray[actualTopIndex] == TopVolantesArray[0])
                {
                    TopSpecialAnimation = TopArray[actualTopIndex];
                }
                else
                {
                    TopSpecialAnimation = null;
                    TopArray[actualTopIndex].SetActive(true);
                }
            }
        }
        else if (actualTypeOfCloth.Contains(BOT_KEY))
        {

            if (actualTypeOfCloth.Contains(RIGHT_KEY))
            {

                BotArray[actualBotIndex].SetActive(false);
                actualBotIndex++;
                if (actualBotIndex >= BotArray.Length)
                {
                    actualBotIndex = 0;
                }
                if (BotArray[actualBotIndex] == FaldaLargaArray[0] || BotArray[actualBotIndex] == FaldaCortaArray[0])
                {
                    BottomSpecialAnimation = BotArray[actualBotIndex];
                }
                else
                {
                    BottomSpecialAnimation = null;
                    BotArray[actualBotIndex].SetActive(true);
                }
            }
            else if (actualTypeOfCloth.Contains(LEFT_KEY))
            {

                BotArray[actualBotIndex].SetActive(false);
                actualBotIndex--;
                if (actualBotIndex < 0)
                {
                    if (actualBotIndex < 0)
                        actualBotIndex = BotArray.Length - 1;
                }
                if (BotArray[actualBotIndex] == FaldaLargaArray[0] || BotArray[actualBotIndex] == FaldaCortaArray[0])
                {
                    BottomSpecialAnimation = BotArray[actualBotIndex];
                }
                else
                {
                    BottomSpecialAnimation = null;
                    BotArray[actualBotIndex].SetActive(true);
                }
            }
        }
        else if (actualTypeOfCloth == "action")
        {
            // Estamos pulsando el action button
            if (BotArray[actualBotIndex] == FaldaLargaArray[0] || BotArray[actualBotIndex] == FaldaCortaArray[0])
            {
                BottomSpecialAnimation = BotArray[actualBotIndex];
            }
            else
            {
                BottomSpecialAnimation = null;
                BotArray[actualBotIndex].SetActive(true);
            }
            if (TopArray[actualTopIndex] == BlusaArray[0] || TopArray[actualTopIndex] == TopVolantesArray[0])
            {
                TopSpecialAnimation = TopArray[actualTopIndex];
            }
            else
            {
                TopSpecialAnimation = null;
                TopArray[actualTopIndex].SetActive(true);
            }

        }

        if (actualTypeOfCloth != "")
        {

            if (TopSpecialAnimation != null)
            {
                if (TopSpecialAnimation == BlusaArray[0])
                {
                    BlusaArray[actualAnimationIndex].SetActive(true);

                }
                else if (TopSpecialAnimation == TopVolantesArray[0])
                {
                    TopVolantesArray[actualAnimationIndex].SetActive(true);

                }
            }
            if (BottomSpecialAnimation != null)
            {
                if (BottomSpecialAnimation == FaldaLargaArray[0])
                {
                    FaldaLargaArray[actualAnimationIndex].SetActive(true);

                }
                else if (BottomSpecialAnimation == FaldaCortaArray[0])
                {
                    FaldaCortaArray[actualAnimationIndex].SetActive(true);

                }
            }
            actualAnimationIndex++;
            if (actualAnimationIndex >= MAX_ANIMATION_NUMBER)
            {
                actualAnimationIndex = 0;
            }
        }
    }
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        camara = camara.GetComponent<Camera>();
        // Si existe la carpeta de snapshots
        if (System.IO.Directory.Exists(Application.dataPath + "/snapshots/"))
        {
            // Accedemos a la carpeta
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "/snapshots/");
            int counter = 0;
            // Entramos en el fichero de snapshots.
            foreach (var item in dir.GetFiles())
            {
                if (item.Name[item.Name.Length - 1] == 'g') { counter++; }
            }
            // De esta forma sabemos cuántas snapshots se han echado hasta la fecha
            currentSnapshot = counter;
        }
        else
        {
            currentSnapshot = 0;
        }

    }
    public void ResetButtonPressed()
    {
        myAnimator.SetBool("ButtonPressed", false);
    }

    public void GoBackToBreathing()
    {
        myAnimator.SetTrigger("BackToBreathing");
        Debug.Log("He sido invocado");
    }

    public void ResetSpecialAnimation()
    {
        foreach (GameObject g in BlusaArray)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in TopVolantesArray)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in FaldaLargaArray)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in FaldaCortaArray)
        {
            g.SetActive(false);
        }
    }
    public void OnPlayButtonPressed()
    {

        CameraMovement.Instance.MoveCameraToStandardPosition();

        mainMenuButtons.SetActive(false);
        editionButtons.SetActive(true);
        photocallButtons.SetActive(false);

    }

    public void OnZoomInuttonPressed(bool onPhotocallCanvas = false)
    {
        if (currentCanvas1)
        {
            currentCanvas1 = !currentCanvas1;
            CameraMovement.Instance.MoveCameraToZoomInPosition();
            if (!onPhotocallCanvas)
            {
                mainMenuButtons.SetActive(false);
                editionButtons.SetActive(true);
                photocallButtons.SetActive(false);
                canvasEdicion.SetActive(true);
            }
            Debug.Log(currentCanvas1);
        }
        else
        {
            currentCanvas1 = !currentCanvas1;
            CameraMovement.Instance.MoveCameraToStandardPosition();
            if (!onPhotocallCanvas)
            {
                mainMenuButtons.SetActive(false);
                editionButtons.SetActive(true);
                photocallButtons.SetActive(false);
                canvasEdicion.SetActive(true);
            }
        }
        //currentCanvas1 = !currentCanvas1;

        /* CameraMovement.Instance.MoveCameraToZoomInPosition();

       mainMenuButtons.SetActive(false);
       editionButtons.SetActive(true);
       photocallButtons.SetActive(false);
       canvasEdicion.SetActive(true);
       */

    }
    public void OnBackToEditionButtonPressed()
    {

        //CameraMovement.Instance.MoveCameraToStandardPosition();

        mainMenuButtons.SetActive(false);
        editionButtons.SetActive(true);
        photocallButtons.SetActive(false);
        canvasEdicion.SetActive(true);
        backToFirstWallpaper();

    }
    public void OnBackToMainButtonPressed()
    {

        CameraMovement.Instance.MoveCameraToMainMenuPosition();
        mainMenuButtons.SetActive(true);
        editionButtons.SetActive(false);
        photocallButtons.SetActive(false);
        canvasEdicion.SetActive(true);


    }
    // Cambia el canvas 
    public void changeCanvas()
    {
        if (currentCanvas)
        {
            canvasEdicion.SetActive(true);
            canvasPhoto.SetActive(false);
        }
        else
        {
            canvasEdicion.SetActive(false);
            canvasPhoto.SetActive(true);
            floorItem.SetActive(true);
        }
        currentCanvas = !currentCanvas;
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("has quit game");
        Application.Quit();

    }

    public void CharacterBreathing()
    {

        ResetSpecialAnimation();

        if (BottomSpecialAnimation != null)
        {
            if (BottomSpecialAnimation == FaldaLargaArray[0])
            {


                FaldaLargaArray[FaldaLargaArray.Length - 1].SetActive(true);
            }
            else if (BottomSpecialAnimation == FaldaCortaArray[0])
            {


                FaldaCortaArray[FaldaCortaArray.Length - 1].SetActive(true);
            }
        }
        if (TopSpecialAnimation != null)
        {
            if (TopSpecialAnimation == BlusaArray[0])
            {


                BlusaArray[BlusaArray.Length - 1].SetActive(true);
            }
            else if (TopSpecialAnimation == TopVolantesArray[0])
            {


                TopVolantesArray[TopVolantesArray.Length - 1].SetActive(true);
            }
        }

    }
}

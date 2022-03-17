using Jypeli;
using System.Threading.Tasks;
using System.ComponentModel;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;

public class Harkkapeli : PhysicsGame
///@Author: Annastiina Miettinen
///@Version: 15/04/2021
///Game: Penumbra







{
    private int i = -1; /// i on Juoni.txt rivin numero.
    private int j = 0; /// j on Vastaukset.txt rivin numero.

    private readonly Label tekstikentta = new Label(); /// tekstikentt‰ on label joka n‰ytt‰‰ teksti‰
    private Widget alusta = new Widget(1080, 720); /// alusta on emo Widget mihin muut Widgetit yhdistyy.

    private const string SANAT = "Juoni.txt";  ///SANAT pit‰‰ sis‰ll‰‰n Juoni.txt sis‰llˆn.
    private string[] script = File.ReadAllLines(SANAT); /// script on string taulukko joka sis‰lt‰‰ Juoni.txt rivi kerrallaan.

    private Pelaaja nykyinenPelaaja = new Pelaaja(); /// nykyinenPelaaja on peli‰ pelaava henkilˆ.

    public override void Begin()
    {
       SetWindowSize(1080, 720, false);
       Level.Size = new Vector(1080, 720);
       Camera.ZoomToLevel();
       Level.CreateBorders(false);


       MultiSelectWindow alkuValikko = new MultiSelectWindow("",
       "Start the Journey", "Quit");

       Add(alkuValikko);

       Image alkuKuva = LoadImage("Title");
       Level.Background.Image = alkuKuva;

       alkuValikko.AddItemHandler(0, AloitaPeli);
       alkuValikko.AddItemHandler(1, Exit);

       Keyboard.Listen(Key.Space, ButtonState.Pressed, Scene, "Dialogi etenee");
       Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    /// <summary>
    /// Alustaa kaiken tarpeellisen pelin toimivuuden kannalta.
    /// </summary>
    private void AloitaPeli()

    {
        alusta.Color = Color.Transparent;
        Add(alusta);


        Widget teksti = new Widget(1080, 200);
        teksti.BorderColor = Color.Black;
        teksti.Color = Color.AshGray;
        teksti.Position = new Vector(0, Level.Bottom + 90);
        teksti.Add(tekstikentta);
        alusta.Add(teksti);

        Level.Background.Image = Kuvalista("default");

        Scene();

    }


    /// <summary>
    /// Sis‰lt‰‰ peliss‰ k‰ytett‰v‰t taustat ja palauttaa halutun taustan.
    /// </summary>
    /// <param name="avain"> Tietty‰ taustaa vastaava avain sana.</param>
    /// <returns>Halutun taustan.</returns>
   private Image Kuvalista(string avain)
      
    {
       Image metsa1 = LoadImage("Metsa1");
       Image metsa2 = LoadImage("Metsa2");
       Image vankila = LoadImage("Vanki1");
       Image vankila2 = LoadImage("Vanki2");
       Image kellari = LoadImage("Kellari1");
       Image linna1 = LoadImage("Linna1");
       Image linna2 = LoadImage("Linna2");
       Image linna3 = LoadImage("Linna3");


        Dictionary<string, Image> taustakuvat = new Dictionary<string, Image>();

       taustakuvat.Add("Forest:",metsa1);
       taustakuvat.Add("Deep Forest:",metsa2);
       taustakuvat.Add("Prison:",vankila);
       taustakuvat.Add("default", kellari);
       taustakuvat.Add("Prison Hall:", vankila2);
       taustakuvat.Add("Courtyard:", linna1);
       taustakuvat.Add("Castle hall:", linna2);
       taustakuvat.Add("Throne room:", linna3);

        return taustakuvat[avain];
    }


    /// <summary>
    /// Ottaa vastaan pelaajan nimen ja tallentaa sen nykyisenPelaajan nimeksi.
    /// </summary>
    /// <param name="ikkuna">kysymysikkunasta saatu vastaus</param>
    private void PelaajanNimi(InputWindow ikkuna)
    
    {
        bool done = false;

         while (!done)
         {
           string nimi = ikkuna.InputBox.Text;
           if (nimi == "10.6")
           {
            nykyinenPelaaja.nimi = "Cola";
           }
           else nykyinenPelaaja.nimi = nimi;

           if (nimi.Length > 1) done = true;
           else done = true;
          }
    }


    /// <summary>
    /// K‰sittelee valintaikkunoiden syˆtteen ja antaa vastauksen perustuen erillisen tekstitiedoston perusteella.
    /// </summary>
    /// <param name="valinta">Joko kyll‰ (0) tai ei (1)</param>
    private void ValintaIkkuna(int valinta)
    {
        const string SANAT = "Vastaukset.txt";
        string[] vastaus = File.ReadAllLines(SANAT);

        switch (valinta)
        {
          case 0:

                j++;

                if (vastaus[j].Contains("[TALK]"))
                {
                    tekstikentta.Text = "";
                    j = j + 1;

                    break;
                }

                else
                {
                    tekstikentta.Text = vastaus[j];
                    j = j + 1;

                }

                break;


          case 1:

                j++;
                tekstikentta.Text = vastaus[j+1];
                j = j + 1;
                break;
        }

        
    }


    /// <summary>
    /// Pit‰‰ sis‰ll‰‰n jokaisen avainsanan mit‰ k‰ytet‰‰n taustakuvan vaihtamiseen.
    /// </summary>
    /// <returns>Palauttaa avainsanan.</returns>
    private List <string> Avainsanat ()
    {
        List<string> avainsanat = new List<string>();
        avainsanat.Add("Forest:");
        avainsanat.Add("Deep Forest:");
        avainsanat.Add("Prison:");
        avainsanat.Add("Prison Hall:");
        avainsanat.Add("Courtyard:");
        avainsanat.Add("Castle hall:");
        avainsanat.Add("Throne room:");

        return avainsanat;
    }


    /// <summary>
    /// Luo jokaisen scenen yhdist‰en tekstin, taustakuvan ja tarkistaa sis‰lt‰‰kˆ annettu rivi tiettyj‰ sanoja, jotka
    /// laukaisevat erillisen tapahtuman (kuten taustanvaihdoksen tai kysymyksen).
    /// </summary>
    private void Scene()
    {
        Klikkaus();
        
        tekstikentta.Text = script[i];

        foreach (string avain in Avainsanat())
        {

        if (script[i].Contains(avain)) Level.Background.Image = Kuvalista(avain);
          
        }
        
        if (script[i].Contains("[Name]"))
        {
           tekstikentta.Text = "";
           InputWindow kysymysIkkuna = new InputWindow("Please write your name here");
           kysymysIkkuna.TextEntered += PelaajanNimi;
           Add(kysymysIkkuna);
        }

        if (script[i].Contains("[Choice]"))
        {
           tekstikentta.Text = "";
          MultiSelectWindow valinta = new MultiSelectWindow(script[i], "Yes", "No");
          valinta.ItemSelected += ValintaIkkuna;
          valinta.DefaultCancel = -1;
          valinta.Color = Color.BlueGray;
          Add(valinta);
        }

        if (script[i].Contains("[nimi]"))
        {
            tekstikentta.Text = nykyinenPelaaja.nimi;
        }

        if (script[i].Contains("[NPC]"))
        {
            tekstikentta.Text = "";
            NPCKuvat(script [i]);
          
        }

        //if (script[i].Contains("[TALK]"))
       // {
           // NPCDialogi();
        //}

        if (script[i].Contains("[END]"))
        {
            Exit();
        }

    }


    /// <summary>
    /// Pit‰‰ sis‰ll‰‰n NPC hahmojen kuvat liitettyn‰ omiin Widget olioihin. Tarkastaa mit‰ hahmoa halutaan ruudulle miss‰kin
    /// v‰liss‰.
    /// </summary>
    /// <param name="sanat"> Sis‰lt‰‰ hahmon nimen, jonka t‰ytyy n‰ky‰ ruudulla.</param>
    private void NPCKuvat (string sanat)
    {
        Image Yuki = LoadImage("Yuki kokokuva");
        Image Pepsi = LoadImage("Pepsi kokokuva");

        Widget HahmoYuki = new Widget(1080, 520);
        HahmoYuki.Color = Color.Transparent;
        HahmoYuki.Position = new Vector(Level.Right - 250, 90);
        HahmoYuki.Add(new Widget(Yuki));

        Widget HahmoPepsi = new Widget(1080, 520);
        HahmoPepsi.Color = Color.Transparent;
        HahmoPepsi.Position = new Vector(Level.Left + 300, 30);
        HahmoPepsi.Add(new Widget(Pepsi));

        if (sanat.Contains("YUKI") && sanat.Contains("PEPSI"))
        {
            alusta.Remove(HahmoYuki);
            alusta.Remove(HahmoPepsi);

            alusta.Add(HahmoYuki);
            alusta.Add(HahmoPepsi);
        }

        if (sanat.Contains("YUKI"))
        {
            alusta.Add(HahmoYuki);
            alusta.Remove(HahmoPepsi);
        }

        if (sanat.Contains("PEPSI"))
        {
            alusta.Add(HahmoPepsi);
            alusta.Remove(HahmoYuki);
        }



    }


    /// <summary>
    /// Ainoa tarkoitus on nostaa i:n arvoa, jotta pelin tarina voi edet‰.
    /// </summary>
    private void Klikkaus ()
    {
        i++;
    }


    /// <summary>
    /// Ik‰v‰ kyll‰ pois j‰‰nyt osuus pelist‰. T‰ss‰ oli oma versio pelin interaktiiviseen dialogiin, mutta aikaraja tuli vastaan,
    /// enk‰ saanut t‰t‰ liitetty‰ osaksi peli‰.
    /// 
    /// Tarkoituksena oli tehd‰ looppi, miss‰ pelaaja pystyisi kirjoittamaan sanan ja saamaan siit‰ reaktion.
    /// Keskustelu loppuisi kun pelaaja kirjoittaisi "End".
    /// </summary>
    /*private void NPCDialogi ()

    {

         bool keskustelu = false;

         while (!(keskustelu))
         {
             tekstikentta.Text = "Write: Name,Job,Location or Queen to interact with Yuki. Write End, to stop talking";
             InputWindow kysymysIkkuna = new InputWindow("");
             kysymysIkkuna.TextEntered += Keskustelu;
             Add(kysymysIkkuna);

              void Keskustelu(InputWindow kysymys)
             {
                 string nimi = "Yuki";

                 Dictionary<string, string> puhe = new Dictionary<string, string>();
                 puhe.Add("Name", nimi);
                 puhe.Add("Job", "Im a guard..");
                 puhe.Add("Location", "You don¥t know where you are? Weird.");
                 puhe.Add("Queen", "You will seen soon enough");
                 puhe.Add(default, "Not your business");

                 int tyytyv‰isyys = 40;

                 NPC Yuki = new NPC(nimi, puhe, tyytyv‰isyys);

                 string avain = kysymys.InputBox.Text;

                 tekstikentta.Text = puhe[avain];
                 if (avain == "End") keskustelu = true;
             }


         }

       

}
*/


}



using Jypeli;
using System.Threading.Tasks;
using System.ComponentModel;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;



class NPC
{
    string Nimi;
    Dictionary<string, string> Dialogi;
    int Tyytyväisyys;

    /// <summary>
    /// NPC class, jota en päässyt vielä hyödyntämään. Pitää sisällään hahmon perustiedot.
    /// </summary>
    /// <param name="nimi">Hahmon nimi.</param>
    /// <param name="puhe">Hahmon dialogissa käytettävät vastaukset</param>
    /// <param name="tyytyväisyys">Arvo, joka mittaa hahmon tyytyväisyyttä pelaajaan.</param>
   public NPC (string nimi, Dictionary<string, string> puhe, int tyytyväisyys)
   {
        Nimi = nimi;
        Dialogi = puhe;
        Tyytyväisyys = tyytyväisyys;
   }
    /// <summary>
    /// Kutsutaan kun pelaaja tekee positiivisen vaikutuksen NPC:hen)
    /// </summary>
    public void VaikutePos ()
    {
        if (Tyytyväisyys >= 50) Tyytyväisyys = Tyytyväisyys + 15;
        if (Tyytyväisyys < 50) Tyytyväisyys = Tyytyväisyys + 5;
        if (Tyytyväisyys < 10) Tyytyväisyys = Tyytyväisyys + 1;

    }
    /// <summary>
    /// Kutsutaan kun pelaaja tekee negatiivisen vaikutuksen NPC:hen)
    /// </summary>
    public void VaikuteNeg()
    {
        if (Tyytyväisyys >= 50) Tyytyväisyys = Tyytyväisyys - 15;
        if (Tyytyväisyys < 50) Tyytyväisyys = Tyytyväisyys - 5;
        if (Tyytyväisyys < 10) Tyytyväisyys = Tyytyväisyys - 1;
    }




}

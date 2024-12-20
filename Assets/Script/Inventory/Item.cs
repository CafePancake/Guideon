using UnityEngine;

/// <summary>
/// classe qui sert a stocker des infos sur un item que le perso recupere, contient son nom, sprite pour inventaire ui et le nombre dans l<inventaire
/// </summary>
public class Item
{
    public string itemName; //nom de la ressource
    public Sprite icon; //icone dans l,invenatire
    public int nbHeld; //nombre que le joueur possede
}

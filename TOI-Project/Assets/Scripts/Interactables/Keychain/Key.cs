using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
/// <summary>
/// Classe que comanda a lógica da chave
/// </summary>
public class Key : InteractablesBehaviour
{
    // Som de pegar a chave
    public AudioClip[] getKeySound;


    /// Método chamado pelo script Interactions do player
    public void Interaction()
    {
        Keychain.Instance.AddKey(this);
		int index = Random.Range(0, getKeySound.Length);
        if (getKeySound != null) AudioSource.PlayClipAtPoint(getKeySound[index], transform.position);
    }

}

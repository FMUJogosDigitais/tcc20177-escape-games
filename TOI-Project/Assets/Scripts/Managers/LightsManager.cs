using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{

    public Light[] lights;

	public float[] originalIntensity;

    public float m_intensity;

	public float Intensity
	{
		get { return m_intensity; }
		set
		{
			m_intensity = value;
			SetIntensity();
		}
	}

	#region Singleton
	//Inicio do Singleton
	private static LightsManager instance = null; 
    public static LightsManager Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Essa é uma medida comum para manipular uma classe com uma referência a si mesma.
        // Se a variável da instância existir, mas não for este objeto, destrua este objeto.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Caso contrário, este objeto será asignado para a instância.
            instance = this;
        }//Fim do Singleton
        #endregion
        #region Awake
        // Demais scripts Awake
        #endregion  
    }

    private void Start()
	{
		lights = this.GetComponentsInChildren<Light>();

		InitIntensity();

	}

	private void InitIntensity()
	{
		for (int i = 0; i < lights.Length; i++)
		{
			originalIntensity[i] = lights[i].intensity;
		}
	}

	private void SetIntensity()
    {
        foreach (Light l in lights)
        {
            l.intensity = m_intensity;
        }
    }

	public void RestoreLights()
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].intensity = originalIntensity[i];
		}
	}

	public void TurnOutLights()
	{
		foreach (Light l in lights)
		{
			l.intensity = 0;
		}
	}
}

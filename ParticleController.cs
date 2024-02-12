using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private MeltScript parentMS;
  
    private MeltScript ms;

    [SerializeField] private Material[] materials; // Assign materials in the Inspector
    [SerializeField] private Material sleepMat;
    private ParticleSystem partSys;
    private ParticleSystemRenderer psr;
   //private ParticleSystem.MainModule particleMain;

    // Start is called before the first frame update
    void Start()
    {
        parentMS = gameObject.transform.parent.gameObject.GetComponent<MeltScript>();
        partSys = GetComponent<ParticleSystem>();
        psr = partSys.GetComponent<ParticleSystemRenderer>();
        //particleMain = particleSystem.main;
        //FireParticle();
    }
    

    public void FireParticle()
    {
        int randomIndex = Random.Range(0, materials.Length);
        Material randomMaterial = materials[randomIndex];

        //particleMain.startColor = randomMaterial.color; // Assuming color property exists in the material
        //particleSystem.material = randomMaterial;
        psr.material = randomMaterial;
        GetComponent<ParticleSystem>().Emit(1);
    }
}

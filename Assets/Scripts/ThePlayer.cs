using UnityEngine;

public class ThePlayer : MonoBehaviour
{
    [SerializeField]
    float glycemie;

    [SerializeField]
    float pleasure;

    [SerializeField]
    float energy;

    [Header("Max Values")]
    [SerializeField]
    float glycemieMax;

    [SerializeField]
    float energyMax;

    [SerializeField]
    float pleasureMax;

    [Header("Min Values")]
    [SerializeField]
    float glycemieMin;

    [SerializeField]
    float energyMin;

    [SerializeField]
    float pleasureMin;

    // Getter & Setter

    public void AddGlycemie(float glycemie)
    {
        this.glycemie += glycemie;
        if (this.glycemie > glycemieMax)
        {
            this.glycemie = glycemieMax;
        }
        if (this.glycemie < glycemieMin)
        {
            this.glycemie = glycemieMin;
        }
    }

    public void AddEnergy(float energy)
    {
        this.energy += energy;
        if (this.energy > energyMax)
        {
            this.energy = energyMax;
        }
        if (this.energy < energyMin)
        {
            this.energy = energyMin;
        }
    }

    public void AddPleasure(float pleasure)
    {
        this.pleasure += pleasure;
        if (this.pleasure > pleasureMax)
        {
            this.pleasure = pleasureMax;
        }
        if (this.pleasure < pleasureMin)
        {
            this.pleasure = pleasureMin;
        }
    }

    public float GetGlycemie()
    {
        return glycemie;
    }

    public float GetEnergy()
    {
        return energy;
    }

    public float GetPleasure()
    {
        return pleasure;
    }
}

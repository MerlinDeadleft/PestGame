using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Talis' code
public class ManaController : MonoBehaviour
{

    [SerializeField] ParticleSystem   particles = null;
    [SerializeField] PlayerController player    = null;

    [SerializeField] bool  regeneratingMana = false;
    [SerializeField] float timer            = 0.0f;

    readonly float manaRegInterval = 0.1f;
    readonly int   maxMana         = 100;

    public static ManaController instance = null;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if(regeneratingMana )
        {
            timer += Time.deltaTime;

            if (timer >= manaRegInterval)
            {
                if(player.Mana < maxMana)
                {
                    player.Mana++;
                }
                else if(player.Mana >= maxMana)
                {
                    particles.Stop();
                }

                timer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if("Player" == other.tag)
        {
            if(player.Mana < maxMana)
            {
                particles.Play();
                regeneratingMana = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if("Player" == other.tag)
        {
            particles.Stop();
            regeneratingMana = false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_amount"></param>
    /// Amount of mana to be spent
    /// <returns>Returns false if not enough mana</returns>
    public static bool UseMana(int _cost, PlayerController _player)
    {
        if ((_player.Mana - _cost) >= 0)
        {
            _player.Mana -= _cost;
            return true;
        }
        else
        {
            Debug.Log("Cost exceeds mana!");
            return false;
        }
    }
}
// Talis' code ende
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // next steps: Create an miniMap on HUD
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public GameObject weaponHolder;
    public GameObject player;
    public Image hasCarbine;
    public Image hasPistol;
    public Image hasShotgun;
    public Image hasRifle;
    private PlayerMovement playerMovement;
    private Gun activeGun;

    private int ammo;
    private int currentMag;
    private float health;




    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        hasCarbine.enabled = playerMovement.haveCarbine;
        hasPistol.enabled = playerMovement.havePistol;
        hasShotgun.enabled = playerMovement.haveShotgun;
        hasRifle.enabled = playerMovement.haveRifle;

        health = playerMovement.health;
        if (health >= 16)
        {
            healthText.text = "Health: " + health + "%";
            healthText.color = new Color32(0, 255, 255, 175);
        }

        else if (health < 16)
        {
            healthText.text = "Health: " + health + "%";
            healthText.color = new Color32(255, 0, 0, 175);
        }

        if (weaponHolder != null)
        {
            activeGun = weaponHolder.GetComponentInChildren<Gun>();
        }      
        

        if (activeGun == null)
        {
            ammoText.text = "";
        }

        else if (activeGun != null)
        {
            ammo = activeGun.ammo;

            if (!activeGun.isReloading && ammo > 0)
            {
                ammoText.text = ammo + " - " + currentMag;
                ammoText.color = new Color32(0, 255, 255, 175);
            }
            
            else if (!activeGun.isReloading && ammo == 0 && currentMag > 0)
            {
                ammoText.text = ammo + " - " + currentMag;
                ammoText.color = new Color32(255, 0, 0, 175);
            }

            else if (!activeGun.isReloading && ammo == 0 && currentMag == 0)
            {
                ammoText.text = "Out Of Ammo!";
                ammoText.color = new Color32(255, 0, 0, 175);
            }

            if (activeGun.isReloading)
            {
                ammoText.text = "Reloading...";
                ammoText.color = new Color32(255, 0, 0, 175);
            }



            if (activeGun.gameObject.name == "Pistol")
            {
                currentMag = playerMovement.pistolMag;
                hasCarbine.color = new Color32(0, 0, 0, 110);
                hasPistol.color = new Color32(0, 0, 0, 255);
                hasShotgun.color = new Color32(0, 0, 0, 110);
                hasRifle.color = new Color32(0, 0, 0, 110);

            }

            else if (activeGun.gameObject.name == "M4_Carbine")
            {
                currentMag = playerMovement.carbineMag;
                hasCarbine.color = new Color32(0, 0, 0, 255);
                hasPistol.color = new Color32(0, 0, 0, 110);
                hasShotgun.color = new Color32(0, 0, 0, 110);
                hasRifle.color = new Color32(0, 0, 0, 110);
            }

            else if (activeGun.gameObject.name == "L96_Rifle")
            {
                currentMag = playerMovement.rifleMag;
                hasCarbine.color = new Color32(0, 0, 0, 110);
                hasPistol.color = new Color32(0, 0, 0, 110);
                hasShotgun.color = new Color32(0, 0, 0, 110);
                hasRifle.color = new Color32(0, 0, 0, 255);
            }

            else if (activeGun.gameObject.name == "870_Shotgun")
            {
                currentMag = playerMovement.shotgunMag;
                hasCarbine.color = new Color32(0, 0, 0, 110);
                hasPistol.color = new Color32(0, 0, 0, 110);
                hasShotgun.color = new Color32(0, 0, 0, 255);
                hasRifle.color = new Color32(0, 0, 0, 110);
            }
        }       

        
    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public List<Material> PotionImages = new List<Material>();
    public List<Material> KeyImages = new List<Material>();
    public List<Material> DressImages = new List<Material>();
    public List<Image> InventorySLOTS = new List<Image>();
    public Image POTION;
    public Image KEY;
    public Image DRESS;
    public GameObject Inventory;

    private Player player;

    int nodress, nopotion, nokey;

    Material noimage, noimage2, noimage3;


    private void Awake()
    {
        
        nopotion = PotionImages.Count -1;
        nodress = DressImages.Count -1;
        nokey = KeyImages.Count - 1;

        noimage = DRESS.material;
        noimage2 = POTION.material;
        noimage3 = KEY.material;

        CloseUI();

    }


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        { 
            switch (player.Disguise)
            {
                case RolesManager.eRoles.PAWN:
                    DRESS.material = DressImages[0];
                    break;
                case RolesManager.eRoles.TOWER:
                    DRESS.material = DressImages[1];
                    break;
                case RolesManager.eRoles.HORSE:
                    DRESS.material = DressImages[2];
                    break;
                case RolesManager.eRoles.BISHOP:
                    DRESS.material = DressImages[3];
                    break;
                case RolesManager.eRoles.PLAYER:
                    DRESS.material = DressImages[nodress];
                    break;
                default:
                    DRESS.material = noimage;
                    break;
            }

            //Debug.Log(player.Disguise + "," + InputManager.Instance.IsDroppingDress);
            if (player.Disguise != RolesManager.eRoles.PLAYER) DRESS.gameObject.SetActive(true);
            useCell();

        }
    }

    void useCell()
    {

        if (!player.IsVisible) NoPotion(); // POTION.material = PotionImages[0];
        else if (GameController.Instance.KeyisTaken) { KEY.material = KeyImages[0]; KEY.gameObject.SetActive(true); }
        else if (GameController.Instance.DoorisOpen) NoKey();
        else if (player.Disguise == RolesManager.eRoles.PLAYER) DRESS.gameObject.SetActive(false);
        else { NoPotion(); NoKey(); }
    }

    public void NoDress()
    {
        DRESS.material = DressImages[nodress];
        DRESS.gameObject.SetActive(false);
    }

    public void NoPotion()
    {
        POTION.material = noimage2;
        POTION.gameObject.SetActive(false);
    }

    public void NoKey()
    {
        KEY.material = noimage3;
        KEY.gameObject.SetActive(false);
    }

    void CloseUI()
    {
        DRESS.material = DressImages[nodress];
        POTION.material = PotionImages[nopotion];
        KEY.material = KeyImages[nokey];
        DRESS.gameObject.SetActive(false);
        POTION.gameObject.SetActive(false);
        KEY.gameObject.SetActive(false);
        Inventory.SetActive(false);
    }


}

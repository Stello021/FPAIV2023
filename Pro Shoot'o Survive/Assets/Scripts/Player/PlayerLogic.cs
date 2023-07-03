using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class PlayerLogic : MonoBehaviour
{
    static private PlayerLogic instance;
    static public PlayerLogic Instance { get { return instance; } }

    [SerializeField] float hpMax;
    [HideInInspector] public float actuallyMaxHp;

    [SerializeField] private float hp;
    public float HP { get { return hp; } set { hp = Mathf.Clamp(value, 0, hpMax); } }
    [SerializeField] private float armor;
    [SerializeField] private float armorMax;
    public float Armor { get { return armor; } set { armor = Mathf.Clamp(value, 0, armorMax); UpdateArmorText(); } }


    public float ReloadMultiplier = 1f;

    public float speedMultiplier = 1f;
    public float speed;

    public float speedBarValue;     //value from 0 to 1
    public float healthBarValue;    //value from 0 to 1
    public float ReloadBarValue;    //value from 0 to 1

    [SerializeField] TMP_Text armorValueText;

    private int points;
    private int deathPoints = 500;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        actuallyMaxHp = hpMax;
        hp = actuallyMaxHp;
        BarsManager.Instance.playerRef = this;
    }

    // Update is called once per frame
    void Update()
    {
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
        UpdateStatsMultiplier();
    }

    public void TakeDamage(float damage)
    {
        float excessDamage = Armor - damage;

        if (Armor > 0)
        {
            if (excessDamage < 0)
            {
                Armor = 0;
                excessDamage = Mathf.Abs(excessDamage);

                //Debug.Log("Excess damage: " + excessDamage);

                HP -= excessDamage;
            }
            else
            {
                Armor -= damage;
            }
        }
        else
        {
            HP -= damage;
        }

        //Debug.Log("armor: " + Armor);
        //Debug.Log("Hp: " + HP);

        if (HP <= 0)
        {
            //Destroy(gameObject);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(2);
        }

    }

    public void Heal(float HealAmount)
    {
        hp = Mathf.Clamp(hp + healthBarValue, 0, actuallyMaxHp);
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
    }

    public void UpdateArmorText()
    {
        armorValueText.text = armor.ToString();
    }
    public void UpdateStatsMultiplier()
    {
        //multipliers affects player' speed or damage taken
        if (ReloadBarValue <= 0 || ReloadBarValue >= 1)
        {
            ReloadMultiplier = 2f;

            ChangeReloadTime();
        }
        else if (ReloadBarValue >= 0.6f && ReloadBarValue <= 0.7f)
        {
            ReloadMultiplier = 0f;

            ChangeReloadTime();
        }
        else
        {
            ReloadMultiplier = 1f;

            ChangeReloadTime();
        }

        if (speedBarValue <= 0 || speedBarValue >= 1)
        {
            speedMultiplier = 0.5f;
        }
        else if (speedBarValue >= 0.6f && speedBarValue <= 0.7f)
        {
            speedMultiplier = 2f;
        }
        else
        {
            speedMultiplier = 1f;
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
    }

    private void SavePoints()
    {
        points -= (int)Time.timeSinceLevelLoad * 2;
        points += (int)(HP + Armor) * 5;

        if (HP <= 0)
        {
            points -= deathPoints;
        }

        //Debug.Log("current points: " + points);


        if (PlayerPrefs.HasKey("Record"))
        {
            //Debug.Log("C'è già un record");
            int record = PlayerPrefs.GetInt("Record");
            //Debug.Log("record points: " + record);

            if (points > record)
            {
                //Debug.Log("Aggiorno il record");
                PlayerPrefs.SetInt("Record", points);
            }
        }
        else
        {
            //Debug.Log("Non c'è un record, mi segno un nuovo record");
            PlayerPrefs.SetInt("Record", points);
        }

        //Debug.Log("Assegno i punti della run attuale");
        PlayerPrefs.SetInt("Current", points);
    }

    private void OnDestroy()
    {
        SavePoints();
    }

    private void ChangeReloadTime()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        WeaponLogic weaponLogic = playerController.currentWeapon.GetComponent<WeaponLogic>();
        ReloadUI reloadUI = weaponLogic.reloadUI.GetComponent<ReloadUI>();

        weaponLogic.reloadTime = weaponLogic.currentReloadTime * ReloadMultiplier;
        reloadUI.reloadTime = weaponLogic.reloadTime;
    }
}

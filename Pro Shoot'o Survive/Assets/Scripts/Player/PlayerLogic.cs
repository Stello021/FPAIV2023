using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int deathPoints = 5000;
    private int barPoints = 3000;
    private int pointsDecreaseRate = 2;
    private int pointsHPBonus = 10;
    [SerializeField] TMP_Text pointsText;

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

        InvokeRepeating("UpdatePointsText", 1.0f, 1.0f);
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
        }
        else if (ReloadBarValue >= 0.61f && ReloadBarValue <= 0.69f)
        {
            ReloadMultiplier = 0f;
        }
        else
        {
            ReloadMultiplier = 1f;
        }
        ChangeReloadTime();

        if (speedBarValue <= 0 || speedBarValue >= 1)
        {
            speedMultiplier = 0.5f;
        }
        else if (speedBarValue >= 0.61f && speedBarValue <= 0.69f)
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

    public void UpdatePointsText()
    {
        points -= pointsDecreaseRate;
        pointsText.text = points.ToString();
    }

    private void SavePoints()
    {
        points += (int)(HP + Armor) * pointsHPBonus;

        if (ReloadBarValue >= 0.6f && ReloadBarValue <= 0.7f)
        {
            points += barPoints;
        }

        if (speedBarValue >= 0.6f && speedBarValue <= 0.7f)
        {
            points += barPoints;
        }

        if (HP <= 0)
        {
            points -= deathPoints;
        }

        

        if (PlayerPrefs.HasKey("Record"))
        {
            int record = PlayerPrefs.GetInt("Record");

            if (points > record)
            {
                PlayerPrefs.SetInt("Record", points);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Record", points);
        }

        PlayerPrefs.SetInt("Current", points);
    }

    private void OnDestroy()
    {
        SavePoints();
    }

    private void ChangeReloadTime()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        WeaponLogic weaponLogic = playerController.CurrentWeapon.GetComponent<WeaponLogic>();
        ReloadUI reloadUI = weaponLogic.ReloadUI.GetComponent<ReloadUI>();

        weaponLogic.reloadTime = weaponLogic.currentReloadTime * ReloadMultiplier;
        reloadUI.reloadTime = weaponLogic.reloadTime;
    }
}

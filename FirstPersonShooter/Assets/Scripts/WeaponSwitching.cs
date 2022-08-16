using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class WeaponSwitching : MonoBehaviour
{
    InputAction switching;

    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfoText;

    void Start() // enabling weapon switch
    {
        switching = new InputAction("Scroll", binding: "<Mouse>/scroll");
        switching.Enable();

        SelectWeapon();
    }

    void Update()
    {
        Gun currentGun = FindObjectOfType<Gun>(); // referencing ammo in UI
        ammoInfoText.text = currentGun.currentAmmo + " / " + currentGun.magazineSize;
        WeaponSwitch();
    }

    private void SelectWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        transform.GetChild(selectedWeapon).gameObject.SetActive(true);    
    }

    private void WeaponSwitch()
    {
        float scrollValue = switching.ReadValue<Vector2>().y;
        int previousSelected = selectedWeapon;

        if (scrollValue > 0)
        {
            selectedWeapon++;
            if (selectedWeapon == 3)
                selectedWeapon = 0;
        }
        else if (scrollValue < 0)
        {
            selectedWeapon--;
            if (selectedWeapon == -1)
                selectedWeapon = 2;
        }
        if (previousSelected != selectedWeapon)
            SelectWeapon();
    }
}

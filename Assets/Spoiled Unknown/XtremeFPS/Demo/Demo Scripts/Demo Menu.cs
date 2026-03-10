using System;
using TMPro;
using UnityEngine;
using XtremeFPS.FPSController;
using XtremeFPS.WeaponSystem;

namespace XtremeFPS.Demo
{
    public class DemoMenu : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI bulletText;
        [SerializeField] private FirstPersonController personController;
        [SerializeField] private UniversalWeaponSystem weaponSystem;

        private void Update()
        {

            CalculateBulletShellsAndSetTheText();
        }

        private void CalculateBulletShellsAndSetTheText()
        {
            if (bulletText == null || weaponSystem == null || !weaponSystem.enabled) return;
            bulletText.text = $"{weaponSystem.BulletsLeft / weaponSystem.bulletsPerTap} / {weaponSystem.totalBullets / weaponSystem.bulletsPerTap}";
        }
    }
}

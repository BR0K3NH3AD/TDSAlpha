using UnityEngine;

namespace TDS.Scripts.LastMegaBoss
{
    public class MegaBossHealthBarActivator : MonoBehaviour
    {
        [SerializeField] private GameObject megaBossHealthBar;

        private void OnEnable()
        {
            MegaBoss.OnMegaBossSpawned += ActivateHealthBar;
        }

        private void OnDisable()
        {
            MegaBoss.OnMegaBossSpawned -= ActivateHealthBar;
        }

        private void ActivateHealthBar()
        {
            megaBossHealthBar.SetActive(true);
        }
    }

}


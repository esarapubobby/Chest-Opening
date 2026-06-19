using System.Collections.Generic;
using UnityEngine;

public class SharkHealth : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] private List<Color> _damageColors = new List<Color>();
    SkinnedMeshRenderer skinnedMR;

    private void Start()
    {
        Transform child = transform.Find("SHARK");
        Transform subChild = child.transform.Find("mesh_0.001");

        skinnedMR = subChild.GetComponent<SkinnedMeshRenderer>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        skinnedMR.material.color = _damageColors[Random.Range(0, _damageColors.Count-1)];
        if (health <= 0)
        {
            AudioManager.Instance.PlaySharkDamage();
            Die();
        }
    }


    void Die()
    {
        skinnedMR.material.color = Color.white;
        gameObject.SetActive(false);
    }
    
}

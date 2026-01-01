using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour 
{
    [System.Serializable] //TANMLAMALAR
    public class LootGroup
    {
        public string nadirlikAdi;
        public GameObject[] prefablari;
        public int spawnAdedi;
    }

    [Header("Eþya Gruplarý")]
    public List<LootGroup> lootGruplari;

    [Header("Spawn Alaný Ayarlarý")]
    public Vector2 spawnAlaniMin;
    public Vector2 spawnAlaniMax;

    [Header("Çakýþma Kontrolü")]
    public float esyaYaricapi = 0.5f; // Eþyalar arasý minimum mesafe
    public int maksimumDeneme = 10;   // Uygun yer bulamazsa kaç kez tekrar denesin?
    public LayerMask lootLayer;       // Sadece lootlarý kontrol etmek için bir Layer

    [Header("Hiyerarþi Ayarý")]
    public Transform lootParent;

    void Start()
    {
        LootlariOlustur();
    }

    public void LootlariOlustur() //LOOTLARI OLUÞTURMAK ÝÇÝN GEREKEN FONKSÝYON
    {
        foreach (LootGroup grup in lootGruplari)
        {
            for (int i = 0; i < grup.spawnAdedi; i++)
            {
                Vector2 spawnPozisyonu = UygunPozisyonBul();

                // Eðer uygun pozisyon bulunduysa (Vector2.zero dönmediyse) oluþtur
                if (spawnPozisyonu != Vector2.zero)
                {
                    GameObject secilenPrefab = grup.prefablari[Random.Range(0, grup.prefablari.Length)];
                    GameObject yeniLoot = Instantiate(secilenPrefab, spawnPozisyonu, Quaternion.identity);

                    if (lootParent != null)
                        yeniLoot.transform.SetParent(lootParent);
                }
            }
        }
    }

    Vector2 UygunPozisyonBul() //LOOTLAR OLUÞURKEN ÇALIÞMASI GEREKEN FONKSÝYON
    {
        for (int deneme = 0; deneme < maksimumDeneme; deneme++)
        {
            Vector2 rastgelePos = new Vector2(
                Random.Range(spawnAlaniMin.x, spawnAlaniMax.x),
                Random.Range(spawnAlaniMin.y, spawnAlaniMax.y)
            );

            // Belirlenen noktada 'esyaYaricapi' kadar alanda baþka collider var mý bak
            Collider2D hit = Physics2D.OverlapCircle(rastgelePos, esyaYaricapi, lootLayer);

            if (hit == null) // Eðer alan boþsa
            {
                return rastgelePos;
            }
        }

        Debug.LogWarning("Uygun yer bulunamadý, bu eþya atlandý.");
        return Vector2.zero; // Uygun yer bulunamadý
    }
}
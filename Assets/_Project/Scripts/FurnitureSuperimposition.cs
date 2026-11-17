using UnityEngine;

public class FurnitureSuperimposition : MonoBehaviour
{
    [Header("Model 3D Furnitur Baru")]
    public GameObject newFurnitureKursiKuning;
    public GameObject newFurnitureBarStool;

    // Panggil saat target Kursi Kuning ditemukan Vuforia
    public void TampilkanKursiKuning()
    {
        if (newFurnitureKursiKuning != null) newFurnitureKursiKuning.SetActive(true);
        if (newFurnitureBarStool != null) newFurnitureBarStool.SetActive(false);
    }

    // Panggil saat target Bar Stool ditemukan Vuforia
    public void TampilkanBarStool()
    {
        if (newFurnitureKursiKuning != null) newFurnitureKursiKuning.SetActive(false);
        if (newFurnitureBarStool != null) newFurnitureBarStool.SetActive(true);
    }

    // Panggil saat target hilang dari kamera
    public void SembunyikanSemuaFurnitur()
    {
        if (newFurnitureKursiKuning != null) newFurnitureKursiKuning.SetActive(false);
        if (newFurnitureBarStool != null) newFurnitureBarStool.SetActive(false);
    }
}
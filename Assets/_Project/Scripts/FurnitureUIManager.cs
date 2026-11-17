
using System.Collections;
using UnityEngine;
using TMPro;
using Vuforia;

public class FurnitureUIManager : MonoBehaviour
{
    [Header("3 Panel Utama")]
    public GameObject welcomPanel;
    public GameObject panelKatalog;
    public GameObject panelMenuAR;

    [Header("Teks Arahan Scanning")]
    public TextMeshProUGUI textScanningHint;

    [Header("Vuforia Model Targets")]
    public GameObject modelTargetArmchair;
    public GameObject modelTargetBarStool;
    public GameObject modelTargetDiningChair;
    public GameObject modelTargetMacaroons;

    private string furniturAktif = "";

    void Start()
    {
        // Pengaturan panel awal
        if (welcomPanel != null)
            welcomPanel.SetActive(true);

        if (panelKatalog != null)
            panelKatalog.SetActive(false);

        if (panelMenuAR != null)
            panelMenuAR.SetActive(false);

        // Nonaktifkan seluruh observer saat aplikasi dimulai
        MatikanSemuaTarget();

        Debug.Log("Furniture System siap.");
    }

    // =========================================
    // 1. MENGAKTIFKAN / MENONAKTIFKAN MODEL TARGET
    // =========================================

    private void SetTargetAktif(GameObject targetObj, bool aktif)
    {
        if (targetObj == null)
        {
            Debug.LogWarning("Model Target belum di-assign!");
            return;
        }

        ModelTargetBehaviour mtb =
            targetObj.GetComponent<ModelTargetBehaviour>();

        if (mtb == null)
        {
            Debug.LogError(
                "ModelTargetBehaviour tidak ditemukan pada " +
                targetObj.name
            );
            return;
        }

        // Hanya mengubah status observer.
        // Jangan mengubah GuideViewMode di sini.
        mtb.enabled = aktif;

        Debug.Log(
            targetObj.name +
            (aktif ? " BERHASIL DIAKTIFKAN" : " DINONAKTIFKAN")
        );
    }

    private void MatikanSemuaTarget()
    {
        SetTargetAktif(modelTargetArmchair, false);
        SetTargetAktif(modelTargetBarStool, false);
        SetTargetAktif(modelTargetDiningChair, false);
        SetTargetAktif(modelTargetMacaroons, false);
    }

    // =========================================
    // 2. TOMBOL MULAI SEKARANG
    // =========================================

    public void MulaiSekarang()
    {
        MatikanSemuaTarget();

        if (welcomPanel != null)
            welcomPanel.SetActive(false);

        if (panelMenuAR != null)
            panelMenuAR.SetActive(false);

        if (panelKatalog != null)
            panelKatalog.SetActive(true);

        Debug.Log("Masuk ke katalog furnitur.");
    }

    // =========================================
    // 3. PILIHAN KATALOG
    // =========================================

    public void PilihArmchair()
    {
        BukaKameraScan("Armchair", modelTargetArmchair);
    }

    public void PilihBarStool()
    {
        BukaKameraScan("Bar Stool", modelTargetBarStool);
    }

    public void PilihDiningChair()
    {
        BukaKameraScan("Dining Chair", modelTargetDiningChair);
    }

    public void PilihMacaroons()
    {
        BukaKameraScan("Macaroons", modelTargetMacaroons);
    }

    // =========================================
    // 4. PROSES PERGANTIAN MODEL TARGET
    // =========================================

    private void BukaKameraScan(
        string namaKursi,
        GameObject targetDipilih)
    {
        if (targetDipilih == null)
        {
            Debug.LogError(
                "Target " + namaKursi +
                " belum dihubungkan di Inspector!"
            );
            return;
        }

        ModelTargetBehaviour mtb =
            targetDipilih.GetComponent<ModelTargetBehaviour>();

        if (mtb == null)
        {
            Debug.LogError(
                "ModelTargetBehaviour tidak ditemukan pada " +
                targetDipilih.name
            );
            return;
        }

        furniturAktif = namaKursi;

        // Pindah panel
        if (panelKatalog != null)
            panelKatalog.SetActive(false);

        if (panelMenuAR != null)
            panelMenuAR.SetActive(true);

        // Matikan target sebelumnya
        MatikanSemuaTarget();

        // Aktifkan target yang dipilih setelah satu frame
        StartCoroutine(AktifkanTargetBerikutnya(targetDipilih));

        if (textScanningHint != null)
        {
            textScanningHint.text =
                "Arahkan kamera dan sejajarkan siluet\n" +
                "dengan " + namaKursi + " lama...";
        }

        Debug.Log("Memilih furnitur: " + namaKursi);
    }

    
private IEnumerator AktifkanTargetBerikutnya(
    GameObject targetDipilih)
{
    yield return new WaitForSeconds(0.5f);

    SetTargetAktif(targetDipilih, true);

    yield return null;

    ModelTargetBehaviour mtb =
        targetDipilih.GetComponent<ModelTargetBehaviour>();

    if (mtb != null)
    {
        int jumlahGuideView = mtb.GetNumGuideViews();

        Debug.Log(
            "[GUIDE VIEW TEST] Target: " + targetDipilih.name +
            " | Jumlah Guide View: " + jumlahGuideView +
            " | Mode: " + mtb.GuideViewMode +
            " | Index Aktif: " + mtb.GetActiveGuideViewIndex()
        );
    }
}


    // =========================================
    // 5. KEMBALI KE KATALOG
    // =========================================

    public void KembaliKeKatalog()
    {
        MatikanSemuaTarget();

        if (panelMenuAR != null)
            panelMenuAR.SetActive(false);

        if (panelKatalog != null)
            panelKatalog.SetActive(true);

        furniturAktif = "";

        Debug.Log("Kembali ke katalog.");
    }

    // =========================================
    // 6. EVENT DARI VUFORIA
    // =========================================

    public void OnScanBerhasil()
    {
        if (textScanningHint != null)
        {
            textScanningHint.text =
                "Furnitur berhasil di-scan!\n" +
                furniturAktif + " baru telah terpasang.";
        }

        Debug.Log("Scan berhasil: " + furniturAktif);
    }

    public void OnScanTerlepas()
    {
        if (textScanningHint != null)
        {
            textScanningHint.text =
                "Target terlepas. Arahkan kembali kamera\n" +
                "ke " + furniturAktif + " lama.";
        }

        Debug.Log("Target terlepas: " + furniturAktif);
    }
}

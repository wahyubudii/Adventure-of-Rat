using UnityEngine.UI;

/// <summary>
/// Saat dapat item, saat mencoba mendapatkan item, saat inventaris penuh, saat memperoleh exp, saat menyelesaikan quest
/// </summary>

public class NotificationManager : Singleton<NotificationManager> {
    private int count = 0;
    private int Count { get { return count; } set { if(value >= NotificationTexts.Length) count = 0; else count++; } }

    public bool ShowingMessage = false;

    public Text[] NotificationTexts;

    //text notifikasi
    private void Generate(string message) {
        Text textObj = NotificationTexts[count];

        if(textObj.gameObject.activeSelf)
            textObj.gameObject.SetActive(false);
        textObj.gameObject.SetActive(true);
        textObj.transform.SetAsLastSibling();  //notifikasi yang baru dibuat diletakkan di bawah.

        textObj.text = message;
        Count++;
    }

    public void Generate_GetItem(string ItemName, int count) {
        string str = "Item yang didapat (" + ItemName + " +" + count + "anjing)";
        Generate(str);
    }

    public void Generate_InventoryIsFull() {
        string str = "Inventaris Anda penuh. Harap kosongkan inventaris Anda.";
        Generate(str);
    }

    public void Generate_GetExp(float Exp) {
        string str = "Dapat Exp. (+" + Exp.ToString() + ")";
        Generate(str);
    }

    public void Generate_CompletableQuest() {
        string str = "Misi berhasil, kembali ke NPC.";
        Generate(str);
    }
}
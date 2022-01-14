using System.Collections;
using UnityEngine;

public class EventCamera : MonoBehaviour {
    public GameObject[] ExclamationMarks;
    private GameObject BossDialogUI;
    public Animator blackScreen;

    private TypeEffect DialogText;

    private Vector3 BossPos = new Vector3(-30.9f, 28.8f, 233f);
    private Vector3 PrincessPos = new Vector3(-49.1f, 26f, 262.3f);

    private bool NextScene = false;

    private void Awake() {
        DialogText = UIManager.Instance.dialogText.GetComponent<TypeEffect>();
        BossDialogUI = UIManager.Instance.BossDialogUI;
    }

    public void StartAnimation() {
        UIManager.Instance.OnOffCanvas(false, false, true);
        UIManager.Instance.joystick.PointerUp();
        blackScreen.SetTrigger("animationStart");

        //menentukann letak awal kamera sebagai kamera utama
        transform.position = GameManager.Instance.Cam.transform.position;

        //menmatikan kamera utama
        GameManager.Instance.Cam.gameObject.SetActive(false);

        StartCoroutine(CameraStop());
    }

    private IEnumerator CameraStop() {
        yield return new WaitForSeconds(3.5f);

        ExclamationMarks[0].SetActive(true);
        ExclamationMarks[1].SetActive(true);
        ExclamationMarks[2].SetActive(true);

        yield return new WaitForSeconds(2f);
        StartCoroutine(MoveToBoss());
    }

    private IEnumerator MoveToBoss() {
        while(transform.position != BossPos) {
            transform.position = Vector3.MoveTowards(transform.position, BossPos, 22f * Time.deltaTime);
            yield return null;
        }
        ExclamationMarks[0].SetActive(false);
        ExclamationMarks[1].SetActive(false);
        ExclamationMarks[2].SetActive(false);

        //Mengaktifkan Kanvas
        BossDialogUI.SetActive(true);
        DialogText.SetMsg("Hei anak kecil, apakah kamu kesini menantangku? aku akan memakanmu sekarang juga...!!");

        //Tunggu hingga user mengklik kotak dialog
        yield return new WaitUntil(() => DialogText.EndCursor.activeSelf && NextScene);

        BossDialogUI.SetActive(false);
        StartCoroutine(MoveToPrincess());
    }

    private IEnumerator MoveToPrincess() {
        NextScene = false;

        while(transform.position != PrincessPos) {
            transform.position = Vector3.MoveTowards(transform.position, PrincessPos, 15f * Time.deltaTime);
            yield return null;
        }
        BossDialogUI.SetActive(true);
        DialogText.SetMsg("Tolong selamatkan aku..!!!hukk hukk (batuk)..");

        yield return new WaitUntil(() => DialogText.EndCursor.activeSelf && NextScene);
        BossDialogUI.SetActive(false);
        StartCoroutine(MoveToPlayer());
    }

    private IEnumerator MoveToPlayer() {
        Vector3 playerPos = GameManager.Instance.Cam.transform.position;
        while(transform.position != playerPos) {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 30f * Time.deltaTime);
            yield return null;
        }

        UIManager.Instance.OnOffCanvas(true, true, true);
        UIManager.Instance.AnimationScreen.SetActive(false);
        UIManager.Instance.BossUI.SetActive(true);
        BossDialogUI.SetActive(false);

        //Aktifkan kamera utama untuk mengubah sudut pandang ke kamera utama
        GameManager.Instance.Cam.gameObject.SetActive(true);
        gameObject.SetActive(false);

        BossQuest.Instance.OnAnimation = false;
        BossQuest.Instance.OnFighting = true;
    }

    public void OnNextDialogBtn() {
        NextScene = true;
    }

    public void OffNextDialogBtn() {
        NextScene = false;
    }
}
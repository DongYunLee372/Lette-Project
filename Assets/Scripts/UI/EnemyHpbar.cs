using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHpbar : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera uiCamera; //UI 카메라를 담을 변수
    public Camera main;
    [SerializeField]
    private Canvas canvas; //캔버스를 담을 변수
    private RectTransform rectParent; //부모의 rectTransform 변수를 저장할 변수
    private RectTransform rectHp; //자신의 rectTransform 저장할 변수
    public Canvas enemyHpBarCanvas;
    //HideInInspector는 해당 변수 숨기기, 굳이 보여줄 필요가 없을 때 
    public Vector3 offset = Vector3.zero; //HpBar 위치 조절용, offset은 어디에 HpBar를 위치 출력할지
    public Transform enemyTr; //적 캐릭터의 위치
    
    public Image myhp;

    public float Curhp;
    public float Maxhp;

    public Vector3 hpBarOffset = new Vector3(-0.5f, 3f, 0);
    public EnemyHpbar MyHpbar;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();


        main = PlayableCharacter.Instance.GetCamera();



    }

    private void LateUpdate()
    {
        main = CameraManager.Instance.Playercamera.GetComponent<Camera>();
        var screenPos = main.WorldToScreenPoint(enemyTr.position + offset); // 몬스터의 월드 3d좌표를 스크린좌표로 변환
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환
        
        Debug.Log(main.name);
        rectHp.localPosition = localPos; // 체력바 위치조정
    }
    public void hit()
    {
        myhp.GetComponent<Image>().fillAmount =Curhp/Maxhp; 
       // myhp.value = (float)Curhp / (float)Maxhp;
    }

    public EnemyHpbar SetHpBar(float HP, Transform trans)
    {
        // // enemyHpBarCanvas = enemyHpBarCanvas.GetComponent<Canvas>();
        // GameObject hpBar = UIManager.Instance.Prefabsload("Hpbar", UIManager.CANVAS_NUM.ex_skill);
        // // myhp = hpBar;
        // myhp = hpBar.GetComponent<Image>();
        // var _hpbar = hpBar.GetComponent<EnemyHpbar>();
        // // hpBar.transform.SetParent(enemyHpBarCanvas.transform);
        // _hpbar.enemyTr = trans;
        // _hpbar.offset = hpBarOffset;
        // _hpbar.Maxhp = HP;
        // _hpbar.Curhp = HP;
        // var _test = hpBar.GetComponent<Image>();
        //// _hpbar.myhp = _test;
        // MyHpbar = _hpbar;

        GameObject hpBar = UIManager.Instance.Prefabsload("Hpbar", UIManager.CANVAS_NUM.ex_skill);

        var _hpbar = hpBar.GetComponent<EnemyHpbar>();
        //  hpBar.transform.SetParent(enemyHpBarCanvas.transform);
        _hpbar.enemyTr = trans;
        _hpbar.offset = hpBarOffset;
        _hpbar.Maxhp = HP;
        _hpbar.Curhp = HP;
        var _test = hpBar.GetComponent<Image>();
        _hpbar.myhp = _test;
        MyHpbar = _hpbar;
       // Debug.Log(this.gameObject.name);
        return MyHpbar;
    }
}

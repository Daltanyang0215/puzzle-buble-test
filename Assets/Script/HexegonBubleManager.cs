using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum Type
{
    Fire,
    Water,
    Eath,
    Heal
}
public class HexegonBubleManager : MonoBehaviour
{
    #region Singleton
    public static HexegonBubleManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion
    #region 변수 모음
    public GameObject block; // 블록
    public Transform blockstranstorm; // 블록 집합
    public List<Color> blockcolor; // 블록 색 ( 나중에는 이미지로 설정 해야 할듯

    public GameObject firespwan; // 발사 위치
    private GameObject firespwan_arrow; // 발사 화살표 오브젝트
    private Vector2 fireblockposition; // 발사 위치 포지션

    public SpriteRenderer nextBlock; // 다음 블록 이미지

    public Transform effect_transform; // 공격이펙트 모임
    public Transform orb_transform; // 블록 파괴 오브 모임
    public GameObject orb; // 블록 파괴 오브

    public Text count_text; // 턴수 텍스트
    int swpancount = 0; // 턴수 카운트

    private List<GameObject> destroyblocks = new List<GameObject>(); // 삭제간 임시 저장 리스트
    private List<GameObject> flydestroyblocks = new List<GameObject>();// 삭제간 임시 저장 리스트 2

    const float offset = 0.866f; // sin(60) 상수

    public Slider Hp;// 체력 실린더
    public Text Hp_text;
    public Slider Recovery; // 회복 실린더
    public int now_Hp = 0; // 현재체력
    public int now_Recovery = 0; // 회복 게이지
    public int total_Hp = 0; // 전체 체력
    public int total_Recovery = 0; // 전체 회복력


    public UnitPanel[] panels; // 유닛 패널들
    public Transform panelholder;// 유닛 패널 부모
    public List<Unit> units = new List<Unit>(); // 유닛 리스트


    public EnemyPanel enemy; // 적 패널
    public List<Enemy> enemies = new List<Enemy>(); // 적리스트

    [HideInInspector]
    public bool offsetspwan = true; // 교차 생성 불
    int bublecount = 1; // 생성 횟수
    
    [HideInInspector]
    public int nextblockindex; // 다음 블록 타입
    [HideInInspector]
    public int saveindex; // 다음 블록 타입 저장
    [HideInInspector]
    public Vector2 savevector; // 드래그 간 백터

    private WaitForFixedUpdate wt = new WaitForFixedUpdate();
    private WaitForSeconds waittime = new WaitForSeconds(0.5f);
    public bool fireending = false;
    #endregion


    private void Start()
    {
        fireblockposition = firespwan.transform.position;
        firespwan_arrow = firespwan.transform.GetChild(0).gameObject;
        SettingBlock();
        panels = panelholder.GetComponentsInChildren<UnitPanel>();
        #region 유닛,맵 등록
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].unit != null)
            {
                panels[i].unit = SendData.instance.units[i];
                panels[i].UpdateSlotUi();
            }
        }
        for (int i = 0; i < MapDataBase.instance.MapDB[SendData.instance.maplevel].Enemy_index.Count; i++)
        {
            enemies.Add(EnemyDataBase.instance.EnemyDB[MapDataBase.instance.MapDB[SendData.instance.maplevel].Enemy_index[i]]);
        }
        if (enemies[0] != null)
        {
            enemy.enemy = enemies[0];
        }
        #endregion

        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].unit != null)
            {
                total_Hp += panels[i].unit.unit_HP;
                total_Recovery += panels[i].unit.unit_Recovery;
            }
        }
        Hp.maxValue = total_Hp;
        Hp.value = total_Hp;
        now_Hp = total_Hp;
        Hp_text.text = now_Hp.ToString() + " / " + total_Hp.ToString();
        StartCoroutine(SlideUpdata());
    }

    private void Update()
    {
        if (!fireending && Time.deltaTime!=0)
        {
            Vector2 mos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                savevector = mos;
                if ((mos.x > -1) && (mos.x < 11) && (mos.y > -10) && (mos.y < 1))
                {
                    firespwan_arrow.SetActive(true);
                    firespwan_arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(GetAngle(firespwan_arrow.transform.position, mos), 5, 175));
                    firespwan_arrow.GetComponent<LineRenderer>().SetPosition(0, mos);
                    firespwan_arrow.GetComponent<LineRenderer>().SetPosition(1, mos);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (Vector2.Distance(savevector, mos) > 0.1f)
                {
                    float engle = GetAngle(savevector, mos);
                    if (engle < -90) engle += 360;
                    firespwan_arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(engle, 5, 175));
                    firespwan_arrow.GetComponent<LineRenderer>().SetPosition(1, mos);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                firespwan_arrow.SetActive(false);
            }
        }
    }
    public void SettingBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j > -5; j--)
            {
                GameObject _block;
                if (Mathf.Abs(j) % 2 == 1)
                {
                    _block = Instantiate(block, new Vector2(i + 0.5f, j * offset), Quaternion.identity, blockstranstorm);
                }
                else
                {
                    _block = Instantiate(block, new Vector2(i, j * offset), Quaternion.identity, blockstranstorm);
                }
                //_block.GetComponent<SpriteRenderer>().color = blockcolor[Random.Range(0, blockcolor.Count)];
                _block.GetComponent<Block1>().state = State.End;
                _block.GetComponent<CircleCollider2D>().radius = 0.48f;
            }
        }
        Instantiate(block, fireblockposition, Quaternion.identity, blockstranstorm);

    }

    

    public IEnumerator SlideUpdata()
    {
        yield return new WaitForSeconds(0.1f);
        
        while (true)
        {

            if (Hp.value != now_Hp)
            {
                Hp.value = Mathf.Clamp(now_Hp, Hp.value - 2, Hp.value + 2);
            }

            if (now_Recovery > Recovery.value)
            {
                Recovery.value++;
                if (Recovery.value >= Recovery.maxValue)
                {
                    now_Recovery -= (int)Recovery.maxValue;
                    Recovery.value = 0;
                    now_Hp += total_Recovery;
                    now_Hp = Mathf.Clamp(now_Hp, 0, total_Hp);
                }
            }
            Hp_text.text = now_Hp.ToString() + " / " + total_Hp.ToString();

            yield return wt;
        }
    }

    public void Fire_End_Coroutine(Vector2 pos)
    {
        StartCoroutine(FireBlockEnd(pos));
    }
    public IEnumerator FireBlockEnd(Vector2 pos)
    {
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.zero);
        if (ray.collider != null)
        {
            Scanbox(ray.collider);
            if (destroyblocks.Count > 2)
            {
                for (int i = 0; i < destroyblocks.Count; i++)
                {
                    if (destroyblocks[i].GetComponent<Block1>().type == Type.Heal)
                    {
                        GameObject spwnorb = Instantiate(orb, destroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                        spwnorb.GetComponent<SpriteRenderer>().color = blockcolor[(int)destroyblocks[i].GetComponent<Block1>().type];
                        spwnorb.GetComponent<Orb>().target = Recovery.gameObject;
                    }
                    else
                    {
                        for (int j = 0; j < panels.Length; j++)
                        {
                            if (panels[j].unit.unit_Type == destroyblocks[i].GetComponent<Block1>().type)
                            {
                                GameObject spwnorb = Instantiate(orb, destroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                                spwnorb.GetComponent<SpriteRenderer>().color = blockcolor[(int)destroyblocks[i].GetComponent<Block1>().type];
                                spwnorb.GetComponent<Orb>().target = panels[j].gameObject;
                            }
                        }
                    }
                    Destroy(destroyblocks[i]);
                }
            }
        }

        for (int i = 0; i < blockstranstorm.childCount; i++)
        {
            flydestroyblocks.Add(blockstranstorm.GetChild(i).gameObject);
        }//블록 리스트 업
        for (int i = 0; i < 10; i++)
        {
            ray = Physics2D.Raycast(new Vector2(i + 0.25f, 0), Vector2.zero);
            if (ray.collider != null)
            {
                Flybox(ray.collider);
            }
        }//공중 블록 검색
        for (int i = 0; i < destroyblocks.Count; i++)
        {
            if (flydestroyblocks.Contains(destroyblocks[i]))
            {
                flydestroyblocks.Remove(destroyblocks[i]);
            }
        }// 중복 삭제 .destroyblocks이 시컨스상 아직 남아 있어서 중복처리를 막아야 횟수가 정확히 나옴
        for (int i = 0; i < flydestroyblocks.Count; i++)
        {
            if (flydestroyblocks[i].GetComponent<Block1>().type == Type.Heal)
            {
                GameObject spwnorb = Instantiate(orb, flydestroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                spwnorb.GetComponent<SpriteRenderer>().color = blockcolor[(int)flydestroyblocks[i].GetComponent<Block1>().type];
                spwnorb.GetComponent<Orb>().target = Recovery.gameObject;
                GameObject spwnorb1 = Instantiate(orb, flydestroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                spwnorb1.GetComponent<SpriteRenderer>().color = blockcolor[(int)flydestroyblocks[i].GetComponent<Block1>().type];
                spwnorb1.GetComponent<Orb>().target = Recovery.gameObject;
            }
            else
            {
                for (int j = 0; j < panels.Length; j++)
                {
                    if (panels[j].unit.unit_Type == flydestroyblocks[i].GetComponent<Block1>().type)
                    {
                        GameObject spwnorb = Instantiate(orb, flydestroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                        spwnorb.GetComponent<SpriteRenderer>().color = blockcolor[(int)flydestroyblocks[i].GetComponent<Block1>().type];
                        spwnorb.GetComponent<Orb>().target = panels[j].gameObject;
                        GameObject spwnorb1 = Instantiate(orb, flydestroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                        spwnorb1.GetComponent<SpriteRenderer>().color = blockcolor[(int)flydestroyblocks[i].GetComponent<Block1>().type];
                        spwnorb1.GetComponent<Orb>().target = panels[j].gameObject;
                    }
                }
            }
            Destroy(flydestroyblocks[i]);
        }//블록 삭제

        destroyblocks.Clear();
        flydestroyblocks.Clear();

        yield return StartCoroutine(WaitOrb());
        Instantiate(block, fireblockposition, Quaternion.identity, blockstranstorm);
        MoveBlock(4);
        yield return wt;
        yield return StartCoroutine(WaitOrb());

        yield return wt;

        if (enemy.now_hp <= 0)
        {
            enemies.RemoveAt(0);
            if (enemies.Count == 0) GameEnd();
            else
            {
                enemy.enemy = enemies[0];
                enemy.EnemyUpdata();
                swpancount = 0;
                count_text.text = (4 - swpancount).ToString();
                bublecount=0;
            }
        }

        fireending = false;


    }

    public IEnumerator WaitOrb()
    {
        while (orb_transform.childCount>0 || effect_transform.childCount > 0)
        {
            yield return wt;
        }

    }//orb 없어질떄 까지 기다리기


    public void SpwanBlock()
    {
        saveindex = nextblockindex;


        for (int i = blockstranstorm.transform.childCount; i > 0; i--)
        {
            if (blockstranstorm.GetChild(i - 1).GetComponent<Block1>().state == State.End)
            {
                blockstranstorm.GetChild(i - 1).transform.position = new Vector2(blockstranstorm.GetChild(i - 1).transform.position.x, blockstranstorm.GetChild(i - 1).transform.position.y - offset);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject _block;
            if (offsetspwan)
            {
                _block = Instantiate(block, new Vector2(i + 0.5f, 0), Quaternion.identity, blockstranstorm);
            }
            else
            {
                _block = Instantiate(block, new Vector2(i, 0), Quaternion.identity, blockstranstorm);
            }
            _block.GetComponent<Block1>().state = State.End;
            _block.GetComponent<CircleCollider2D>().radius = 0.49f;

        }
        offsetspwan = !offsetspwan;
        nextblockindex = saveindex;
    }

    public void MoveBlock(int count)
    {
        if (bublecount % count == 0)
        {
            SpwanBlock();

            for (int i = 0; i < blockstranstorm.childCount; i++)
            {
                if ((blockstranstorm.GetChild(i).gameObject.transform.position.y < -6.5f) && (blockstranstorm.GetChild(i).GetComponent<Block1>().state == State.End))
                    destroyblocks.Add(blockstranstorm.GetChild(i).gameObject);

            }// y가 -6.5 이하에 블록은 일제 제거
            for (int i = 0; i < destroyblocks.Count; i++)
            {
                Destroy(destroyblocks[i]);
                GameObject spwnorb = Instantiate(orb, destroyblocks[i].transform.position, Quaternion.identity, orb_transform);
                spwnorb.GetComponent<SpriteRenderer>().color = new Color(0.75f,0,1,1);
                spwnorb.GetComponent<Orb>().target = enemy.gameObject;
            }//블록 삭제
            destroyblocks.Clear();
        }
        swpancount++;
        if (swpancount >= 4) swpancount = 0;

        count_text.text = (4- swpancount).ToString();
        bublecount++;
    }// n번 슈팅 후 블록 생성


    public void Scanbox(Collider2D _box)
    {
        destroyblocks.Add(_box.gameObject);

        Collider2D[] circle = Physics2D.OverlapCircleAll(_box.gameObject.transform.position, 1.0f);
        if (circle.Length != 0)
        {
            foreach (Collider2D coll in circle)
            {
                if ((coll.tag == "Block"))
                {
                    if ((coll.GetComponent<Block1>().type == _box.GetComponent<Block1>().type) && (!destroyblocks.Contains(coll.gameObject)))
                    {
                        Scanbox(coll);
                    }
                }
            }
        }
    }
    public void Flybox(Collider2D _box)
    {
        flydestroyblocks.Remove(_box.gameObject);
        Collider2D[] circle = Physics2D.OverlapCircleAll(_box.gameObject.transform.position, 1.0f);
        if (circle.Length != 0)
        {
            foreach (Collider2D coll in circle)
            {
                if (flydestroyblocks.Contains(coll.gameObject) && (coll.tag == "Block"))
                {
                    Flybox(coll);
                }
            }
        }
    }

    public bool Heal()
    {
        bool isHeal = false;

        now_Recovery++;
        isHeal = true;
        return isHeal;
    }

    public void GameEnd()
    {
        SendData.instance.maplevel++;
        SendData.instance.maplevel = Mathf.Clamp(SendData.instance.maplevel, 0, MapDataBase.instance.MapDB.Count - 1);
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void Gamepause(bool stop)
    {
        if (stop)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public float GetAngle(Vector2 vStart, Vector2 vEnd)
    {
        Vector2 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void GameEixt()
    {
        Application.Quit();
    }

}

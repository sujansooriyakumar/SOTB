using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderDisplay : MonoBehaviour
{
    private BattleState stateRef;
    private Queue<CombatParticipantData> turnOrder;
    [SerializeField] private Transform referenceParent;
    [SerializeField] private GameObject referencePrefab;

    [SerializeField] private TurnOrderIcon iconPrefab;
    [SerializeField] private Transform iconParent;
    private List<CombatParticipantData> currentOrder = new();
    private Dictionary<int, GameObject> references = new();
    [ShowInInspector] private List<TurnOrderIcon> spawnedIcons = new();
    private Dictionary<CombatParticipantData, TurnOrderIcon> turnIconDictionary = new();
    private Vector3 turnPlayerScale = new Vector3(1.3f, 1.3f, 1.3f);
    private bool isAnimating;
    public void InitializeTurnOrder(BattleState battleState)
    {
        stateRef = battleState;
        stateRef.OnBattleStateChanged += InitUpdate;
        turnOrder = battleState.turnOrder;

        for (int i = turnOrder.Count * 6; i >= 0; i--)
        {
            references.Add(i, Instantiate(referencePrefab, referenceParent));
        }
        references[0].GetComponent<RectTransform>().localScale = turnPlayerScale;
        LayoutRebuilder.ForceRebuildLayoutImmediate(referenceParent.GetComponent<RectTransform>());
        int iterations = turnOrder.Count > 6 ? turnOrder.Count : 6;

        for (int i = 0; i < iterations; i++)
        {
            TurnOrderIcon obj = Instantiate(iconPrefab, iconParent);
            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform ref_rect = references[i].GetComponent<RectTransform>();
            rect.position = ref_rect.position;
            rect.localScale = ref_rect.localScale;
            spawnedIcons.Add(obj);

        }

        List<CombatParticipantData> turnOrderList = turnOrder.ToList();
        currentOrder = turnOrderList;
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            int j = i >= turnOrderList.Count ? i% turnOrderList.Count : i;
            spawnedIcons[i].SetParticipantRef(turnOrderList[j]);

        }
        /*
         * get first  to go
         * instnatiate images to parent
         * map to placeholders
         * scale up current turn player 
         * 
         * on update:
         * spawn player outside of top 6 outside of cam (ie if turnorder.count > 6 spawn next player,
         * otherwise loop again
         * fade out current turn player
         * destroy current turn player object
         * lerp all players to next order
         * lerp scale up from next to go to current player
         */
    }
    private void InitUpdate(BattleState battleState) {  
        StartCoroutine(UpdateTurnOrder(battleState));

        /*
         * get difference diff btwn 6 & turnOrder count
         * move last turn icon to ref[7]
         * set last turn icon to icons[diff]
         * enqueue dequeue icons
         */
    }
    private IEnumerator UpdateTurnOrder(BattleState state)
    {
        while (isAnimating) yield return null;
        isAnimating = true;

        List<CombatParticipantData> oldOrder = currentOrder;
        List<CombatParticipantData> newOrder = state.turnOrder.ToList();

        if (oldOrder.Count == 0 || spawnedIcons.Count == 0)
        {
            currentOrder = newOrder;
            isAnimating = false;
            yield break;
        }

        // hash set for easier checks
        HashSet<CombatParticipantData> alive = new(newOrder);

        // check if turn advanced
        bool turnAdvanced = newOrder.Count > 0 &&
                            (!alive.Contains(oldOrder[0]) || newOrder[0] != oldOrder[0]);

        // store icons that need to leave
        List<TurnOrderIcon> leaving = new();
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            CombatParticipantData p = oldOrder[i % oldOrder.Count];
            bool died = !alive.Contains(p);
            bool endedTurn = turnAdvanced && i == 0;
            if (died || endedTurn)
                leaving.Add(spawnedIcons[i]);
        }

        // update current order and end early if no update required
        if (leaving.Count == 0)
        {
            currentOrder = newOrder;
            isAnimating = false;
            yield break;
        }

        // remove leaving icons from spanwed list
        currentOrder = newOrder;
        foreach (TurnOrderIcon icon in leaving)
            spawnedIcons.Remove(icon);

        // fade all leaving icons
        List<Coroutine> fades = new();
        foreach (TurnOrderIcon icon in leaving)
            fades.Add(StartCoroutine(FadeIcon(icon.gameObject, 1.0f)));
        foreach (Coroutine fade in fades)
            yield return fade;

        // destroy all icons if battle over
        if (newOrder.Count == 0)
        {
            foreach (TurnOrderIcon icon in leaving) Destroy(icon.gameObject);
            foreach (TurnOrderIcon icon in spawnedIcons) Destroy(icon.gameObject);
            spawnedIcons.Clear();
            isAnimating = false;
            yield break;
        }

        // refill queue
        int target = Mathf.Min(Mathf.Max(6, newOrder.Count), references.Count - 1);
        while (spawnedIcons.Count < target)
        {
            int slot = spawnedIcons.Count;

            TurnOrderIcon icon;
            if (leaving.Count > 0)
            {
                icon = leaving[leaving.Count - 1];
                leaving.RemoveAt(leaving.Count - 1);
            }
            else
            {
                icon = Instantiate(iconPrefab, iconParent);
            }

            RectTransform rect = icon.GetComponent<RectTransform>();
            SetAlpha(1f, icon.GetComponent<Image>());

            RectTransform park = references[Mathf.Min(slot + 1, references.Count - 1)].GetComponent<RectTransform>();
            rect.position = park.position;
            rect.localScale = park.localScale;
            icon.SetParticipantRef(newOrder[slot % newOrder.Count]);
            spawnedIcons.Add(icon);
        }

        // destroy leftover icons
        foreach (TurnOrderIcon icon in leaving)
            Destroy(icon.gameObject);

        // start slides
        List<Coroutine> slides = new();
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            RectTransform iconRect = spawnedIcons[i].GetComponent<RectTransform>();
            RectTransform slotRef = references[i].GetComponent<RectTransform>();
            slides.Add(StartCoroutine(SlideIconDown(iconRect, slotRef)));
        }
        foreach (Coroutine slide in slides)
            yield return slide;

        // resync
        for (int i = 0; i < spawnedIcons.Count; i++)
            spawnedIcons[i].SetParticipantRef(newOrder[i % newOrder.Count]);

        isAnimating = false;
    }

    private IEnumerator SlideIconDown(RectTransform icon, RectTransform nextPos)
    {
        Vector2 startPos = icon.position;
        Vector2 endPos = nextPos.position;

        Vector2 startScale = icon.localScale;
        Vector2 endScale = nextPos.localScale;
        float elapsed = 0f;
        float duration = 0.5f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            icon.position = Vector2.Lerp(startPos, endPos, t);
            icon.localScale = Vector2.Lerp(startScale, endScale, t);
            yield return null;
        }
        icon.position = endPos;
        icon.localScale = endScale;
        yield return null;
    }


    private IEnumerator FadeIcon(GameObject icon, float duration)
    {
        Image i = icon.GetComponent<Image>();
        float timer = 0f;

        SetAlpha(1f, i);
        while (timer < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / (duration));
            SetAlpha(alpha, i);

            timer += Time.deltaTime;
            yield return null;
        }
        // move other icons down
        // spawn in prev destroyed icon at top of list
        SetAlpha(0f, i);
    }

    private void SetAlpha(float alpha, Image image)
    {
    
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private GameObject target = null;
    
    [SerializeField]
    private Transform in_hand = null;
    
    [SerializeField]
    private Rig rig = null;
    
    [SerializeField]
    private Animator animator = null;
    
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioTakeFruit = null;
    [SerializeField]
    private AudioClip audioPutFruit = null;
    #endregion

    #region Private Fields
    private Fruit      fruitInHand           = null;
    private Quaternion defaultRotationInHand = Quaternion.Euler(0, -90.0f, -90.0f);
    
    private static readonly int putFruitTrigger = Animator.StringToHash("putFruitInBasket");
    private static readonly int danceTrigger    = Animator.StringToHash("dance");
    private int fruitLayer = 0;
    
    private bool isCoroutineRunning = false;
    private bool isInMenu           = false;
    #endregion


    #region Private Methods
    private void Start()
    {
        LevelInfo.onWin += OnWinHandler;
        Menu.onLevelStart += OnLevelStartHandler;
        fruitLayer = LayerMask.NameToLayer("Fruit");
    }

    private void OnDestroy()
    {
        LevelInfo.onWin -= OnWinHandler;
        Menu.onLevelStart -= OnLevelStartHandler;
    }

    private void Update()
    {
        if (!isCoroutineRunning)
            InputTouch();
    }

    private void InputTouch()
    {
        if (isInMenu)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                getObjectOnTouch(touch.position);
        }

        void getObjectOnTouch( Vector2 position )
        {
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 5.0f, 1 << fruitLayer))
            {
                TakeFruit(raycastHit.transform.gameObject);
            }
        }
    }

    private void TakeFruit( GameObject fruit )
    {
        float moveHandTime = 0.15f;
        
        StartCoroutine(moveHandToFruit());
        

        IEnumerator moveHandToFruit()
        {
            isCoroutineRunning = true;
            
            while (rig.weight < 1.0f)
            {
                if (!isFruitActive())
                {
                    yield return returnTheHand();
                    
                    isCoroutineRunning = false;
                    
                    yield break;
                }

                target.transform.position = fruit.transform.position;
                rig.weight += Time.deltaTime / moveHandTime;
                yield return null;
            }

            if (isFruitActive())
                yield return takeFruitInHand();
            else
            {
                yield return returnTheHand();
                
                isCoroutineRunning = false;
            }

            bool isFruitActive() => fruit.activeSelf;
        }

        IEnumerator returnTheHand()
        {
            while (rig.weight > 0)
            {
                rig.weight -= Time.deltaTime / moveHandTime;
                yield return null;
            }
        }

        IEnumerator takeFruitInHand()
        {
            audioSource.PlayAudioClip(audioTakeFruit);
            
            fruitInHand = fruit.GetComponent<Fruit>();
            fruitInHand.is_picked = true;
            
            fruit.transform.SetParent(in_hand);
            fruit.transform.localPosition = Vector3.zero;
            fruit.transform.localRotation = defaultRotationInHand;
            
            yield return returnTheHand();
            
            animator.SetTrigger(putFruitTrigger);
        }
    }

    private void OnWinHandler( bool is_win )
    {
        isInMenu = true;
        
        if (is_win)
            animator.SetTrigger(danceTrigger);
    }

    private void OnLevelStartHandler()
    {
        isInMenu = false;
    }
    #endregion

    #region Public Methods
    public void OnPutFruitAnimationEnded() => isCoroutineRunning = false;// Animation Events
    public void OnHandOverTheBasket()
    {
        audioSource.PlayAudioClip(audioPutFruit);
        Basket.instance.PutFruit(fruitInHand);
    }
    #endregion
}

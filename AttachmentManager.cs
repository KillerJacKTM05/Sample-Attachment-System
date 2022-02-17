using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RootMotion.Demos;
using RootMotion.Dynamics;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class AttachmentManager : Singleton<AttachmentManager>
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform playerParent;
    [SerializeField] [Range(0, 50f)] private float rotationSpeed = 5f;
    [SerializeField] List<GameObject> AttachableObjects;
    [SerializeField]public GameObject createdObj;

    [SerializeField] public Button BalloonButton;
    [SerializeField] public Button BombButton;

    private bool isParented = false;
    private bool lightened = false;
    private int objIndex;
    private Vector3 fingerPos;

    public bool objCreated = false;
    public bool isKicked = false;
    public bool isBalloonActive = false;
    public bool isGameWon = false;
    public PuppetMaster puppet;
    public BehaviourPuppet puppetBehaviour;

    private Bomb[] inGameBombs;
    private Balloon[] inGameBalloons;
    private LevelEndParticle[] levelEndingConfetties;
    private GameObject pelvis;
    private GameObject player;

    private bool isOnFoot = true;

    void Start()
    {
        lightened = false;
        
    }

    public void OnPmSet()
    {
        isOnFoot = true;
        puppetBehaviour.onRegainBalance.unityEvent.AddListener(delegate
        {
            isOnFoot = true;
        });
        puppetBehaviour.onLoseBalance.unityEvent.AddListener(delegate
        {
            isOnFoot = false;
        });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objCreated && !GameManager.CanPlay)
        {
            UpdatePos();
        }

        if (isGameWon && !lightened)
        {
            foreach (var fetti in levelEndingConfetties)
            {
                fetti.Enlighten();
            }
            lightened = true;
        }
    }
    public void SetPlatform(Transform plat)                 //set the platform reference
    {
        platform = plat;
    }
    public void SetPlayerParent(Transform Parent)
    {
        playerParent = Parent;
    }
    public void CoRoutineStarter()                                  //it's about character follows the platform's movement.
    {
        CameraManager.Instance.getTarget().SetParent(platform);
        isParented = true;
        StartCoroutine(RotatePlatform());
    }
    public void DetachParent()                                       //detach it.
    {
        isParented = false;
        CameraManager.Instance.getTarget().parent = playerParent;
        //find attached elements
        inGameBombs = Bomb.FindObjectsOfType<Bomb>();
        inGameBalloons = Balloon.FindObjectsOfType<Balloon>();
        levelEndingConfetties = LevelEndParticle.FindObjectsOfType<LevelEndParticle>();
    }

    public void InvokeAttachableObject(int ind, Vector3 pos)
    {
        createdObj = Instantiate(AttachableObjects[ind], pos, Quaternion.identity);
        objCreated = true;
    }

    public void SetIndex(int i)
    {
        objIndex = i;
        InvokeAttachableObject(objIndex, fingerPos);
    }

    public void SetPos(Vector3 pos)
    {
        fingerPos = pos;
    }

    public void UpdatePos()
    {
        createdObj.transform.position = fingerPos;
    }

    public void SetKick(int ind)
    {
        if (!isOnFoot)
            return;

        if(ind == 0)
        {
            isKicked = true;
        }
        else if(ind == 1)
        {
            isKicked = false;
        }
    }
    public void SetBalloon(int ind)
    {
        foreach (var balloonScript in inGameBalloons)
        {
            if (ind == 0)
            {
                isBalloonActive = true;
                CameraManager.Instance.SetTarget(pelvis.transform, pelvis.transform);
                balloonScript.Inflate();
            }
            else if (ind == 1)
            {
                isBalloonActive = false;
                CameraManager.Instance.SetTarget(player.transform, player.transform);
                balloonScript.DetachBalloon();
            }
        }
    }
    
    public void Explosionnnn()
    {
        puppet.state = PuppetMaster.State.Dead;
        
        foreach(var bombScript in inGameBombs)
        {
            bombScript.Explosion();
        }

        DOVirtual.DelayedCall(3, () =>
        {
            puppet.state = PuppetMaster.State.Alive;
        });
    }

    public Transform getPlayerParent()
    {
        return playerParent;
    }

    public void SetPelvis(GameObject pel)
    {
        pelvis = pel;
    }
    public void SetPlayer(GameObject ply)
    {
        player = ply;
    }
    private IEnumerator RotatePlatform()                    //this is a placeholder routine for now, this will be changed according to game mechanics.
    {
        while (!GameManager.CanPlay)
        {
            platform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up, Space.World);
            if (GameManager.CanPlay)
            {
                yield break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
    }
}

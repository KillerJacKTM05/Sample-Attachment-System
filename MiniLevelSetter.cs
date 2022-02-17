using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class MiniLevelSetter : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _platform;
    [SerializeField] private PuppetMaster _pm;
    [SerializeField] private BehaviourPuppet _bp;
    [SerializeField] private GameObject _pelvis;
    void Start()
    {
        ProgressBarManager.Instance.SetData(_start, _end, _player);
        AttachmentManager.I.SetPlatform(_platform);
        AttachmentManager.I.SetPlayerParent(_player.parent);
        AttachmentManager.I.puppet = _pm;
        AttachmentManager.I.puppetBehaviour = _bp;
        AttachmentManager.I.OnPmSet();

        AttachmentManager.I.SetPelvis(_pelvis);
        AttachmentManager.I.SetPlayer(_player.gameObject);
        CameraManager.Instance.SetTarget(_player.transform, _player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

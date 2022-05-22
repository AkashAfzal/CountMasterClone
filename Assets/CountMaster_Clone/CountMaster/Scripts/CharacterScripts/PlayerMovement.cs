using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float minX = -2.5f;
    public float maxX = 2.5f;
    public float forwardSpeed = 10f;
    public float horizontalSpeed = 7.5f;
    public float distanceCamToChar = 40f;
    [Space(10)]
    public bool userCanControl = true;
    public bool moveForward = true;
    public bool moveHorizontal = true;
    [Space(10)]
    public Finish finish;
    public Cinemachine.CinemachineVirtualCamera finishCam;

    Vector3 charStartPos = Vector3.zero;
    Vector3 startPoint = Vector3.zero;
    Vector3 endPoint = Vector3.zero;
    PlayerController _playerController;
    IEnumerator fightMoveCo;
    Camera _camera;
    Vector3 inputPoint = Vector3.zero;
    float movableLeft = 0f;
    float movableRight = 0f;
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _camera = Camera.main;
        StartCoroutine(UpdateBorders());
    }
    private void Update()
    {
        if (userCanControl)
        {
            Move();
        }
    }
    public virtual void Move()
    {
        if (moveForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, forwardSpeed * Time.deltaTime);
        }
        if (moveHorizontal)
        {
            inputPoint = Input.mousePosition;
            inputPoint.z = distanceCamToChar;
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = _camera.ScreenToWorldPoint(inputPoint);
                startPoint.x -= _camera.transform.position.x;
                charStartPos = transform.localPosition;
            }
            if (Input.GetMouseButton(0))
            {
                endPoint = Camera.main.ScreenToWorldPoint(inputPoint);
                endPoint.x -= _camera.transform.position.x;
                float distance = endPoint.x - startPoint.x;
                Vector3 targetPos = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x + distance, transform.localPosition.y, transform.localPosition.z), horizontalSpeed * Time.deltaTime);
                if ((targetPos.x > movableLeft || transform.position.x < targetPos.x) && (targetPos.x < movableRight || transform.position.x > targetPos.x))
                {
                    transform.localPosition = targetPos;
                    startPoint.x += transform.localPosition.x - charStartPos.x;
                    charStartPos = transform.localPosition;
                }
                else
                {
                    startPoint = _camera.ScreenToWorldPoint(inputPoint);
                    startPoint.x -= _camera.transform.position.x;
                    charStartPos = transform.localPosition;
                }
            }
        }
    }
    
    private IEnumerator UpdateBorders()
    {
        while (!LevelController.instance.isLevelFinished)
        {
            float leftBorder = 0f;
            float rightBorder = 0f;
            foreach (Character _character in _playerController.allCharacters)
            {
                if (_character.transform.localPosition.x < leftBorder)
                {
                    leftBorder = _character.transform.localPosition.x;
                }
                if (_character.transform.localPosition.x > rightBorder)
                {
                    rightBorder = _character.transform.localPosition.x;
                }
            }
            movableLeft = minX - leftBorder;
            movableRight = maxX - rightBorder;
            yield return new WaitForSeconds(1f);
        }
    }

    public void FightMove(Transform target)
    {
        if(fightMoveCo == null)
        {
            fightMoveCo = FightMoveRoutine(target);
            StartCoroutine(fightMoveCo);
        }
    }
    public void StopFightMove()
    {
        if(fightMoveCo != null)
        {
            StopCoroutine(fightMoveCo);
            fightMoveCo = null;
        }
    }
    IEnumerator FightMoveRoutine(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        while (!userCanControl)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.5f * Time.deltaTime);
            yield return null;
        }
        fightMoveCo = null;
    }
    public IEnumerator MoveCenter()
    {
        moveHorizontal = false;
        while (transform.position != new Vector3(0f, transform.position.y, transform.position.z))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, transform.position.y, transform.position.z), horizontalSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(FinishMove());
    }
    private IEnumerator FinishMove()
    {
        userCanControl = false;
        int count = 0;
        Vector3 targetPos;
        while (transform.position != finish.transform.position)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, finish.transform.position, forwardSpeed * Time.deltaTime);
            yield return null;
        }
        
        while (_playerController.finishObjects.Count > 0)
        {
            targetPos = finish.transform.position + new Vector3(0f, 0f, count);
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, forwardSpeed * Time.deltaTime);
                yield return null;
            }
            count++;
            finish.finishSteps[count].stepCamera.SetActive(true);
            _playerController.finishObjects[_playerController.finishObjects.Count - 1].parent = null;
            for (int i = 0; i < _playerController.finishObjects[_playerController.finishObjects.Count - 1].childCount; i++)
            {
                _playerController.finishObjects[_playerController.finishObjects.Count - 1].GetChild(i).GetComponent<Animator>().SetBool("Run", false);
            }
            _playerController.finishObjects.RemoveAt(_playerController.finishObjects.Count - 1);
        }
        if (count - 2 > 0)
        {
            yield return new WaitForSeconds(0.5f);
            finish.finishSteps[count - 2].particles.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            LevelController.instance.Win();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            LevelController.instance.Win();
        }
    }
}
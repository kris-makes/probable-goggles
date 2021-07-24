using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{


    // ============================
    // Public variables
    // ============================


    // ----------------------------
    // Player properties
    // ----------------------------

    // Speeds
    [Range(1f, 50f)] public float speed;
    [Range(1f, 50f)] public float idleSpeed;
    [Range(1f, 50f)] public float runningSpeed;
    [Range(1f, 50f)] public float crouchSpeed;
    [Range(1f, 50f)] public float maxSpeed;
    [Range(1f, 50f)] public float maxSpeedRun;
    [Range(0.1f, 50f)] public float maxSpeedCrouch;

    [Range(0.1f, 5f)] public float crouchHeight;
    
    // Jumping
    [Range(1f, 500f)] public float jumpPower;
    public int jumpCount;
    public int maxJumps;

    // States
    public bool inAir = false;
    public bool isStopped;
    public bool isRunning;
    public bool isCrouching;

    // Grabbing
    [Range(0.5f, 15f)] public float grabRange;
    public LayerMask layerMask;

    // Axes
    public float horizontal, vertical;
    public float mouseX, mouseY;

    // Mouse (lehet, hogy az input beállításos értéket kéne ezzel egyenlõvé tenni?)
    [Range(1f, 50f)] public float mouseSensitivity;

    public GameObject leftHand;
    public GameObject rightHand;

    // Camera reference and settings
    public Camera cam;
    public Vector3 camOffset;
    Vector3 camRotation;
    Quaternion camStartRotation = new Quaternion(0f, 0f, 0f, 0f);


    // Components
    public Rigidbody rb;
    public CapsuleCollider capsCol;
    public RectTransform cursorTransform;
    public TextMeshProUGUI objectName;
    public Vector2 normalCursor = new Vector2(128f, 128f);
    public Vector2 bigCursor = new Vector2(128f, 128f);
    public Vector2 largeCursor = new Vector2(64f, 64f);
    public Image cursorImage;
    public Sprite handIcon;
    public Sprite defaultCursor;
    public float normalCursorSize;
    public float bigCursorSize;
    public float largeCursorSize;

    // TODO: for teleporting, remove from final build
    public Transform startingPoint;
    int currentPosition = 0;


    // ============================
    // Private variables
    // ============================


    int i;

    Vector3 standingPosition;
    Vector3 standingScale;
    Vector3 crouchPosition;
    Vector3 crouchScale;
    Vector3 crouchOffset = new Vector3(0f, 0.65f, 0f);

    bool upperLimmit, lowerLimit;
    GameObject collidedWith;
    int collisionCount;
    GameObject lookingAtObject;
    GameObject currentlyHeldObject;
    Action heldObjectAction;
    KeyCode currentKeycode;
    Vector3 objectColliderSize;
    Vector3 placeDownPosition;
    Vector3 forwardDirection;
    Vector3 rightDirection;
    int direction;
    Quaternion placeDownRotation;
    RaycastHit hitInfo;
    Rigidbody heldRB;
    Collider heldCol;
    bool interactable;
    bool grabbable;
    bool gadget;
    bool canItDoSomething;


    // Start is called before the first frame update
    void Start()
    {
        cam.transform.position = transform.position + camOffset;
        standingPosition = cam.transform.position;
        standingScale = transform.localScale;
        cam.transform.rotation = camStartRotation;
        transform.position = startingPoint.position;
        speed = idleSpeed;
        crouchPosition = new Vector3(cam.transform.position.x, crouchHeight, cam.transform.position.z);
        crouchScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        //cursorTransform.sizeDelta = normalCursor; TODO: miért nem jó így?
        cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normalCursorSize);
        cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, normalCursorSize);

        Vector3 playerVelocity = new Vector3(maxSpeed, maxSpeed, maxSpeed); //TODO: ez nem feltétlen kell

        // TODO: Ez a rész itt nem feltétlen kell
        objectName.text = "";
        lookingAtObject = null; // TODO: erre talán lehetne jobb megoldás
        interactable = false; // erre is
        grabbable = false; // erre is
        cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normalCursorSize);
        cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, normalCursorSize);
        //cursorTransform.sizeDelta = normalCursor; TODO: Miért nem jó?
        cursorImage.sprite = defaultCursor;
    }


    // Update is called once per frame
    void Update()
    {
        // Update camera position
        cam.transform.position = transform.position + camOffset;

        // Get axes
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        
        // Move player
        rb.AddForce(transform.forward * vertical * Time.deltaTime * speed, ForceMode.VelocityChange);
        rb.AddForce(transform.right * horizontal * Time.deltaTime * speed, ForceMode.VelocityChange);
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            direction = rb.velocity.x > 0f ? 1 : -1;
            rb.velocity = new Vector3(maxSpeed * direction, rb.velocity.y, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.z) > maxSpeed)
        {
            direction = rb.velocity.z > 0f ? 1 : -1;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed *direction);
        }


        // Looking around
        camRotation = cam.transform.eulerAngles;
        upperLimmit = camRotation.x < 360f && camRotation.x > 275f;
        lowerLimit = camRotation.x >= 0f && camRotation.x < 85f;
        cam.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity, Space.World);
        
        if (upperLimmit || lowerLimit)
        {
            cam.transform.Rotate(-1f * Vector3.right * mouseY * Time.deltaTime * mouseSensitivity, Space.Self);
        }
        else
        {
            if (mouseY >= 0f && camRotation.x > 85f && camRotation.x < 90f)
            {
                cam.transform.Rotate(-1f * Vector3.right * mouseY * Time.deltaTime * mouseSensitivity, Space.Self);
            }
            else if (mouseY <= 0f && camRotation.x < 275f && camRotation.x > 270f)
            {
                cam.transform.Rotate(-1f * Vector3.right * mouseY * Time.deltaTime * mouseSensitivity, Space.Self);
            }
            else
            {
                Debug.LogError("Beragadt a kamera!");
            }
        }

        transform.Rotate(transform.up * mouseX * Time.deltaTime * mouseSensitivity);

        
        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            inAir = true;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            ++jumpCount;
        }

        // Running
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true;
            speed = runningSpeed;
            maxSpeed *= maxSpeedRun;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            speed = idleSpeed;
            maxSpeed /= maxSpeedRun;
        }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // TODO: a collider-t a lábhoz helyezni
            isCrouching = true;
            isStopped = false; // TODO: nem biztos, hogy kell
            camOffset *= crouchHeight;
            cam.transform.position = crouchPosition; // TODO: Interpolálással? (Lerp)
            transform.localScale = crouchScale; // TODO: Interpolálással? (Lerp)
            maxSpeed *= maxSpeedCrouch;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            camOffset /= crouchHeight;
            maxSpeed /= maxSpeedCrouch;
            cam.transform.position = standingPosition;
            transform.localScale = standingScale;
        }

        
        // Handling stopping player movement
        if (horizontal != 0f || vertical != 0f || inAir)
        {
            isStopped = false;
        }
        else
        {
            isStopped = true;
        }

        if (isStopped)
        {
            //Debug.Log("stopped");
            rb.velocity = Vector3.zero;
        }


        // Showing an object's name
        if (Raycast(grabRange, false))
        {
            lookingAtObject = hitInfo.collider.gameObject;
            // TODO: ez a canItDoSomething nem biztos hogy a legjobb megoldás
            canItDoSomething = lookingAtObject.CompareTag("Grab") || lookingAtObject.CompareTag("Action") || lookingAtObject.CompareTag("Gadget") || lookingAtObject.CompareTag("Interact");
            grabbable = lookingAtObject.CompareTag("Grab") || lookingAtObject.CompareTag("Gadget") || lookingAtObject.CompareTag("Interact");
            interactable = lookingAtObject.CompareTag("Action") || lookingAtObject.CompareTag("Gadget") || lookingAtObject.CompareTag("Interact");
            if (canItDoSomething)
            {
                cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bigCursorSize);
                cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bigCursorSize);
                objectName.text = lookingAtObject.name/* + (interactable ? " [E]" : "")*/;
                if (interactable)
                {
                    heldObjectAction = lookingAtObject.GetComponent<Action>();
                    if (heldObjectAction != null && heldObjectAction.whatToDo[0] != "")
                    {
                        for (int i = 0; i < heldObjectAction.keyCodes.Length; ++i)
                        {
                            objectName.text += $"\n({heldObjectAction.whatToDo[i]}) [{heldObjectAction.keyCodes[i]}]";
                        }
                        //objectName.text += $"\n({heldObjectAction.whatToDo})";
                    }
                }
                //cursorTransform.sizeDelta = bigCursor; TODO: Miért nem jó?
                if (grabbable)
                {
                    cursorImage.sprite = handIcon;
                    cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, largeCursorSize);
                    cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, largeCursorSize);
                }
            }
            else
            {
                objectName.text = "";
                lookingAtObject = null; // TODO: erre talán lehetne jobb megoldás
                interactable = false; // erre is
                grabbable = false; // erre is
                cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normalCursorSize);
                cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, normalCursorSize);
                //cursorTransform.sizeDelta = normalCursor; TODO: Miért nem jó?
                cursorImage.sprite = defaultCursor;
            }
        }
        else
        {

        }
        /* TODO: ez hogy is legyen?
        else
        {
            objectName.text = "";
            lookingAtObject = null; // TODO: erre talán lehetne jobb megoldás
            interactable = false; // erre is
            grabbable = false; // erre is
            cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normalCursorSize);
            cursorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, normalCursorSize);
            //cursorTransform.sizeDelta = normalCursor; TODO: Miért nem jó?
            cursorImage.sprite = defaultCursor;
        }
        */

        // TODO: kevésbé számítás igényes? Vagy ez sem rossz?
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keycode))
            {
                currentKeycode = keycode;
            }
        }

        // Interacting with an object
        if (Input.GetKeyDown(currentKeycode))
        {
            if (lookingAtObject != null)
            {
                interactable = lookingAtObject.CompareTag("Action") || lookingAtObject.CompareTag("Gadget") || lookingAtObject.CompareTag("Interact");
                if (interactable)
                    if (heldObjectAction != null)
                    {
                        i = 0;
                        while (i < heldObjectAction.keyCodes.Length && heldObjectAction.keyCodes[i] != currentKeycode)
                        {
                            ++i;
                        }
                        if (i < heldObjectAction.keyCodes.Length)
                        {
                            heldObjectAction.actions[i].Invoke();
                            Debug.Log($"Interaction with {lookingAtObject.name}\nKeycode: {currentKeycode}\nAction: {heldObjectAction.actions[i]}");
                        }
                    }
            }
        }

        
        // Picking up an object
        if (Input.GetMouseButtonDown(0))
        {
            
            if (lookingAtObject != null)
            {
                grabbable = lookingAtObject.CompareTag("Grab") || lookingAtObject.CompareTag("Gadget") || lookingAtObject.CompareTag("Interact");
                if (grabbable)
                {
                    currentlyHeldObject = lookingAtObject;
                    objectColliderSize = hitInfo.collider.bounds.size; // ez talán nem kell
                    currentlyHeldObject.transform.position = leftHand.transform.position;
                    currentlyHeldObject.transform.SetParent(leftHand.transform);
                    currentlyHeldObject.transform.localScale /= 2.5f; // TODO: a gameObject mérete szerint kéne
                    heldRB = currentlyHeldObject.GetComponent<Rigidbody>();
                    if (heldRB != null)
                    {
                        heldRB.isKinematic = true;
                    }
                    heldCol = currentlyHeldObject.GetComponent<Collider>();
                    if (heldCol != null)
                    {
                        heldCol.isTrigger = true;
                    }
                    //currentlyHeldObject.SetActive(false);
                }
            }
        }
        
        // Placing down an object
        if (Input.GetMouseButtonDown(1))
        {
            if (Raycast(grabRange, false) && currentlyHeldObject != null)
            {
                Debug.Log(objectColliderSize);
                placeDownPosition = hitInfo.point + objectColliderSize / 2f; // itt megcsinálni jól
                Debug.Log(hitInfo.point + ", " + objectColliderSize / 2f);
                placeDownRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
                currentlyHeldObject.transform.localScale *= 2.5f;
                currentlyHeldObject.transform.SetParent(null);
                heldCol.isTrigger = false;
                currentlyHeldObject.transform.position = placeDownPosition;
                currentlyHeldObject.transform.rotation = placeDownRotation;
                heldRB.isKinematic = false;
                //currentlyHeldObject.SetActive(true);
                currentlyHeldObject = null;
            }
        }


        // TODO: JUST FOR TESTING: TELEPORTING, REMOVE FROM FINAL BUILD
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentPosition == 0)
            {
                transform.position = new Vector3(4f, 1f, 4f);
                currentPosition = 1;
            }
            else
            {
                transform.position = startingPoint.position;
                currentPosition = 0;
            }
        }

    }
    

    // Checking if player is standing on something
    private void OnTriggerEnter(Collider other)
    {
        ++collisionCount;
        collidedWith = other.gameObject;
        inAir = false;
        jumpCount = 0;
    }



    private void OnTriggerExit(Collider other)
    {
        --collisionCount;
        if (collisionCount == 0)
        {
            inAir = true;
        }
    }


    // Raycasting
    bool Raycast(float grabRange, bool onlyObjects)
    {
        if (onlyObjects)
            return Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, grabRange, ~7);
        else
            return Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, grabRange, ~layerMask);
    }

    Vector3 GetLargestVector3Value(Vector3 input)
    {
        if (input[0] > input[1])
            if (input[0] > input[2])
                return new Vector3(1, 0, 0);
            else if (input[2] > input[1])
                return new Vector3(0, 0, 1);
        return new Vector3(0, 1, 0);
    }
}
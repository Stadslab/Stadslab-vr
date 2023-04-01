using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class buttonScript : MonoBehaviour
{

    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;

    private bool ispressed;
    private Vector3 startPos;
    public Animator leftAnimator;
    public Animator rightAnimator;
    private ConfigurableJoint joint;

    public UnityEvent onPressed, onReleased;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();

    }

    // Update is called once per frame
    void Update()
    {
      if(!ispressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }
      if (ispressed && GetValue() - threshold <= 0)
        {
            released();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;
        if (Mathf.Abs(value) < deadzone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1f, 1f);
    }

    public void click()
    {
        Debug.Log("click");
        leftAnimator.SetTrigger("press");
        rightAnimator.SetTrigger("click");
    }
    private void Pressed()
    {
        ispressed = true;
        onPressed.Invoke();
        Debug.Log("Pressed");
    }

    private void released()
    {
        ispressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
    }
}

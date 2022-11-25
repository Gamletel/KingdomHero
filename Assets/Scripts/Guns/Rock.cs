using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
public class Rock : MonoBehaviour
{
    private Rigidbody _rb;
    private float _lifetime = 10f;
    private bool _isGrounded = false;
    [SerializeField] private float _rotationSpeed = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (_isGrounded)
            return;
        transform.Rotate(Vector3.forward, _rotationSpeed);
    }

    private void OnEnable()
    {
        this.StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this._rb.constraints = RigidbodyConstraints.FreezeAll;
            _isGrounded = true;
        } 
    }

    private void OnDisable()
    {
        _rb.constraints = RigidbodyConstraints.None;
        this.StopCoroutine(LifeTime());
        _isGrounded = false;
    }

    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifetime);
        this._rb.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}

﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;          // ความเร็วเดินหน้า
    public float laneDistance = 3f;   // ระยะห่างของเลน
    public float tiltAngle = 20f;     // องศาการเอียง
    public float tiltSpeed = 5f;      // ความเร็วการเอียง
    private int currentLane = 1;      // เลนเริ่มต้น (0=ซ้าย, 1=กลาง, 2=ขวา)
    private Quaternion targetTilt;    // การเอียงตัว

    public CoinManager coinManager;

    void Start()
    {
        targetTilt = transform.rotation; // ตั้งค่าการเอียงเริ่มต้น
    }

    void Update()
    {
        MoveForward();    // ตัวละครเดินหน้า
        HandleLaneChange();  // ย้ายเลน
        HandleTilting();  // เอียงตัว
    }

    void MoveForward()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime; // X = เดินหน้า
    }

    void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0) // A = ไปซ้าย
        {
            currentLane--;
            MoveToLane();
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2) // D = ไปขวา
        {
            currentLane++;
            MoveToLane();
        }
    }

    void MoveToLane()
    {
        float newZ = (1 - currentLane) * laneDistance; // ซ้ายเป็น +, ขวาเป็น -
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, newZ);
        transform.position = newPosition;
    }

    void HandleTilting()
    {
        if (Input.GetKey(KeyCode.Q)) // กด Q เอียงซ้าย
        {
            targetTilt = Quaternion.Euler(-tiltAngle, 0, 0); // เปลี่ยนจาก Z เป็น X
        }
        else if (Input.GetKey(KeyCode.E)) // กด E เอียงขวา
        {
            targetTilt = Quaternion.Euler(tiltAngle, 0, 0);
        }
        else // ปล่อยปุ่ม กลับมาตรง
        {
            targetTilt = Quaternion.Euler(0, 0, 0);
        }

        // ทำ Smooth Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTilt, Time.deltaTime * tiltSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinManager.coinCount++;
        }
    }
}
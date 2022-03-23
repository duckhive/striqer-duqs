using System;
using System.Collections.Generic;
using UnityEngine;

namespace Replay
{
    public class ActionReplay : MonoBehaviour
    {
        private bool replayMode;
        private float currentReplayIndex;
        private float indexChangeRate;
        private Rigidbody rb;
        private List<ReplayRecord> replayRecords = new List<ReplayRecord>();


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                replayMode = !replayMode;

                if (replayMode)
                {
                    SetTransform(0);
                    rb.isKinematic = true;
                }
                else
                {
                    SetTransform(replayRecords.Count - 1);
                    rb.isKinematic = false;
                }
            }

            indexChangeRate = 0;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                indexChangeRate = 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                indexChangeRate = -1;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                indexChangeRate *= 0.5f;
            }
        }   

        private void FixedUpdate()
        {
            if(!replayMode)
                replayRecords.Add(new ReplayRecord { pos = transform.position, rot = transform.rotation});
            else
            {
                float nextIndex = currentReplayIndex + indexChangeRate;

                if (nextIndex < replayRecords.Count && nextIndex >= 0)
                {
                    SetTransform(nextIndex);
                }
            }
        }

        private void SetTransform(float index)
        {
            currentReplayIndex = index;
            
            ReplayRecord replayRecord = replayRecords[(int)index];

            transform.position = replayRecord.pos;
            transform.rotation = replayRecord.rot;
        }
    }
}

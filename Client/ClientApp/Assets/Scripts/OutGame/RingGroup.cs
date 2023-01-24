using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RingGroup : MonoBehaviour
{
    public float ringRotateDeltaOffset = 10f;
    public float ringRotateSpeed = 60f;
    public float ringRotateDeltaIncreaseAmount = 180f;

    private class Ring
    {
        public Transform transform;
        public float rotateSpeedIncreaseDelta;
    }

    private List<Ring> rings = new();

    private void Start()
    {
        var ringList = GetComponentsInChildren<Transform>()
            .Where(x => x.name == "Ring");

        var index = 0;
        foreach (var ringTf in ringList)
        {
            ringTf.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            rings.Add(
                new Ring
                {
                    transform = ringTf,
                    rotateSpeedIncreaseDelta = index * ringRotateDeltaOffset
                }
            );
            index++;
        }
    }

    private void Update()
    {
        foreach (var ring in rings)
        {
            ring.transform.Rotate(0, 0, (Mathf.Sin(Mathf.Deg2Rad * ring.rotateSpeedIncreaseDelta) + 1.6f) * ringRotateSpeed * Time.deltaTime);

            ring.rotateSpeedIncreaseDelta += ringRotateDeltaIncreaseAmount * Time.deltaTime;

            if (ring.rotateSpeedIncreaseDelta > 360)
            {
                ring.rotateSpeedIncreaseDelta -= 360;
            }
        }
    }
}

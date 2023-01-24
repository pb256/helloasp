using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool<T> where T : MonoBehaviour
{
    private T mOriginal;
    private Stack<T> mPool = new();
    private Transform mParent;
    private Action<T> mInitAction;

    /// <summary>
    /// 한개 개체 생성
    /// </summary>
    public void Create()
    {
        T item = GameObject.Instantiate(mOriginal, mParent);

        mInitAction?.Invoke(item);
        item.gameObject.SetActive(false);

        mPool.Push(item);
    }

    /// <summary>
    /// 오브젝트 풀 초기 로드
    /// </summary>
    /// <param name="obj">오브젝트</param>
    /// <param name="count">개수</param>
    /// <param name="initAction">초기화 함수</param>
    public void Load(T obj, int count, Action<T> initAction = null)
    {
        if (mOriginal != obj)
        {
            Clear();
        }

        mOriginal = obj;
        mInitAction = initAction;
        mPool.Clear();

        for (var i = 0; i < count; i++)
        {
            Create();
        }
    }

    public void Clear()
    {
        foreach (T item in mPool)
        {
            GameObject.Destroy(item);
        }

        mPool.Clear();

        mOriginal = null;
        mInitAction = null;
    }

    public T Request()
    {
        if (mPool.Count == 0)
        {
            Create();
        }
        T item = mPool.Pop();

        item.gameObject.SetActive(true);
        item.transform.SetParent(mParent);

        return item;
    }

    public void Return(T item)
    {
        if (mPool.Contains(item))
        {
            // 충돌 체크와 같이 한 프레임에 두 번 이상 일어날 수 있는 곳에서 반환시 주의
            Debug.LogError($"{typeof(T)} 인스턴스의 풀 반환 중복 확인 됨");
            return;
        }

        mPool.Push(item);
        item.gameObject.SetActive(false);
    }

    public void SetParent(Transform parent)
    {
        mParent = parent;
    }
}

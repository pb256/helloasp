using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPoolManager : SceneSingleton<ObjectPoolManager>
{
    #region 프리팹 리스트
    public Bullet bullet;
    public EnemyBullet enemyBullet;
    public EnemyWarning enemyWarning;
    public Enemy_Circle enemyCircle;
    public Enemy_Penta enemyPenta;
    public Enemy_Tri enemyTri;
    public Enemy_Diamond enemyDiamond;
    public Enemy_Star enemyStar;
    public Enemy_Sun enemySun;
    public Enemy_Hexa enemyHexa;
    public Enemy_Line enemyLine;
    public EnemyExplosion enemyExplosion;
    #endregion

    #region 풀 리스트
    private ObjectPool<Bullet> mBulletPool = new();
    private ObjectPool<EnemyBullet> mEnemyBulletPool = new();
    private ObjectPool<EnemyWarning> mEnemyWarningPool = new();
    private ObjectPool<Enemy_Circle> mEnemyCirclePool = new();
    private ObjectPool<Enemy_Penta> mEnemyPentaPool = new();
    private ObjectPool<Enemy_Tri> mEnemyTriPool = new();
    private ObjectPool<Enemy_Diamond> mEnemyDiamondPool = new();
    private ObjectPool<Enemy_Star> mEnemyStarPool = new();
    private ObjectPool<Enemy_Sun> mEnemySunPool = new();
    private ObjectPool<Enemy_Hexa> mEnemyHexaPool = new();
    private ObjectPool<Enemy_Line> mEnemyLinePool = new();
    private ObjectPool<EnemyExplosion> mEnemyExplosionPool = new();
    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        mBulletPool.SetParent(transform);
        mBulletPool.Load(bullet, 20);

        mEnemyBulletPool.SetParent(transform);
        mEnemyBulletPool.Load(enemyBullet, 50);

        mEnemyCirclePool.SetParent(transform);
        mEnemyCirclePool.Load(enemyCircle, 30);
        mEnemyPentaPool.SetParent(transform);
        mEnemyPentaPool.Load(enemyPenta, 30);
        mEnemyTriPool.SetParent(transform);
        mEnemyTriPool.Load(enemyTri, 30);
        mEnemyDiamondPool.SetParent(transform);
        mEnemyDiamondPool.Load(enemyDiamond, 30);
        mEnemyStarPool.SetParent(transform);
        mEnemyStarPool.Load(enemyStar, 30);
        mEnemySunPool.SetParent(transform);
        mEnemySunPool.Load(enemySun, 30);
        mEnemyHexaPool.SetParent(transform);
        mEnemyHexaPool.Load(enemyHexa, 30);
        mEnemyLinePool.SetParent(transform);
        mEnemyLinePool.Load(enemyLine, 30);

        mEnemyWarningPool.SetParent(transform);
        mEnemyWarningPool.Load(enemyWarning, 30);

        mEnemyExplosionPool.SetParent(transform);
        mEnemyExplosionPool.Load(enemyExplosion, 20);
    }

    public Bullet GetBullet()
    {
        return mBulletPool.Request();
    }

    public void ReturnBullet(Bullet item)
    {
        mBulletPool.Return(item);
    }

    public EnemyBullet GetEnemyBullet()
    {
        return mEnemyBulletPool.Request();
    }

    public void ReturnenemyBullet(EnemyBullet item)
    {
        mEnemyBulletPool.Return(item);
    }

    public EnemyWarning GetEnemyWarning()
    {
        return mEnemyWarningPool.Request();
    }

    public void ReturnEnemyWarning(EnemyWarning item)
    {
        mEnemyWarningPool.Return(item);
    }

    public Enemy_Circle GetEnemyCircle()
    {
        return mEnemyCirclePool.Request();
    }

    public void ReturnEnemyCircle(Enemy_Circle item)
    {
        mEnemyCirclePool.Return(item);
    }

    public Enemy_Penta GetEnemyPenta()
    {
        return mEnemyPentaPool.Request();
    }

    public void ReturnEnemyPenta(Enemy_Penta item)
    {
        mEnemyPentaPool.Return(item);
    }

    public Enemy_Tri GetEnemyTri()
    {
        return mEnemyTriPool.Request();
    }

    public void ReturnEnemyTri(Enemy_Tri item)
    {
        mEnemyTriPool.Return(item);
    }

    public Enemy_Diamond GetEnemyDiamond()
    {
        return mEnemyDiamondPool.Request();
    }

    public void ReturnEnemyDiamond(Enemy_Diamond item)
    {
        mEnemyDiamondPool.Return(item);
    }

    public Enemy_Star GetEnemyStar()
    {
        return mEnemyStarPool.Request();
    }

    public void ReturnEnemyStar(Enemy_Star item)
    {
        mEnemyStarPool.Return(item);
    }

    public Enemy_Sun GetEnemySun()
    {
        return mEnemySunPool.Request();
    }

    public void ReturnEnemySun(Enemy_Sun item)
    {
        mEnemySunPool.Return(item);
    }

    public Enemy_Hexa GetEnemyHexa()
    {
        return mEnemyHexaPool.Request();
    }

    public void ReturnEnemyHexa(Enemy_Hexa item)
    {
        mEnemyHexaPool.Return(item);
    }

    public Enemy_Line GetEnemyLine()
    {
        return mEnemyLinePool.Request();
    }

    public void ReturnEnemyLine(Enemy_Line item)
    {
        mEnemyLinePool.Return(item);
    }

    public EnemyExplosion GetEnemyExplosion()
    {
        return mEnemyExplosionPool.Request();
    }

    public void ReturnEnemyExplosion(EnemyExplosion item)
    {
        mEnemyExplosionPool.Return(item);
    }
}

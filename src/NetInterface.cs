using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetInterface : MonoBehaviour
{
    protected Net mind = null;
    protected float score;
    protected int id;
    protected float[] outputs;
    public static int next_id = 0;
	public static bool MACHINE = false;

    protected abstract IEnumerator Think();

    public void Initialize(Net net)
    {
        mind = net;
        score = 0;
        id = next_id;
        next_id++;
    }

    public void Initialize(Net net, float score, int id)
    {
        mind = net;
        this.score = score;
        this.id = id;
    }

    public float Score { get => score; set => score = value; }
    public Net Mind { get => mind; set => mind = value; }
    public int Id { get => id; set => id = value; }
}

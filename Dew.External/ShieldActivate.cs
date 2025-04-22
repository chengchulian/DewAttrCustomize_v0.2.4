using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldActivate : MonoBehaviour
{
	public float ImpactLife;

	private Vector4[] points;

	private Material m_material;

	private List<Vector4> Hitpoints;

	private MeshRenderer m_meshRenderer;

	private float time;

	private void Start()
	{
		time = Time.time;
		points = new Vector4[30];
		Hitpoints = new List<Vector4>();
		m_meshRenderer = GetComponent<MeshRenderer>();
		m_material = m_meshRenderer.material;
	}

	private void Update()
	{
		m_material.SetVectorArray("_Points", points);
		Hitpoints = (from s in Hitpoints
			select new Vector4(s.x, s.y, s.z, s.w + Time.deltaTime / ImpactLife) into w
			where w.w <= 1f
			select w).ToList();
		if (Time.time > time + 0.1f)
		{
			time = Time.time;
			AddEmpty();
		}
		Hitpoints.ToArray().CopyTo(points, 0);
	}

	public void AddHitObject(Vector3 position)
	{
		position -= base.transform.position;
		position = position.normalized / 2f;
		Hitpoints.Add(new Vector4(position.x, position.y, position.z, 0f));
	}

	public void AddEmpty()
	{
		Hitpoints.Add(new Vector4(0f, 0f, 0f, 0f));
	}
}

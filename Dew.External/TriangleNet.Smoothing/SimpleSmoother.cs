using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Topology;
using TriangleNet.Topology.DCEL;
using TriangleNet.Voronoi;

namespace TriangleNet.Smoothing;

public class SimpleSmoother : ISmoother
{
	private TrianglePool pool;

	private Configuration config;

	private IVoronoiFactory factory;

	private ConstraintOptions options;

	public SimpleSmoother()
		: this(new VoronoiFactory())
	{
	}

	public SimpleSmoother(IVoronoiFactory factory)
	{
		this.factory = factory;
		pool = new TrianglePool();
		config = new Configuration(() => RobustPredicates.Default, () => pool.Restart());
		options = new ConstraintOptions
		{
			ConformingDelaunay = true
		};
	}

	public SimpleSmoother(IVoronoiFactory factory, Configuration config)
	{
		this.factory = factory;
		this.config = config;
		options = new ConstraintOptions
		{
			ConformingDelaunay = true
		};
	}

	public void Smooth(IMesh mesh)
	{
		Smooth(mesh, 10);
	}

	public void Smooth(IMesh mesh, int limit)
	{
		Mesh smoothedMesh = (Mesh)mesh;
		GenericMesher mesher = new GenericMesher(config);
		IPredicates predicates = config.Predicates();
		options.SegmentSplitting = smoothedMesh.behavior.NoBisect;
		for (int i = 0; i < limit; i++)
		{
			Step(smoothedMesh, factory, predicates);
			smoothedMesh = (Mesh)mesher.Triangulate(Rebuild(smoothedMesh), options);
			factory.Reset();
		}
		smoothedMesh.CopyTo((Mesh)mesh);
	}

	private void Step(Mesh mesh, IVoronoiFactory factory, IPredicates predicates)
	{
		foreach (Face face in new BoundedVoronoi(mesh, factory, predicates).Faces)
		{
			if (face.generator.label == 0)
			{
				Centroid(face, out var x, out var y);
				face.generator.x = x;
				face.generator.y = y;
			}
		}
	}

	private void Centroid(Face face, out double x, out double y)
	{
		double atmp = 0.0;
		double xtmp = 0.0;
		double ytmp = 0.0;
		HalfEdge edge = face.Edge;
		int first = edge.Next.ID;
		do
		{
			Point p = edge.Origin;
			Point q = edge.Twin.Origin;
			double ai = p.x * q.y - q.x * p.y;
			atmp += ai;
			xtmp += (q.x + p.x) * ai;
			ytmp += (q.y + p.y) * ai;
			edge = edge.Next;
		}
		while (edge.Next.ID != first);
		x = xtmp / (3.0 * atmp);
		y = ytmp / (3.0 * atmp);
	}

	private Polygon Rebuild(Mesh mesh)
	{
		Polygon data = new Polygon(mesh.vertices.Count);
		foreach (global::TriangleNet.Geometry.Vertex v in mesh.vertices.Values)
		{
			v.type = VertexType.InputVertex;
			data.Points.Add(v);
		}
		List<ISegment> subsegs = new List<SubSegment>(mesh.subsegs.Values).ConvertAll((Converter<SubSegment, ISegment>)((SubSegment x) => x));
		data.Segments.AddRange(subsegs);
		data.Holes.AddRange(mesh.holes);
		data.Regions.AddRange(mesh.regions);
		return data;
	}
}

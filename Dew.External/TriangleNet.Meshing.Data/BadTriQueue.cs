using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Data;

internal class BadTriQueue
{
	private const double SQRT2 = 1.4142135623730951;

	private BadTriangle[] queuefront;

	private BadTriangle[] queuetail;

	private int[] nextnonemptyq;

	private int firstnonemptyq;

	private int count;

	public int Count => count;

	public BadTriQueue()
	{
		queuefront = new BadTriangle[4096];
		queuetail = new BadTriangle[4096];
		nextnonemptyq = new int[4096];
		firstnonemptyq = -1;
		count = 0;
	}

	public void Enqueue(BadTriangle badtri)
	{
		count++;
		double length;
		int posexponent;
		if (badtri.key >= 1.0)
		{
			length = badtri.key;
			posexponent = 1;
		}
		else
		{
			length = 1.0 / badtri.key;
			posexponent = 0;
		}
		int exponent = 0;
		while (length > 2.0)
		{
			int expincrement = 1;
			double multiplier = 0.5;
			while (length * multiplier * multiplier > 1.0)
			{
				expincrement *= 2;
				multiplier *= multiplier;
			}
			exponent += expincrement;
			length *= multiplier;
		}
		exponent = 2 * exponent + ((length > 1.4142135623730951) ? 1 : 0);
		int queuenumber = ((posexponent <= 0) ? (2048 + exponent) : (2047 - exponent));
		if (queuefront[queuenumber] == null)
		{
			if (queuenumber > firstnonemptyq)
			{
				nextnonemptyq[queuenumber] = firstnonemptyq;
				firstnonemptyq = queuenumber;
			}
			else
			{
				int i;
				for (i = queuenumber + 1; queuefront[i] == null; i++)
				{
				}
				nextnonemptyq[queuenumber] = nextnonemptyq[i];
				nextnonemptyq[i] = queuenumber;
			}
			queuefront[queuenumber] = badtri;
		}
		else
		{
			queuetail[queuenumber].next = badtri;
		}
		queuetail[queuenumber] = badtri;
		badtri.next = null;
	}

	public void Enqueue(ref Otri enqtri, double minedge, Vertex apex, Vertex org, Vertex dest)
	{
		BadTriangle newbad = new BadTriangle();
		newbad.poortri = enqtri;
		newbad.key = minedge;
		newbad.apex = apex;
		newbad.org = org;
		newbad.dest = dest;
		Enqueue(newbad);
	}

	public BadTriangle Dequeue()
	{
		if (firstnonemptyq < 0)
		{
			return null;
		}
		count--;
		BadTriangle result = queuefront[firstnonemptyq];
		queuefront[firstnonemptyq] = result.next;
		if (result == queuetail[firstnonemptyq])
		{
			firstnonemptyq = nextnonemptyq[firstnonemptyq];
		}
		return result;
	}
}

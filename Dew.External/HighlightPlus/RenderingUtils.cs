using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HighlightPlus;

public static class RenderingUtils
{
	private static Mesh _fullScreenMesh;

	private static Matrix4x4 matrix4x4Identity = Matrix4x4.identity;

	private static Mesh fullscreenMesh
	{
		get
		{
			if (_fullScreenMesh != null)
			{
				return _fullScreenMesh;
			}
			float num = 1f;
			float num2 = 0f;
			_fullScreenMesh = new Mesh();
			_fullScreenMesh.SetVertices(new List<Vector3>
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(1f, -1f, 0f),
				new Vector3(1f, 1f, 0f)
			});
			_fullScreenMesh.SetUVs(0, new List<Vector2>
			{
				new Vector2(0f, num2),
				new Vector2(0f, num),
				new Vector2(1f, num2),
				new Vector2(1f, num)
			});
			_fullScreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, calculateBounds: false);
			_fullScreenMesh.UploadMeshData(markNoLongerReadable: true);
			return _fullScreenMesh;
		}
	}

	public static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex)
	{
		destination = new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1);
		cmd.SetRenderTarget(destination);
		cmd.SetGlobalTexture(ShaderParams.MainTex, source);
		cmd.DrawMesh(fullscreenMesh, matrix4x4Identity, material, 0, passIndex);
	}
}

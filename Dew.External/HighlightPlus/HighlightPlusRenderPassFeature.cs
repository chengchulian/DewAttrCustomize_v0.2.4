using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HighlightPlus;

public class HighlightPlusRenderPassFeature : ScriptableRendererFeature
{
	private class HighlightPass : ScriptableRenderPass
	{
		private class PassData
		{
			public Camera camera;

			public RenderTargetIdentifier colorTarget;

			public RenderTargetIdentifier depthTarget;

			public bool clearStencil;

			public CommandBuffer cmd;
		}

		private class DistanceComparer : IComparer<HighlightEffect>
		{
			public Vector3 camPos;

			public int Compare(HighlightEffect e1, HighlightEffect e2)
			{
				Vector3 position = e1.transform.position;
				float dx1 = position.x - camPos.x;
				float dy1 = position.y - camPos.y;
				float dz1 = position.z - camPos.z;
				float distE1 = dx1 * dx1 + dy1 * dy1 + dz1 * dz1 + e1.sortingOffset;
				Vector3 position2 = e2.transform.position;
				float dx2 = position2.x - camPos.x;
				float dy2 = position2.y - camPos.y;
				float dz2 = position2.z - camPos.z;
				float distE2 = dx2 * dx2 + dy2 * dy2 + dz2 * dz2 + e2.sortingOffset;
				if (distE1 > distE2)
				{
					return -1;
				}
				if (distE1 < distE2)
				{
					return 1;
				}
				return 0;
			}
		}

		private readonly PassData passData = new PassData();

		public bool usesCameraOverlay;

		private ScriptableRenderer renderer;

		private RenderTextureDescriptor cameraTextureDescriptor;

		private static DistanceComparer effectDistanceComparer = new DistanceComparer();

		private bool clearStencil;

		private static int frameCount;

		public void Setup(HighlightPlusRenderPassFeature passFeature, ScriptableRenderer renderer)
		{
			base.renderPassEvent = passFeature.renderPassEvent;
			clearStencil = passFeature.clearStencil;
			this.renderer = renderer;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			this.cameraTextureDescriptor = cameraTextureDescriptor;
			ConfigureInput(ScriptableRenderPassInput.Depth);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTargetIdentifier cameraDepthTarget = renderer.cameraDepthTarget;
			passData.clearStencil = clearStencil;
			passData.camera = renderingData.cameraData.camera;
			passData.colorTarget = cameraColorTarget;
			passData.depthTarget = cameraDepthTarget;
			CommandBuffer cmd = CommandBufferPool.Get("Highlight Plus");
			cmd.Clear();
			passData.cmd = cmd;
			ExecutePass(passData);
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		private static void ExecutePass(PassData passData)
		{
			int count = HighlightEffect.effects.Count;
			if (count == 0)
			{
				return;
			}
			Camera cam = passData.camera;
			int camLayer = 1 << cam.gameObject.layer;
			CameraType camType = cam.cameraType;
			if (!HighlightEffect.customSorting && ((camType == CameraType.Game && frameCount++ % 10 == 0) || !Application.isPlaying))
			{
				effectDistanceComparer.camPos = cam.transform.position;
				HighlightEffect.effects.Sort(effectDistanceComparer);
			}
			for (int k = 0; k < count; k++)
			{
				HighlightEffect effect = HighlightEffect.effects[k];
				if ((effect.ignoreObjectVisibility || effect.isVisible) && effect.isActiveAndEnabled && (camType != CameraType.Reflection || effect.reflectionProbes) && ((int)effect.camerasLayerMask & camLayer) != 0)
				{
					effect.SetCommandBuffer(passData.cmd);
					effect.BuildCommandBuffer(passData.camera, passData.colorTarget, passData.depthTarget, passData.clearStencil);
					passData.clearStencil = false;
				}
			}
		}
	}

	private HighlightPass renderPass;

	public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

	[Tooltip("Clears stencil buffer before rendering highlight effects. This option can solve compatibility issues with shaders that also use stencil buffers.")]
	public bool clearStencil;

	[Tooltip("If enabled, effects will be visible also in Edit mode (when not in Play mode).")]
	public bool previewInEditMode = true;

	[Tooltip("If enabled, effects will be visible also in Preview camera (preview camera shown when a camera is selected in Editor).")]
	public bool showInPreviewCamera = true;

	public static bool installed;

	public static bool showingInEditMode;

	private const string PREVIEW_CAMERA_NAME = "Preview Camera";

	private void OnDisable()
	{
		installed = false;
	}

	public override void Create()
	{
		renderPass = new HighlightPass();
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		showingInEditMode = previewInEditMode;
		Camera cam = renderingData.cameraData.camera;
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			renderPass.usesCameraOverlay = cam.GetUniversalAdditionalCameraData().cameraStack.Count > 0;
		}
		renderPass.Setup(this, renderer);
		renderer.EnqueuePass(renderPass);
		installed = true;
	}
}

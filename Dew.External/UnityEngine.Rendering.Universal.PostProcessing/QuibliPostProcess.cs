using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.PostProcessing;

public class QuibliPostProcess : ScriptableRendererFeature
{
	[Serializable]
	public class Settings
	{
		[SerializeField]
		public List<string> renderersAfterOpaqueAndSky;

		[SerializeField]
		public List<string> renderersBeforePostProcess;

		[SerializeField]
		public List<string> renderersAfterPostProcess;

		public Settings()
		{
			renderersAfterOpaqueAndSky = new List<string>();
			renderersBeforePostProcess = new List<string>();
			renderersAfterPostProcess = new List<string>();
		}
	}

	[SerializeField]
	public Settings settings = new Settings();

	private CompoundPass _afterOpaqueAndSky;

	private CompoundPass _beforePostProcess;

	private CompoundPass _afterPostProcess;

	private RenderTargetHandle _afterPostProcessColor;

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.postProcessEnabled)
		{
			if (_afterOpaqueAndSky.HasPostProcessRenderers && _afterOpaqueAndSky.PrepareRenderers(in renderingData))
			{
				renderer.EnqueuePass(_afterOpaqueAndSky);
			}
			if (_beforePostProcess.HasPostProcessRenderers && _beforePostProcess.PrepareRenderers(in renderingData))
			{
				renderer.EnqueuePass(_beforePostProcess);
			}
			if (_afterPostProcess.HasPostProcessRenderers && _afterPostProcess.PrepareRenderers(in renderingData))
			{
				renderer.EnqueuePass(_afterPostProcess);
			}
			SetupRenderPassesCore(renderer, in renderingData);
		}
	}

	private void SetupRenderPassesCore(ScriptableRenderer renderer, in RenderingData renderingData)
	{
		RenderTargetIdentifier cameraTarget = renderer.cameraColorTarget;
		if (_afterOpaqueAndSky.HasPostProcessRenderers && _afterOpaqueAndSky.PrepareRenderers(in renderingData))
		{
			_afterOpaqueAndSky.Setup(cameraTarget, cameraTarget);
		}
		if (_beforePostProcess.HasPostProcessRenderers && _beforePostProcess.PrepareRenderers(in renderingData))
		{
			_beforePostProcess.Setup(cameraTarget, cameraTarget);
		}
		if (_afterPostProcess.HasPostProcessRenderers && _afterPostProcess.PrepareRenderers(in renderingData))
		{
			_afterPostProcess.Setup(cameraTarget, cameraTarget);
		}
	}

	public override void Create()
	{
		_afterPostProcessColor.Init("_AfterPostProcessTexture");
		Dictionary<string, CompoundRenderer> shared = new Dictionary<string, CompoundRenderer>();
		_afterOpaqueAndSky = new CompoundPass(InjectionPoint.AfterOpaqueAndSky, InstantiateRenderers(settings.renderersAfterOpaqueAndSky, shared));
		_beforePostProcess = new CompoundPass(InjectionPoint.BeforePostProcess, InstantiateRenderers(settings.renderersBeforePostProcess, shared));
		_afterPostProcess = new CompoundPass(InjectionPoint.AfterPostProcess, InstantiateRenderers(settings.renderersAfterPostProcess, shared));
	}

	private List<CompoundRenderer> InstantiateRenderers(List<string> names, Dictionary<string, CompoundRenderer> shared)
	{
		List<CompoundRenderer> renderers = new List<CompoundRenderer>(names.Count);
		foreach (string n in names)
		{
			if (shared.TryGetValue(n, out var renderer))
			{
				renderers.Add(renderer);
				continue;
			}
			Type type = Type.GetType(n);
			if (type == null || !type.IsSubclassOf(typeof(CompoundRenderer)))
			{
				continue;
			}
			CompoundRendererFeatureAttribute attribute = CompoundRendererFeatureAttribute.GetAttribute(type);
			if (attribute != null)
			{
				renderer = Activator.CreateInstance(type) as CompoundRenderer;
				renderers.Add(renderer);
				if (attribute.ShareInstance)
				{
					shared.Add(n, renderer);
				}
			}
		}
		return renderers;
	}
}
